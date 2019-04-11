using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Vueling.XXX.Library.Entities;
using System.Linq;
using Vueling.XXX.EF.DB.Infrastructure.IntegrationTest.TestServices;

namespace Vueling.XXX.EF.DB.Infrastructure.IntegrationTest.GivenContextWithLazyLoadingEnabled
{
    [TestClass]
    public class WhenTrackingDisabled
    {
        static Vueling.XXX.Library.InfrastructureContracts.IUnitOfWorkBooking Sut;
        static Vueling.XXX.EF.DB.Infrastructure.Configuration.IXXXInfrastructureConfiguration _Configuration;//to force copy of assembly
        static Vueling.DBAccess.Contracts.ServiceLibrary.IRepository<Booking> _BookingRepository;

        static bool trackingEnabled;

        [ClassInitialize]
        public static void ClassInitialize(TestContext TestContext)
        {
            trackingEnabled = false;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Sut = ServiceLocator.Resolve<Vueling.XXX.Library.InfrastructureContracts.IUnitOfWorkBooking>();
            _BookingRepository = Sut.GetRepository<Vueling.XXX.Library.Entities.Booking>();
        }

        [TestMethod]
        public void InsertManyTest()
        {
            int amount = 10;

            var newBookings = Vueling.XXX.EF.DB.Infrastructure.IntegrationTest.MocksFactory.InMemoryBookingProvider.CreateBatch(amount);

            _BookingRepository.InsertMany(newBookings);

            int saved = Sut.Save();

            Assert.IsTrue(saved == amount * 3);
        }

        [TestMethod]
        public void FirstOrDefaultTest()
        {
            var booking = _BookingRepository.FirstOrDefault(x => x.Id > 0, trackingEnabled);

            Assert.IsNotNull(booking);
        }

        [TestMethod]
        [Ignore]//Execute in controlled conditions. Need data in database to complete successfuly.
        public void UpdateRootEntityTest()
        {
            var booking = _BookingRepository.FirstOrDefault(x => x.Id > 0, trackingEnabled);

            booking.Modified = DateTime.Now;

            _BookingRepository.UpdateRootEntity(booking);

            var saved = Sut.Save();

            Assert.IsTrue(saved == 1);
        }

        [TestMethod]
        [Ignore]//Execute in controlled conditions. Need data in database to complete successfuly.
        public void UpdateManyRootEntitiesTest()
        {
            var bookings = _BookingRepository.Get(x => x.Id > 0, trackingEnabled: trackingEnabled).ToList();

            bookings.ForEach(booking => booking.Modified = DateTime.Now);

            _BookingRepository.UpdateManyRootEntities(bookings);

            var saved = Sut.Save();

            Assert.IsTrue(saved > 0);
        }

        [TestMethod]
        public void GetTest()
        {
            int page = 1;
            var pageSize = 2;

            var booking = _BookingRepository.Get(
                x => x.Journeys.Any() && x.Passengers.Any(),
                x => x.OrderBy(z => z.Id),
                new List<System.Linq.Expressions.Expression<Func<Booking, object>>> { y => y.Passengers, z => z.Journeys },
                page,
                pageSize,
                trackingEnabled).ToList();

            Assert.IsTrue(booking.Count == pageSize);
        }

        [TestMethod]
        [Ignore]//Execute in controlled conditions. Need data in database to complete successfuly.
        public void DeleteTest()
        {
            var booking = _BookingRepository.FirstOrDefault(x => x.Id > 0, trackingEnabled);
            
            _BookingRepository.Delete(booking.Id);

            var deleted = Sut.Save();

            Assert.IsTrue(deleted == 1);
        }

        [TestMethod]
        [Ignore]//Execute in controlled conditions. Need data in database to complete successfuly.
        public void DeleteManyTest()
        {
            int page = 1;
            int pageSize = 30;

            var bookings = _BookingRepository.Get(
                filter : null,//x => x.Journeys.Any() && x.Passengers.Any() && x.Passengers.Count >= 2,
                orderBy: x => x.OrderBy(z => z.Id),

                //new List<System.Linq.Expressions.Expression<Func<Booking, object>>> { y => y.Passengers, z => z.Journeys },
                //new List<System.Linq.Expressions.Expression<Func<Booking, object>>> { z => z.Journeys }, 
                includeProperties : null,

                page: page,
                pageSize: pageSize,
                trackingEnabled : trackingEnabled);

            _BookingRepository.DeleteMany(bookings);

            var deleted = Sut.Save();

            Assert.IsTrue(deleted > 0, "Ensure that filter is true to execute this test. All bookings that meet filter conditions must be deleted.");
        }

        [TestMethod]
        [Ignore]
        public void UpdateGraphTest()
        {
            var booking = _BookingRepository.FirstOrDefault(x => x.Id > 0, trackingEnabled);

            _BookingRepository.UpdateGraph(booking);

            var saved = Sut.Save();

            //cuando lazyloading enabled y tracking enabled/disabled saved es cero porque detecta que no hemos hecho ningun cambio,
            //pero si lazyloading esta disabled entonces eliminara los hijos porque no los ha cargado y el updategraph verifica el agregado completo
            Assert.IsTrue(saved > 0);
        }

        [TestMethod]
        [Ignore]
        public void UpdateGraphsTest()
        {
            int page = 1;
            var pageSize = 1;

            var bookings = _BookingRepository.Get(
                x => x.Journeys.Any() && x.Passengers.Any(),
                x => x.OrderBy(z => z.Id),
                new List<System.Linq.Expressions.Expression<Func<Booking, object>>> { y => y.Passengers, z => z.Journeys },
                page,
                pageSize,
                trackingEnabled).ToList();

            foreach (var booking in bookings)
            {
                AlterBookingService.ApplyInsertDeleteAndModifyActions(booking);
            }

            _BookingRepository.UpdateGraphs(bookings);

            var saved = Sut.Save();

            Assert.IsTrue(saved > 0);
        }


    }
}
