using Cubica.Core.Services;
using Cubica.Data.Repository.IRepository;
using Cubica.Models.Model;
using Cubica.Models.ViewModel;
using Moq;

namespace Cubica.Core.Tests
{
    [TestFixture]
    public class CubicalBookingServiceTests
    {
        private Mock<ICubicalBookingRepository> _cubicalBookingRepoMock;
        private Mock<ICubicalRepository> _cubicalRepoMock;
        private CubicalBookingService _bookingService;
        private CubicalBooking _request;
        private List<Cubical> _availableCubicals;

        [SetUp]
        public void Setup()
        {
            _request = new CubicalBooking
            {
                FirstName = "Ryan",
                LastName = "Joa",
                Email = "ryan@gmail.com",
                Date = new DateTime(2024, 4, 4)
            };

            _availableCubicals = new List<Cubical>
            {
                new Cubical
                {
                    Id = 10,
                    CubicalName = "A09",
                    FloorNumber = 1
                }
            };

            _cubicalBookingRepoMock = new Mock<ICubicalBookingRepository>();
            _cubicalRepoMock = new Mock<ICubicalRepository>();
            _cubicalRepoMock.Setup(c => c.GetAll()).ReturnsAsync(_availableCubicals);

            _bookingService = new CubicalBookingService(_cubicalBookingRepoMock.Object, _cubicalRepoMock.Object);
        }

        [TestCase]
        public async Task GetAllBooking_InvokeMethod_CheckIfRepoIsCalled()
        {
            await _bookingService.GetAllBooking();
            _cubicalBookingRepoMock.Verify(x => x.GetAll(null), Times.Once);
        }

        [Test]
        public async Task CubicalBooking_SavingBookingWithAvailableCubical_ReturnResultWithAllValues()
        {
            CubicalBooking savedCubicalBooking = null;
            _cubicalBookingRepoMock.Setup(x => x.Book(It.IsAny<CubicalBooking>()))
                .Callback<CubicalBooking>(booking =>
                {
                    savedCubicalBooking = booking;
                });

            //act
            await _bookingService.BookCubical(_request);

            //assert
            _cubicalBookingRepoMock.Verify(x => x.Book(It.IsAny<CubicalBooking>()), Times.Once);

            Assert.NotNull(savedCubicalBooking);
            Assert.AreEqual(_request.FirstName, savedCubicalBooking.FirstName);
            Assert.AreEqual(_request.LastName, savedCubicalBooking.LastName);
            Assert.AreEqual(_request.Email, savedCubicalBooking.Email);
            Assert.AreEqual(_request.Date, savedCubicalBooking.Date);
            Assert.AreEqual(_availableCubicals.First().Id, savedCubicalBooking.CubicalId);
        }

        [Test]
        public async Task CubicalBookingResultCheck_InputResult_ValuesMatchInResult()
        {
            CubicalBookingResult result = await _bookingService.BookCubical(_request);

            Assert.NotNull(result);
            Assert.AreEqual(_request.FirstName, result.FirstName);
            Assert.AreEqual(_request.LastName, result.LastName);
            Assert.AreEqual(_request.Email, result.Email);
            Assert.AreEqual(_request.Date, result.Date);
        }

        //[Test]
        //public async Task ResultCodeSuccess_CubicalAvailability_ReturnsSuccessResultCode()
        //{
        //    var result =await _bookingService.BookCubical(_request);

        //    Assert.AreEqual(CubicalBookingCode.Success, result.Code);
        //}

        //This is a dynamic way to write testcase for both booking codes
        [TestCase(true, ExpectedResult = CubicalBookingCode.Success)]
        [TestCase(false, ExpectedResult = CubicalBookingCode.NoCubicalAvailable)]
        public async Task<CubicalBookingCode> CubicalBookingResultCheck_InputResult_ValuesMatchInResult(bool cubicalAvailability)
        {
            if (!cubicalAvailability)
            {
                _availableCubicals.Clear();
            }

            return _bookingService.BookCubical(_request).Result.Code;
        }


        [TestCase(0, false)]
        [TestCase(55, true)]
        public async Task CubicalBooking_BookAvailableCubical_ReturnsBookingId(
            int expectedBookingId, bool cubicalAvailability)
        {
            if (!cubicalAvailability)
            {
                _availableCubicals.Clear();
            }

            _cubicalBookingRepoMock.Setup(x => x.Book(It.IsAny<CubicalBooking>()))
                .Callback<CubicalBooking>(booking =>
                {
                    booking.BookingId = 55;
                });

            var result = await _bookingService.BookCubical(_request);
            Assert.AreEqual(expectedBookingId, result.BookingId);
        }

        [Test]
        public async Task BookNotInvoked_SaveBookingWithoutAvailableCubical_BookMethodNotInvoked()
        {
                _availableCubicals.Clear();
          
            var result = await _bookingService.BookCubical(_request);
            _cubicalBookingRepoMock.Verify(x => x.Book(It.IsAny<CubicalBooking>()), Times.Never);
        }
    }
}
