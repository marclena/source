using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vueling.XXX.Library.Entities;

[assembly: InternalsVisibleTo("Vueling.XXX.DB.Infrastructure.UnitTest")]
namespace Vueling.XXX.DB.Infrastructure.MapFactories.MapDomainToConceptualModel.FleetRepository
{
    class SeatEntityToSeatDbObjectFactory : MapDomainToConceptualModelFactoryBase
    {

        internal override TOutput GetDbObjectFromEntity<TInput, TOutput>(TInput entity)
        {
            if (entity == null) { return default(TOutput); }

            var entitiesSeats = entity as List<Seat>;//if casting fail, return null, so we have to verify the result
            if (entitiesSeats == null) { throw new InvalidCastException("Cast to type List<Seat> has fail."); }

            return string.Join(",", entitiesSeats.Select(x => x.Row + x.Column).ToArray()) as TOutput;
        }

    }
}
