using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.XXX.Library.DomainServicesContracts;
using NMock;
using Vueling.XXX.Library.DomainServicesImplementations;
using System.Linq;
using Vueling.XXX.Library.Configuration;
using Vueling.XXX.Library.UnitTest.MocksProvider;

namespace Vueling.XXX.Library.UnitTest.GivenBookingInstance
{
    [TestClass]
    public class WhenRequestBookingFeatures
    {
        static MockFactory mockFactory;

        static Mock<IXXXLibraryConfiguration> _XXXLibraryConfigurationMock;
        static IBookingFeaturesDomainServices Sut;

        [ClassInitialize]
        public static void ClassInitialize(TestContext TestContext)
        {
            mockFactory = new MockFactory();

            _XXXLibraryConfigurationMock = mockFactory.CreateMock<IXXXLibraryConfiguration>();

            Sut = new BookingFeaturesDomainServices(_XXXLibraryConfigurationMock.MockObject);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            
        }

        [TestMethod]
        public void Then_IsAgencyPassWithAgencyAgent()
        {
            mockFactory.ClearExpectations();

            bool expected = true;

            string salesAgent = "Agency";
            int amountOfBooking = 1;

            var booking = BookingProvider.GetBookings(salesAgent, amountOfBooking).First();

            _XXXLibraryConfigurationMock.Expects.One.GetProperty<string>(x => x.PartialCodeForAgencyAgent).WillReturn("agency");

            bool actual = Sut.IsAgency(booking);

            Assert.IsTrue(actual == expected, "Expected to pass IsAgency feature.");

            mockFactory.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        public void Then_IsAgencyFailWithWebAgent()
        {
            mockFactory.ClearExpectations();

            bool expected = false;

            string salesAgent = "web";
            int amountOfBooking = 1;

            var booking = BookingProvider.GetBookings(salesAgent, amountOfBooking).First();

            _XXXLibraryConfigurationMock.Expects.One.GetProperty<string>(x => x.PartialCodeForAgencyAgent).WillReturn("agency");

            bool actual = Sut.IsAgency(booking);

            Assert.IsTrue(actual == expected, "Expected to fail IsAgency feature.");

            mockFactory.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        public void Then_IsCorporatePassWithCorporateAgent()
        {
            mockFactory.ClearExpectations();

            bool expected = true;

            string salesAgent = "Corporate";
            int amountOfBooking = 1;

            var booking = BookingProvider.GetBookings(salesAgent, amountOfBooking).First();

            _XXXLibraryConfigurationMock.Expects.One.GetProperty<string>(x => x.PartialCodeForCorporateAgent).WillReturn("corp");

            bool actual = Sut.IsCorporate(booking);

            Assert.IsTrue(actual == expected, "Expected to pass IsCorporate feature.");

            mockFactory.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        public void Then_IsCorporateFailWithWebAgent()
        {
            mockFactory.ClearExpectations();

            bool expected = false;

            string salesAgent = "web";
            int amountOfBooking = 1;

            var booking = BookingProvider.GetBookings(salesAgent, amountOfBooking).First();

            _XXXLibraryConfigurationMock.Expects.One.GetProperty<string>(x => x.PartialCodeForCorporateAgent).WillReturn("corp");

            bool actual = Sut.IsCorporate(booking);

            Assert.IsTrue(actual == expected, "Expected to fail IsCorporate feature.");

            mockFactory.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        public void Then_IsEnabledToAddNewJourneysPassWithJourneysAllowed()
        {
            mockFactory.ClearExpectations();

            bool expected = false;

            string salesAgent = "Corporate";
            int amountOfBooking = 1;

            var booking = BookingProvider.GetBookings(salesAgent, amountOfBooking).First();

            var maxJourneysAllowedByBooking = booking.Journeys.Count;

            _XXXLibraryConfigurationMock.Expects.One.GetProperty<int>(x => x.MaxJourneysAllowedByBooking).WillReturn(maxJourneysAllowedByBooking);

            bool actual = Sut.IsEnabledToAddNewJourneys(booking);

            Assert.IsTrue(actual == expected, "Expected to pass IsEnabledToAddNewJourneys feature.");

            mockFactory.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        public void Then_IsEnabledToAddNewJourneysFailWithMoreThanJourneysAllowed()
        {
            mockFactory.ClearExpectations();

            bool expected = false;

            string salesAgent = "Corporate";
            int amountOfBooking = 1;

            var booking = BookingProvider.GetBookings(salesAgent, amountOfBooking).First();

            var maxJourneysAllowedByBooking = booking.Journeys.Count - 1;

            _XXXLibraryConfigurationMock.Expects.One.GetProperty<int>(x => x.MaxJourneysAllowedByBooking).WillReturn(maxJourneysAllowedByBooking);

            bool actual = Sut.IsEnabledToAddNewJourneys(booking);

            Assert.IsTrue(actual == expected, "Expected to fail IsEnabledToAddNewJourneys feature.");

            mockFactory.VerifyAllExpectationsHaveBeenMet();
        }
        
    }
}
