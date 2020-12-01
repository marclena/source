using System.Collections.Generic;
using System.Threading.Tasks;
using ATC.Log.Contracts.ServiceLibrary;
using XX.Template.Library.Contracts.Dto;
using XX.Template.Library.Impl;
using XX.Template.Library.Impl.Error;
using XX.Template.Repository.Contracts;
using XX.Template.Repository.Contracts.Model;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace XX.Template.LibraryName.Impl.UnitTest
{
    public class FooService_DoSomethingAsyncShould
    {
        public FooService_DoSomethingAsyncShould()
        {
            _mockNavitaireWcfProxy = new Mock<INavitaireWcfProxy>();
            _mockBookingLogRepo = new Mock<IBookingLogRepository>();
           _mockLogger = new Mock<ILogWrapper<FooService>>();
            _mockBusinessErrors = new Mock<IOptions<Dictionary<BusinessErrorType, BusinessErrorObject>>>();
        }

        private readonly Mock<INavitaireWcfProxy> _mockNavitaireWcfProxy;

        private readonly Mock<IBookingLogRepository> _mockBookingLogRepo;

        private readonly Mock<ILogWrapper<FooService>> _mockLogger;

        private readonly Mock<IOptions<Dictionary<BusinessErrorType, BusinessErrorObject>>> _mockBusinessErrors;

        [Fact]
        [Trait("Category", "UnitTest")]
        public async void ReturnOkGivenAnExistingPnr()
        {
            _mockNavitaireWcfProxy
                .Setup(x => x.LogonAsync())
                .Returns(Task.FromResult("LOGON_SIGNATURE"));

            _mockNavitaireWcfProxy
                .Setup(x => x.GetBookingAsync(It.IsAny<string>(), It.IsAny<NavitaireGetBookingRq>()))
                .ReturnsAsync(new NavitaireWcfBookingModel
                {
                    RecordLocator = "XXX"
                });

            _mockBookingLogRepo
                .Setup(x => x.Add(It.IsAny<BookingLogModel>()));

            _mockBookingLogRepo
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new BookingLogModel());


            _mockBusinessErrors
                .Setup(x => x.Value)
                .Returns(new Dictionary<BusinessErrorType, BusinessErrorObject>()
                {
                    {
                        BusinessErrorType.BookingNotFound, new BusinessErrorObject()
                        {
                            Message = "Test Msg BookingNotFound {0}",
                            Code = 200

                        }
                    },

                    {
                        BusinessErrorType.InvalidUserOrPassword, new BusinessErrorObject()
                        {
                            Message = "Test Msg InvalidUserOrPassword",
                            Code = 300

                        }
                    },
                });

            var sut = new FooService(
                _mockBookingLogRepo.Object,
                _mockBusinessErrors.Object,
                _mockLogger.Object,
                _mockNavitaireWcfProxy.Object);

            var request = new FooRqDto
            {
                Email = "a@a.com",
                RecordLocator = "xxx",
                StationCode = "EUR"
            };

            var result = await sut.DoSomethingAsync(request);

            Assert.False(result.HasErrors);
            Assert.Empty(result.Errors);
        }
    }
}