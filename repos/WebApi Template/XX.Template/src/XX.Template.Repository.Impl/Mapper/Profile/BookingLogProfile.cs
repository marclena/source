using XX.Template.Repository.Contracts.Model;
using XX.Template.Repository.Impl.Entity;

namespace XX.Template.Repository.Impl.Mapper.Profile
{
    public class BookingLogProfile : AutoMapper.Profile
    {
        public BookingLogProfile()
        {
            CreateMap<BookingLogEntity, BookingLogModel>()
                .ForMember(model => model.Date, opt => opt.MapFrom(entity => entity.Date))
                .ForMember(model => model.Email, opt => opt.MapFrom(entity => entity.Email))
                .ForMember(model => model.RecordLocator, opt => opt.MapFrom(entity => entity.RecordLocator))
                .ReverseMap();
        }
    }
}