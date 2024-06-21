using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Stripe.Checkout;

namespace SWP391_eTeacherSystem.Pages
{
    public class PaymentModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public PaymentModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string StripePublicKey => _configuration["Stripe:PublishableKey"];

        public string RequirementsJson { get; set; }

        public void OnGet()
        {
            // Sample data - replace with your logic to fetch requirements from DB
            var requirements = new List<Requirement>
            {
                new Requirement
                {
                    Requirement_id = "REQ001",
                    User_id = "USR001",
                    Subject_name = "Mathematics",
                    Price = 5000,
                    Number_of_session = 10
                    // Add other required properties
                },
                new Requirement
                {
                    Requirement_id = "REQ002",
                    User_id = "USR002",
                    Subject_name = "Physics",
                    Price = 4500,
                    Number_of_session = 8
                    // Add other required properties
                }
            };

            RequirementsJson = JsonConvert.SerializeObject(requirements);
        }

        public async Task<IActionResult> OnPostCreateCheckoutSessionAsync([FromBody] List<Requirement> requirements)
        {
            var domain = "https://localhost:7169"; // Update with your domain

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{domain}/Payment/OrderConfirmation",
                CancelUrl = $"{domain}/Payment/Cancel",
            };

            foreach (var item in requirements)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // Convert to cents
                        Currency = "vnd", // Adjust to your currency
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Subject_name,
                        },
                    },
                    Quantity = item.Number_of_session,
                });
            }

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return new JsonResult(new { sessionId = session.Id });
        }
    }
}
