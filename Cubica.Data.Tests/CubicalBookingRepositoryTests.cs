using Cubica.Data.Repository;
using Cubica.Models.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Cubica.Data.Tests
{
    [TestFixture]
    public class CubicalBookingRepositoryTests
    {
        private CubicalBooking cubicalBooking_One;
        private CubicalBooking cubicalBooking_Two;
        private DbContextOptions<ApplicationDbContext> options;

        public CubicalBookingRepositoryTests()
        {
            cubicalBooking_One = new CubicalBooking()
            {
                FirstName = "Jack",
                LastName = "Daniel",
                Date = new DateTime(2024, 1, 1),
                Email = "jack@gmail.com",
                BookingId = 11,
                CubicalId = 1
            };

            cubicalBooking_Two = new CubicalBooking()
            {
                FirstName = "Volks",
                LastName = "Wagen",
                Date = new DateTime(2024, 2, 2),
                Email = "volks@gmail.com",
                BookingId = 22,
                CubicalId = 2
            };
        }

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                  .UseInMemoryDatabase(databaseName: "temp_Cubica").Options;
        }

        [Test]
        [Order(1)]
        public async Task SaveBooking_BookingOne_CheckValuesFromDatabase()
        {
            //arrange

            //act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new CubicalBookingRepository(context);
                await repository.Book(cubicalBooking_One);
            }

            //assert
            using (var context = new ApplicationDbContext(options))
            {
                var bookingFromDb = context.CubicalBookings.FirstOrDefault(x => x.BookingId == 11);
                Assert.AreEqual(cubicalBooking_One.BookingId, bookingFromDb.BookingId);
                Assert.AreEqual(cubicalBooking_One.FirstName, bookingFromDb.FirstName);
                Assert.AreEqual(cubicalBooking_One.LastName, bookingFromDb.LastName);
                Assert.AreEqual(cubicalBooking_One.Email, bookingFromDb.Email);
                Assert.AreEqual(cubicalBooking_One.Date, bookingFromDb.Date);
            }
        }

        [Test]
        [Order(2)]
        public async Task GetAllBooking_BookingOneAndTwo_CheckBothValuesFromDatabase()
        {
            //arrange
            var expectedResult = new List<CubicalBooking> { cubicalBooking_One, cubicalBooking_Two };

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                var repository = new CubicalBookingRepository(context);
                await repository.Book(cubicalBooking_One);
                await repository.Book(cubicalBooking_Two);
            }

            //act
            List<CubicalBooking> actualList;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new CubicalBookingRepository(context);
                var temp = await repository.GetAll(null);
                actualList = temp.ToList();
            }

            //assert
            CollectionAssert.AreEqual(expectedResult, actualList, new BookingCompare());
        }

        private class BookingCompare : IComparer
        {
            public int Compare(object x, object y)
            {
                var booking1 = (CubicalBooking)x;
                var booking2 = (CubicalBooking)y;
                if (booking1.BookingId != booking2.BookingId)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
