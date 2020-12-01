using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.XXX.Library.Entities;
using Vueling.XXX.Library.Validations;
using Vueling.XXX.Library.DomainServicesContracts;
using NMock;
using Vueling.XXX.Library.DomainServicesImplementations;
using Vueling.XXX.Library.InfrastructureContracts;
using System.Linq;
using Vueling.XXX.Library.UnitTest.MocksProvider;
using Vueling.XXX.Library.Exceptions;

namespace Vueling.XXX.Library.UnitTest.GivenBookingInstance
{
    [TestClass]
    public class WhenCancelBooking
    {
        static MockFactory mockFactory;

        static IBookingCancelerDomainServices Sut;
        
        static Mock<IUnitOfWorkBookingCanceler> _UnitOfWorkBookingCanceler;
        static Mock<Vueling.DBAccess.Contracts.ServiceLibrary.IRepository<Booking>> _BookingRepository;
        static Mock<Vueling.DBAccess.Contracts.ServiceLibrary.IRepository<Journey>> _JourneyRepository;

        static Mock<IBookingFeaturesDomainServices> _BookingFeaturesDomainServices;

        [ClassInitialize]
        public static void ClassInitialize(TestContext TestContext)
        {
            mockFactory = new MockFactory();

            _UnitOfWorkBookingCanceler = mockFactory.CreateMock<IUnitOfWorkBookingCanceler>();

            _BookingRepository = mockFactory.CreateMock<Vueling.DBAccess.Contracts.ServiceLibrary.IRepository<Booking>>();
            _JourneyRepository = mockFactory.CreateMock<Vueling.DBAccess.Contracts.ServiceLibrary.IRepository<Journey>>();

            _UnitOfWorkBookingCanceler.Expects.One.Method(x => x.GetRepository<Booking>()).WillReturn(_BookingRepository.MockObject);
            _UnitOfWorkBookingCanceler.Expects.One.Method(x => x.GetRepository<Journey>()).WillReturn(_JourneyRepository.MockObject);

            _BookingFeaturesDomainServices = mockFactory.CreateMock<IBookingFeaturesDomainServices>();
            IBookingCancelerValidations _IBookingCancelerValidations = new BookingCancelerValidations(_BookingFeaturesDomainServices.MockObject);

            Sut = new BookingCancelerDomainServices(_UnitOfWorkBookingCanceler.MockObject, _IBookingCancelerValidations);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            
        }

        [TestMethod]
        public void Then_BookingIsCanceled()
        {
            mockFactory.ClearExpectations();

            string salesAgent = "web";
            int amountOfBooking = 1;

            var bookingsToCancel = BookingProvider.GetBookings(salesAgent, amountOfBooking);
            var firstModificationDate = bookingsToCancel.SingleOrDefault().Modified;
            var journeysAmountToCancel = bookingsToCancel.SingleOrDefault().Journeys.Count;

            _BookingRepository.Expects.One.Method(x => x.Get(null, null, null, null, null, false)).WithAnyArguments().WillReturn(bookingsToCancel);
            _JourneyRepository.Expects.Exactly(journeysAmountToCancel).Method(x => x.Delete(null)).WithAnyArguments();
            _BookingRepository.Expects.One.Method(x => x.UpdateRootEntity(null)).WithAnyArguments();
            _UnitOfWorkBookingCanceler.Expects.One.Method(x => x.Save()).WillReturn(journeysAmountToCancel);

            _BookingFeaturesDomainServices.Expects.One.Method(x => x.IsAgency(null)).WithAnyArguments().WillReturn(false);

            Sut.CancelBooking(bookingsToCancel.First().Id);

            Assert.IsTrue(bookingsToCancel.SingleOrDefault().Modified > firstModificationDate, "Property Modified in Booking was not modified.");

            mockFactory.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBookingOperationException))]
        public void Then_BookingIsNotCanceledWhenAgency()
        {
            mockFactory.ClearExpectations();

            string salesAgent = "agency";
            int amountOfBooking = 1;

            var bookingsToCancel = BookingProvider.GetBookings(salesAgent, amountOfBooking);
            var firstModificationDate = bookingsToCancel.SingleOrDefault().Modified;
            var journeysAmountToCancel = bookingsToCancel.SingleOrDefault().Journeys.Count;

            _BookingRepository.Expects.One.Method(x => x.Get(null, null, null, null, null, false)).WithAnyArguments().WillReturn(bookingsToCancel);
            _JourneyRepository.Expects.Exactly(journeysAmountToCancel).Method(x => x.Delete(null)).WithAnyArguments();
            _BookingRepository.Expects.One.Method(x => x.UpdateRootEntity(null)).WithAnyArguments();
            _UnitOfWorkBookingCanceler.Expects.One.Method(x => x.Save()).WillReturn(journeysAmountToCancel);

            _BookingFeaturesDomainServices.Expects.One.Method(x => x.IsAgency(null)).WithAnyArguments().WillReturn(true);

            Sut.CancelBooking(bookingsToCancel.First().Id);
        }

                
    }
}
