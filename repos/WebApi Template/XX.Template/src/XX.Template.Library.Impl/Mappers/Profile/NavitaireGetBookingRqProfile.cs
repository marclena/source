using XX.Template.Library.Contracts.Dto;
using XX.Template.Repository.Contracts.Model;

namespace XX.Template.Library.Impl.Mappers.Profile
{
    public class NavitaireGetBookingRqProfile : AutoMapper.Profile
    {
        public NavitaireGetBookingRqProfile()
        {
            CreateMap<FooRqDto, NavitaireGetBookingRq>()
                .ForMember(x => x.RecordLocator, opt => opt.MapFrom(src => src.RecordLocator));
        }
    }
}