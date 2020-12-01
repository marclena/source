using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ATC.Log.Contracts.ServiceLibrary;
using ATC.Log.Contracts.ServiceLibrary.LogEvents;
using XX.Template.Core.Extensions;
using XX.Template.Library.Contracts;
using XX.Template.Library.Contracts.Dto;
using XX.Template.Library.Impl.Error;
using XX.Template.Library.Impl.Mappers.Extension;
using XX.Template.Repository.Contracts;
using XX.Template.Repository.Contracts.Model;
using Microsoft.Extensions.Options;


namespace XX.Template.Library.Impl
{
    public class FooService : IFooService
    {
        private readonly IBookingLogRepository _bookingLogRepository;
        private readonly ILogWrapper<FooService> _logger;
        private readonly INavitaireWcfProxy _navitaireWcfProxy;
        private readonly Dictionary<BusinessErrorType, BusinessErrorObject> _errors;


        public FooService(IBookingLogRepository bookingLogRepository,
                          IOptions<Dictionary<BusinessErrorType, BusinessErrorObject>> errorsOptions,
                          ILogWrapper<FooService> logger, 
                          INavitaireWcfProxy navitaireWcfProxy)
        {
            _bookingLogRepository = bookingLogRepository;
            _logger = logger;
            _navitaireWcfProxy = navitaireWcfProxy;
            _errors = errorsOptions.Value;
        }


        public async Task<OperationResult<FooRsDto>> DoSomethingAsync(FooRqDto data)
        {
            try
            {
                var result = new OperationResult<FooRsDto>();
          
                _logger.LogInformation(new LogEventTrace()
                {
                   Message = "DoSomethingAsync"
                });

                var login = await _navitaireWcfProxy.LogonAsync();

                if (login == null)
                {
                    result.AddError(
                        _errors[BusinessErrorType.InvalidUserOrPassword].Message,
                        _errors[BusinessErrorType.InvalidUserOrPassword].Code);
                    return result;
                }

                var bookingResponse = await _navitaireWcfProxy.GetBookingAsync(login, data.MapToNavitaireGetBookingRq());

                if (bookingResponse?.RecordLocator == null)
                {
                    result.AddError(
                        string.Format(CultureInfo.InvariantCulture, 
                        _errors[BusinessErrorType.BookingNotFound].Message, data.RecordLocator),
                        _errors[BusinessErrorType.BookingNotFound].Code);
                    return result;
                }

                _bookingLogRepository.Add(new BookingLogModel
                {
                    RecordLocator = bookingResponse.RecordLocator,
                    Date = DateTime.UtcNow,
                    Email = "test@test.com"
                });

                await _bookingLogRepository.SaveAsync();
                
                return result.AddResult(bookingResponse.MapToFooRsDto());
                   
            }
            catch (Exception e)
            {
                _logger.LogError( new LogEventExceptionError()
                {
                    Exception = e,
                    Message = e.Message
                });
                throw;
            }
        }
    }
}