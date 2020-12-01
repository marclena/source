using XX.Template.Library.Contracts.Dto;
using XX.Template.Repository.Contracts.Model;

namespace XX.Template.Library.Impl.Mappers.Profile
{
    public class FooRsDtoProfile : AutoMapper.Profile
    {
        public FooRsDtoProfile()
        {
            CreateMap<NavitaireWcfBookingModel, FooRsDto>()
                .ForMember(x => x.AgentId, opt => opt.MapFrom(src => src.AgentId))
                .ForMember(x => x.CreationDate, opt => opt.MapFrom(src => src.CreationDate));
        }
    }
}