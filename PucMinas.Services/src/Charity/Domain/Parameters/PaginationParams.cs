using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.Parameters
{
    public class PaginationParams : ICloneable
    {
        private const int MAX_SIZE = 500;
        private const int MIN_SIZE = 1;

        public PaginationParams()
        {
            this.Page = 1;
            this.Size = 100;
        }
        
        [Range(MIN_SIZE, MAX_SIZE, ErrorMessage = "The page must be greater than zero")]
        [JsonProperty("page")]
        public int Page { get; set; }

        [Range(MIN_SIZE, MAX_SIZE, ErrorMessage = "The max pagination size must be between 1 to 500")]
        [JsonProperty("size")]
        public int Size { get; set; }

        public object Clone()
        {          
            var jsonString = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject(jsonString, this.GetType());
        }    
    }
}
