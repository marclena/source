using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.XXX.Library.Entities;
using System.Collections.Generic;
using Vueling.XXX.Library.Validations;
using Vueling.XXX.Library.DomainServicesContracts;
using NMock;
using Vueling.XXX.Library.Exceptions;

namespace Vueling.XXX.Library.UnitTest.GivenBookingInstance
{
    [TestClass]
    public class WhenApplyCancelerValidations
    {
        static MockFactory mockFactory;

        static IBookingCancelerValidations Sut;
        static Mock<IBookingFeaturesDomainServices> _BookingFeaturesDomainServicesMocked;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext TestContext)
        {
            mockFactory = new MockFactory();

            _BookingFeaturesDomainServicesMocked = mockFactory.CreateMock<IBookingFeaturesDomainServices>();

            Sut = new BookingCancelerValidations(_BookingFeaturesDomainServicesMocked.MockObject);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBookingOperationException))]
        public void Then_ValidationFailWhenAgency()
        {
            mockFactory.ClearExpectations();

            _BookingFeaturesDomainServicesMocked.Expects.One.Method(x => x.IsAgency(null)).WithAnyArguments().WillReturn(true);

            Booking booking = GetNewBooking("agency" , 2);

            Sut.Validate(booking);

            mockFactory.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBookingOperationException))]
        public void Then_ValidationFailWhenFlew()
        {
            mockFactory.ClearExpectations();

            _BookingFeaturesDomainServicesMocked.Expects.One.Method(x => x.IsAgency(null)).WithAnyArguments().WillReturn(false);

            Booking booking = GetNewBooking("web", -2);

            Sut.Validate(booking);

            mockFactory.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidBookingOperationException))]
        public void Then_ValidationFailWhenJourneysNull()
        {
            mockFactory.ClearExpectations();

            _BookingFeaturesDomainServicesMocked.Expects.One.Method(x => x.IsAgency(null)).WithAnyArguments().WillReturn(false);

            Booking booking = GetNewBooking("web", 2);

            booking.Journeys.Clear();

            Sut.Validate(booking);

            mockFactory.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        public void ValidationOKWhenMeetAllConditions()
        {
            mockFactory.ClearExpectations();

            _BookingFeaturesDomainServicesMocked.Expects.One.Method(x => x.IsAgency(null)).WithAnyArguments().WillReturn(false);

            Booking booking = GetNewBooking("web", 2);

            Sut.Validate(booking);

            mockFactory.VerifyAllExpectationsHaveBeenMet();
        }

        private Booking GetNewBooking(string salesAgent, int addDays)
        {
            return new Booking
            {
                Id = 1,
                SalesAgent = salesAgent,
                Journeys = new List<Journey>
                {
                    new Journey
                    {
                        ArrivalDate = DateTime.Now.AddDays(addDays),
                        DepartureDate = DateTime.Now.AddDays(addDays)
                    }
                }
            };
        }

        
    }
}
