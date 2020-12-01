using AutoMapper;
using XX.Template.Repository.Contracts.Model;
using XX.Template.Repository.Impl.Entity;
using XX.Template.Repository.Impl.Mapper.Profile;

namespace XX.Template.Repository.Impl.Mapper.Extension
{
    public static class BookingLogMapper
    {
        static BookingLogMapper()
        {
            Mapper = new MapperConfiguration
                    (cfg => { cfg.AddProfile<BookingLogProfile>(); })
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }


        public static BookingLogEntity ToEntity(this BookingLogModel model)
        {
            return model == null ? null : Mapper.Map<BookingLogEntity>(model);
        }


        public static BookingLogModel ToModel(this BookingLogEntity entity)
        {
            return entity == null ? null : Mapper.Map<BookingLogModel>(entity);
        }
    }
}