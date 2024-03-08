using Cubica.Core.IServices;
using Cubica.Data.Repository.IRepository;
using Cubica.Models.Model;
using Cubica.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Cubica.Core.Services
{
    public class CubicalBookingService : ICubicalBookingService
    {
        private readonly ICubicalBookingRepository _cubicalBookingRepository;
        private readonly ICubicalRepository _cubicalRepository;

        public CubicalBookingService(ICubicalBookingRepository cubicalBookingRepository, ICubicalRepository cubicalRepository)
        {
            _cubicalBookingRepository = cubicalBookingRepository;
            _cubicalRepository = cubicalRepository;
        }

        public async Task<CubicalBookingResult> BookCubical(CubicalBooking booking)
        {
            if (booking == null) throw new ArgumentNullException(nameof(booking));

            CubicalBookingResult result = new()
            {
                FirstName = booking.FirstName,
                LastName = booking.LastName,
                Email = booking.Email,
                Date = booking.Date
            };

            var bookedCubical = await _cubicalBookingRepository.GetAll(booking.Date);
            var bookedCubicalNumber = bookedCubical.Select(x => x.CubicalId);

            var allCubical = await _cubicalRepository.GetAll();
            var availableCubical = allCubical.Where(x => !bookedCubicalNumber.Contains(x.Id));

            if (availableCubical.Any())
            {
                CubicalBooking cubicalBooking = new()
                {
                    FirstName = booking.FirstName,
                    LastName = booking.LastName,
                    Email = booking.Email,
                    Date = booking.Date
                };

                cubicalBooking.CubicalId = availableCubical.First().Id;
                await _cubicalBookingRepository.Book(cubicalBooking);
                result.BookingId = cubicalBooking.BookingId;
                result.Code = CubicalBookingCode.Success;
            }
            else
            {
                result.Code = CubicalBookingCode.NoCubicalAvailable;
            }

            return result;
        }

        public async Task<IEnumerable<CubicalBooking>> GetAllBooking()
        {
            return await _cubicalBookingRepository.GetAll(null);
        }
    }
}
