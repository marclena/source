using AutoMapper;
using XX.Template.Repository.Contracts.Model;
using XX.Template.Repository.Impl.Mapper.Profile;
using ExternalServiceName.ExternalService.BookingManager;

namespace XX.Template.Repository.Impl.Mapper.Extension
{
    public static class GetBookingMapper
    {
        static GetBookingMapper()
        {
            Mapper = new MapperConfiguration
                (cfg =>
                {
                    cfg.AddProfile<GetBookingRequestDataProfile>();
                    cfg.AddProfile<NavitaireWcfBookingModelProfile>();
                })
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static GetBookingRequestData MapToGetBookingRequestData(this NavitaireGetBookingRq rq)
        {
            return rq == null ? null : Mapper.Map<GetBookingRequestData>(rq);
        }

        public static NavitaireWcfBookingModel MapToNavitaireWcfBookingModel(this GetBookingResponse bookingRs)
        {
            return bookingRs == null ? null : Mapper.Map<NavitaireWcfBookingModel>(bookingRs);
        }
    }
}