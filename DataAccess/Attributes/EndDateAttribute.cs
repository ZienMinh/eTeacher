using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Attributes
{
    public class EndDateAttribute : ValidationAttribute
    {
        private readonly string _startDatePropertyName;

        public EndDateAttribute(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            if (startDateProperty == null)
                throw new ArgumentException("Property with this name not found");

            var startDate = (DateOnly?)startDateProperty.GetValue(validationContext.ObjectInstance);
            var endDate = (DateOnly?)value;

            if (endDate != null && startDate != null && endDate <= startDate)
            {
                return new ValidationResult("End date must be after start date");
            }

            return ValidationResult.Success;
        }
    }
}
