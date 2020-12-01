using XX.Template.Repository.Contracts.Model;
using ExternalServiceName.ExternalService.BookingManager;

namespace XX.Template.Repository.Impl.Mapper.Profile
{
    public class NavitaireWcfBookingModelProfile : AutoMapper.Profile
    {
        public NavitaireWcfBookingModelProfile()
        {
            CreateMap<GetBookingResponse, NavitaireWcfBookingModel>()
                .ForMember(x => x.RecordLocator, opt => opt.MapFrom(x => x.Booking.RecordLocator));
        }
    }
}