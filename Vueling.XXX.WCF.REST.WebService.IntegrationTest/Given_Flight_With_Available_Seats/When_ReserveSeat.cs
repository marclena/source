//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Vueling.XXX.Contracts.ServiceLibrary;
//using Vueling.XXX.Contracts.ServiceLibrary.DTO;
//using Vueling.XXX.DB.Infrastructure.Exceptions;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Globalization;

//[assembly: CLSCompliant(true)]
//namespace Vueling.XXX.WCF.REST.WebService.IntegrationTest.Given_Flight_With_Available_Seats
//{
//    [TestClass]
//    [DeploymentItem(@"Vueling.XXX.WCF.REST.WebService.IntegrationTest\bin\Debug\")]
//    public class When_ReserveSeat
//    {
//        private const string SUCCESS_MESSAGE_TO_CLIENT = "Seat reserved.";
//        private static string[] _listOfPossibleColumsOfSeatToAssign;
//        private string _flighIdentifier;
//        private DateTime _departureTime;
//        private static List<string> _alreadyCreatedFlights;

//        #region .: given scenario :.

//        [ClassInitialize]
//        public static void ClassInitialize(TestContext context)
//        {
//            _listOfPossibleColumsOfSeatToAssign = new[] { "A", "B", "C", "D", "E", "F" };
//            _alreadyCreatedFlights = new List<string>();
//        }

//        #endregion

//        [TestInitialize]
//        public void TestInitialize()
//        {
//            var aircraftMaintenanceApplicationService = ServiceLocator.Resolve<IAircraftMaintenanceApplicationService>();

//            CreateNewAircraftForTest(aircraftMaintenanceApplicationService);
//        }

//        private void CreateNewAircraftForTest(IAircraftMaintenanceApplicationService aircraftMaintenanceApplicationService)
//        {
//            var rand = new Random();
//            _flighIdentifier = "FN" + rand.Next(1000, 9999).ToString(CultureInfo.InvariantCulture);
//            int months = rand.Next(2, 12);
//            _departureTime = new DateTime(DateTime.Now.AddMonths(months).Year, DateTime.Now.AddMonths(months).Month, DateTime.Now.AddMonths(months).Day, DateTime.Now.AddMonths(months).Hour, DateTime.Now.AddMonths(months).Minute, 0);

//            if (!_alreadyCreatedFlights.Contains(_flighIdentifier))
//            {
//                _alreadyCreatedFlights.Add(_flighIdentifier);

//                var flight = new FlightDTO(_flighIdentifier, _departureTime);

//                aircraftMaintenanceApplicationService.CreateNewEmptyAircraft(flight);
//            }
//        }

//        [TestMethod]
//        [TestCategory("IntegrationTest"), TestCategory("Vueling.XXX.WCF.REST.WebService")]
//        public void Then_Returns_Success_Message_If_Params_Are_Ok()
//        {

//            //ARRANGE
//            var serviceClient = ServiceLocator.Resolve<ISeatReservationForAircraftsWebService>();

//            var rand = new Random();
//            int rowOfSeatToAssign = rand.Next(1, 12);
//            string columOfSeatToAssign = _listOfPossibleColumsOfSeatToAssign[rand.Next(1, _listOfPossibleColumsOfSeatToAssign.Length)];

//            //ACT
//            string actual = serviceClient.ReserveASeat(_flighIdentifier, _departureTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture), rowOfSeatToAssign.ToString(CultureInfo.InvariantCulture), columOfSeatToAssign);

//            //ASSERT
//            Assert.AreEqual(SUCCESS_MESSAGE_TO_CLIENT, actual);

//        }

//        [TestCleanup]
//        public void TestCleanup()
//        {
//            var aircraftMaintenanceApplicationService = ServiceLocator.Resolve<IAircraftMaintenanceApplicationService>();

//            var flight = new FlightDTO(_flighIdentifier, _departureTime);

//            try
//            {
//                aircraftMaintenanceApplicationService.ReleaseAircraft(flight);
//            }
//            catch (AircraftNotFoundOnDatabaseException aircraftNotFoundOnDatabaseException)
//            {
//                Trace.TraceError("Not exsiting aircraft " + aircraftNotFoundOnDatabaseException.Message);
//            }
//        }

//    }
//}
