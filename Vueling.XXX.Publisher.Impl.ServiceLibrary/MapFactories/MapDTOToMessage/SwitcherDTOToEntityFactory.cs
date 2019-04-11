using System;
using System.Globalization;

namespace Vueling.XXX.Publisher.Impl.ServiceLibrary.MapFactories.MapDTOToMessage
{
    internal class SwitcherDTOToEntityFactory
    {

        private SwitcherDTOToEntityFactory()
        {

        }

        internal static MapDTOToMessageFactoryBase GetFactoryFor(EnumMessage model)
        {
            switch (model)
            {
                case EnumMessage.Flight:
                    return new FlightDTOToMessageFactory();
                case EnumMessage.FlightCancelled:
                    return new FlightCancelledDTOToMessageFactory();
                case EnumMessage.FlightRescheduled:
                    return new FlightRescheduledDTOToMessageFactory();
                default:
                    throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "The factory for type {0} is not implemented.", model));
            }
        }
    }
}
