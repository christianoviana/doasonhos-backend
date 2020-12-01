using Microsoft.AspNetCore.Mvc.ModelBinding;
using PucMinas.Services.Charity.Domain.Results.Exceptions;
using System.Net;

namespace PucMinas.Services.Charity.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static ErrorMessage ToErrorMessage(this ModelStateDictionary modelStateDictionary)
        {
            if (!modelStateDictionary.IsValid)
            {
                ErrorMessage errorMessage = new ErrorMessage((int)HttpStatusCode.BadRequest, "There are invalid parameters.");

                foreach (var value in modelStateDictionary)
                {
                    foreach (var erro in value.Value.Errors)
                    {
                        errorMessage.Details.Add($"{value.Key} - {erro.ErrorMessage}");
                    }
                }

                return errorMessage;
            }

            return null;          
        }
    }
}
