using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vueling.Messaging.Message;

namespace Vueling.XXX.Message.Commands
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CustomRules.Maintenability", "VY1001:GlobalUseDecoratedServices")]
    public class RescheduleFlight : ICommand
    {
        public RescheduleFlight()
        {
            CommandType = CommandType.Transactional;
        }

        public CommandType CommandType { get; set; }

        public string Identifier { get; set; }
        public DateTime NewDepartureDate { get; set; }
    }
}
