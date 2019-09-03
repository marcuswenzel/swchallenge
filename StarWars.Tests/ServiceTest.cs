using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarWars.Service;
using StarWars.Service.External.SWApi;

namespace StarWars.Tests
{
    [TestClass]
    public class ServiceTest
    {
        StarWarsService StarWarsService;
        
        public ServiceTest()
        {
            this.StarWarsService = new StarWarsService();
        }

        [TestMethod]
        public void VerifyIfMethodGetAllStarshipsReturnCorrectly()
        {
            var starShips = this.StarWarsService.GetAllStarships();

            Assert.IsNotNull(starShips);
            Assert.IsTrue(starShips.Count > 0);
        }

        [TestMethod]
        public void VerifyConnectionWithExternalAPIUsingValidEndpoints()
        {
            // Empty endpoint, service get endpoint from file settings
            var externalWSService = new SWApiService();

            var starShips = externalWSService.GetStarshipAsync();

            Assert.IsNotNull(starShips);
            Assert.IsTrue(starShips.Result.Count > 0);

            // Valid endpoint
            externalWSService = new SWApiService("http://swapi.co/api/starships/");

            starShips = externalWSService.GetStarshipAsync();

            Assert.IsNotNull(starShips);
            Assert.IsTrue(starShips.Result.Count > 0);
        }

        [TestMethod]
        public void VerifyConnectionWithExternalAPIUsingAnInValidEndpoints()
        {
            // Invalid endpoint
            var externalWSService = new SWApiService("http://swapi.co/api/starshipsxxx/");

            var starShips = externalWSService.GetStarshipAsync();

            Assert.IsFalse(starShips.Result.Count > 0);
        }

    }
}
