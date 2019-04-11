using Vueling.Extensions.Library.DI;
using Vueling.XXX.EF.DB.Infrastructure.BoundedContexts;
using Vueling.XXX.EF.DB.Infrastructure.Configuration;
using Vueling.XXX.EF.DB.Infrastructure.Repositories;

namespace Vueling.XXX.EF.DB.Infrastructure.UnitOfWorks
{
    [RegisterContextAttribute]
    public class UnitOfWorkBooking : Vueling.DBAccess.EF.DB.Infrastructure.UnitOfWorkBase, 
        Vueling.XXX.Library.InfrastructureContracts.IUnitOfWorkBooking
    {
        public UnitOfWorkBooking(IXXXInfrastructureConfiguration _IXXXInfrastructureConfiguration)
            : base(new BookingContext(_IXXXInfrastructureConfiguration))
        { }

        public override Vueling.DBAccess.Contracts.ServiceLibrary.IRepository<T> GetRepository<T>()
        {
            return (Vueling.DBAccess.Contracts.ServiceLibrary.IRepository<T>)GetRepository<T>(typeof(BookingRepository));
        }
    }
}
