using Newtonsoft.Json;
using StarWars.Domain.Entities;
using System.Collections.Generic;

namespace StarWars.Service.External.SWApi.Entities
{
    public class StarshipSWApi
    {
        [JsonProperty("results")]
        public List<Starship> Starships { get; set; }

        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
    }
}
