using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Vueling.XXX.MessageConsumer.WindowsService.MapFactories.MapMessageToApplicationDTO
{
    internal class SwitcherMessageToApplicationDTO
    {

        private SwitcherMessageToApplicationDTO()
        {

        }

        internal static MapMessageToApplicationDTOFactoryBase GetFactoryFor(MessageType model)
        {
            switch (model)
            {
                case MessageType.FlightRescheduled:
                    return new FlightRescheduledMapper();
                case MessageType.FlightCancelled:
                    return new FlightCancelledMapper();
                default:
                    throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "The factory for type {0} is not implemented.", model));
            }
        }

    }
}
