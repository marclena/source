using XX.Template.Repository.Contracts.Model;
using ExternalServiceName.ExternalService.BookingManager;

namespace XX.Template.Repository.Impl.Mapper.Profile
{
    public class GetBookingRequestDataProfile : AutoMapper.Profile
    {
        public GetBookingRequestDataProfile()
        {
            CreateMap<NavitaireGetBookingRq, GetBookingRequestData>()
                .ForMember(x => x.GetByRecordLocator, opt => opt.MapFrom(src => new GetByRecordLocator
                {
                    RecordLocator = src.RecordLocator
                }))
                .ForMember(x => x.GetBookingBy, opt => opt.Ignore())
                .ForMember(x => x.GetByID, opt => opt.Ignore())
                .ForMember(x => x.GetByThirdPartyRecordLocator, opt => opt.Ignore());
        }
    }
}