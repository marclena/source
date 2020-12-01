using AutoMapper;
using XX.Template.Library.Contracts.Dto;
using XX.Template.Library.Impl.Mappers.Profile;
using XX.Template.Repository.Contracts.Model;

namespace XX.Template.Library.Impl.Mappers.Extension
{
    public static class GetBookingMapper
    {
        static GetBookingMapper()
        {
            Mapper = new MapperConfiguration
                (cfg =>
                {
                    cfg.AddProfile<FooRsDtoProfile>();
                    cfg.AddProfile<NavitaireGetBookingRqProfile>();
                })
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static FooRsDto MapToFooRsDto(this NavitaireWcfBookingModel booking)
        {
            return booking == null ? null : Mapper.Map<FooRsDto>(booking);
        }

        public static NavitaireGetBookingRq MapToNavitaireGetBookingRq(this FooRqDto rq)
        {
            return rq == null ? null : Mapper.Map<NavitaireGetBookingRq>(rq);
        }
    }
}