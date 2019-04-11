﻿using System;

namespace Vueling.XXX.WebUI.MapFactories.IQueryableMaps
{
    internal class MappingIQueryableFactory
    {
        internal static MappingBase GetFor(EnumViewModel model)
        {
            switch (model)
            {
                case EnumViewModel.BookingViewModel:
                    return new MappingBookingViewModel();
                default:
                    throw new NotImplementedException(string.Format("The mapping for type {0} is not implemented.", model));
            }
        }
    }
}
