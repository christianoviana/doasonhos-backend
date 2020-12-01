using System;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Filters
{
    public class MinDateValidatorAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);
            return !(d == DateTime.MinValue); 
        }
    }
}
