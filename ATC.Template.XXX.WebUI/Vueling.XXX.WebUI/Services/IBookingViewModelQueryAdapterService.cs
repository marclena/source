namespace Vueling.XXX.WebUI.Services
{
    public interface IBookingViewModelQueryAdapterService
    {
        System.Linq.IQueryable<Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO> ApplyFilters(Telerik.Web.Mvc.GridCommand command, System.Linq.IQueryable<Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO> data);
        System.Linq.IQueryable<Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO> ApplyPaging(Telerik.Web.Mvc.GridCommand command, System.Linq.IQueryable<Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO> data);
        System.Linq.IQueryable<Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO> ApplySorting(Telerik.Web.Mvc.GridCommand command, System.Linq.IQueryable<Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO> data);
    }
}
