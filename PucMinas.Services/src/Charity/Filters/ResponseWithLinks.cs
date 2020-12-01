using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace PucMinas.Services.Charity.Filters
{
    public class ResponseWithLinks : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var provider = (LinkFilter) serviceProvider.GetService(typeof(LinkFilter));

            if (provider == null)
            {
                throw new Exception("The Link Filter provider cannot be null");
            }

            return provider;
        }
    }
}
