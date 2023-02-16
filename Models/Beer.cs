using System;
using Newtonsoft.Json;

namespace CascadeBrewingCo.Models
{
	public class Beer
	{
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}


