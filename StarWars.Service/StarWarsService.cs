using System;
using System.Collections.Generic;
using System.Linq;
using StarWars.Domain.Entities;
using StarWars.Service.External.SWApi;

namespace StarWars.Service
{
    public class StarWarsService
    {
        private SWApiService SWService = new SWApiService();

        public List<Starship> GetAllStarships()
        {
            // Get all Star Ships
            return SWService.GetStarshipAsync().Result;
        }

        public List<Starship> GetAllStarshipsWithNumberOfRessupplies(long distanceInMGLT)
        {
            var starships = SWService.GetStarshipAsync().Result;

            // calculate the number of ressupplies between planets
            foreach (Starship starship in starships)
                starship.ResupplyFrequency = CalculateStarshipsResupply(distanceInMGLT, starship);

            return starships;
        }

        private string CalculateStarshipsResupply(long distanceInMGLT, Starship starship)
        {
            // Solving 'unknown' returns
            if (!long.TryParse(starship.MGLT, out long starshipMGLT))
                return starship.MGLT;

            // Convert total consumable in hours
            DateTime JourneyStart = DateTime.Now;
            DateTime JourneyEnd = DateTime.Now;

            string[] consumableUnity = starship.Consumables.Split(' ');
            
            int consumableValue = Convert.ToInt32(consumableUnity[0]);
            string consumableType = consumableUnity[1].ToLower();

            switch (consumableType)
            {
                case "day":
                case "days":
                    JourneyEnd = JourneyEnd.AddDays(consumableValue);
                    break;
                case "week":
                case "weeks":
                    JourneyEnd = JourneyEnd.AddDays(consumableValue * 7);
                    break;
                case "month":
                case "months":
                    JourneyEnd = JourneyEnd.AddMonths(Convert.ToInt32(consumableValue));
                    break;
                case "year":
                case "years":
                    JourneyEnd = JourneyEnd.AddYears(Convert.ToInt32(consumableValue));
                    break;
            }

            int minutes = (int)(JourneyEnd - JourneyStart).TotalMinutes;
            int numHours = Enumerable.Range(0, minutes)
                .Select(min => JourneyStart.AddMinutes(min))
                .GroupBy(dt => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0, 0))
                .Count();

            // 1 MGLT is equivalent to 1 hour
            return (distanceInMGLT / (numHours * starshipMGLT)).ToString();
        }
    }
}
