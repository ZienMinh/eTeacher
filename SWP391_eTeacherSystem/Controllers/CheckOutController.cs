using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace SWP391_eTeacherSystem.Controllers
{
    public class CheckOutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

		public IActionResult OrderConfirmation()
		{
			var service = new SessionService();
			Session session = service.Get(TempData["Session"].ToString());
			if(session.PaymentStatus == "Paid")
			{
				return View("Success");
			}
			return View("Login");
        }

        public IActionResult CheckOut()
        {
			List<Requirement> orders = new List<Requirement>();

			var domain = "https://localhost:7169/";

			var options = new SessionCreateOptions
			{
				SuccessUrl = domain + $"Checkout/OderConfirmation",
				CancelUrl = domain + "Checkout/Login",
				LineItems = new List<SessionLineItemOptions>(),
				Mode = "payment",
			};

			foreach (var item in orders)
			{
				var sessionListItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(item.Price * item.Number_of_session),
						Currency = "vnd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.Subject_name.ToString(),

						}
					},
					Quantity = item.Number_of_session
				};
				options.LineItems.Add(sessionListItem);

			}

			var service = new SessionService();
			Session session = new Session();

			TempData["Session"] = session.Id;

			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult(303);
		}
    }
}
