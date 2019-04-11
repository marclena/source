using Vueling.Extensions.Library.DI;
using Vueling.XXX.EF.DB.Infrastructure.BoundedContexts;
using Vueling.XXX.EF.DB.Infrastructure.Configuration;

namespace Vueling.XXX.EF.DB.Infrastructure.UnitOfWorks
{
    [RegisterContextAttribute]
    public class UnitOfWorkBookingCanceler : Vueling.DBAccess.EF.DB.Infrastructure.UnitOfWorkBase, Vueling.XXX.Library.InfrastructureContracts.IUnitOfWorkBookingCanceler
    {
        public UnitOfWorkBookingCanceler(IXXXInfrastructureConfiguration _IXXXInfrastructureConfiguration)
            : base(new BookingCancelerContext(_IXXXInfrastructureConfiguration))
        {

        }
    }
}
