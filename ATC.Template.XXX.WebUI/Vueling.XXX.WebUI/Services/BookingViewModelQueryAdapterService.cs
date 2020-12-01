using System.ComponentModel;
using System.Linq;
using Telerik.Web.Mvc;
using Vueling.Extensions.Library.DI;

namespace Vueling.XXX.WebUI.Services
{
    [RegisterServiceAttribute]
    public class BookingViewModelQueryAdapterService : Vueling.XXX.WebUI.Services.IBookingViewModelQueryAdapterService
    {
        public IQueryable<Contracts.ServiceLibrary.DTO.BookingDTO> ApplyFilters(GridCommand command, IQueryable<Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO> data)
        {
            return TelerikQueryableExtensions.ApplyFilters<Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO>(command, data);
        }

        public IQueryable<Contracts.ServiceLibrary.DTO.BookingDTO> ApplyPaging(GridCommand command, IQueryable<Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO> data)
        {
            if (command.PageSize > 0)
            {
                data = data.Skip((command.Page - 1) * command.PageSize);
            }

            return data.Take(command.PageSize);
        }

        public IQueryable<Contracts.ServiceLibrary.DTO.BookingDTO> ApplySorting(GridCommand command, IQueryable<Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO> data)
        {
            if (!command.SortDescriptors.Any())
            {
                return data.OrderBy(item => item.Id);
            }

            //Apply sorting
            foreach (SortDescriptor sortDescriptor in command.SortDescriptors)
            {
                if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                {
                    switch (sortDescriptor.Member)
                    {
                        case "RecordLocator":
                            data = data.OrderBy(item => item.RecordLocator);
                            break;
                        case "SalesAgent":
                            data = data.OrderBy(item => item.SalesAgent);
                            break;
                        case "Created":
                            data = data.OrderBy(item => item.Created);
                            break;
                        case "Modified":
                            data = data.OrderBy(item => item.Modified);
                            break;
                        default:
                            data = data.OrderBy(item => item.Id);
                            break;
                    }
                }
                else
                {
                    switch (sortDescriptor.Member)
                    {
                        case "RecordLocator":
                            data = data.OrderByDescending(item => item.RecordLocator);
                            break;
                        case "SalesAgent":
                            data = data.OrderByDescending(item => item.SalesAgent);
                            break;
                        case "Created":
                            data = data.OrderByDescending(item => item.Created);
                            break;
                        case "Modified":
                            data = data.OrderByDescending(item => item.Modified);
                            break;
                        default:
                            data = data.OrderByDescending(item => item.Id);
                            break;
                    }
                }
            }

            return data;
        }

    }
}