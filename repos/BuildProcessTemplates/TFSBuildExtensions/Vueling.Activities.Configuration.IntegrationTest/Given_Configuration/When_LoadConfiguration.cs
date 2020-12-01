using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

[assembly: CLSCompliant(true)]
namespace Vueling.Activities.Configuration.IntegrationTest.Given_Flight_With_Available_Seats
{
    [TestClass]
    public class When_LoadConfiguration
    {
        [TestMethod]
        public void Then_Configuration_Initialize_Successfully()
        {
            var domain = Vueling.Activities.Configuration.Configuration.domain;

            Assert.AreEqual("vuelingbcn", domain);
        }
    }
}
