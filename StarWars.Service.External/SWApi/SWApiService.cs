using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StarWars.Domain.Entities;
using StarWars.Service.External.SWApi.Entities;

namespace StarWars.Service.External.SWApi
{
    public class SWApiService
    {
        private readonly string urlWSApiBase;

        public SWApiService()
        {
            this.urlWSApiBase = Properties.Settings.Default.WSApiEndpoint;
        }

        public SWApiService(string wsApiEndpoint)
        {
            this.urlWSApiBase = wsApiEndpoint;
        }

        public async Task<List<Starship>> GetStarshipAsync()
        {
            if (string.IsNullOrEmpty(this.urlWSApiBase))
                throw new Exception("Endpoint to Star Wars API not configured.");

            List<Starship> starships = new List<Starship>();

            try
            {
                var url = this.urlWSApiBase;

                HttpClient httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(url)
                };

                // Read all API pages
                var readApiCompleted = false;
                while (!readApiCompleted)
                {
                    // Calls asynchronously each page
                    await httpClient.GetAsync(url).ContinueWith(async (starshipSearchTask) =>
                    {
                        var response = await starshipSearchTask;
                        if (response.IsSuccessStatusCode)
                        {
                            // read page
                            string asyncResponse = await response.Content.ReadAsStringAsync();

                            // deserialize read to starship list objects
                            var result = JsonConvert.DeserializeObject<StarshipSWApi>(asyncResponse);
                            if (result != null)
                            {
                                // if there are starships, add to collection
                                if (result.Starships.Count > 0)
                                    starships.AddRange(result.Starships.ToList());

                                // Get the URL for the next page
                                url = result.Next;

                                // if next url is empty, read is completed
                                if (string.IsNullOrEmpty(url))
                                    readApiCompleted = true;
                            }
                        }
                        else
                            readApiCompleted = true;
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error acessing Star Wars API", ex);
            }

            return starships;
        }
    }
}
