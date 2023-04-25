using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Core;
using System;
using NUnit.Framework;
using TechTalk.SpecFlow;
using HotelBooking.Core;
using Moq;

namespace HotelBooking.SpecFlow.StepDefinitions
{
 

    [Binding]
    public class BookingManagerSteps
    {
        private IRepository<Booking> _bookingRepository;
        private IRepository<Room> _roomRepository;
        private IBookingManager _bookingManager;
        private Booking _booking;
        private bool _bookingResult;
        private string _errorMessage;

        [Given("a hotel with available rooms")]
        public void GivenAHotelWithAvailableRooms()
        {
            _bookingRepository = new MockRepository<Booking>();
            _roomRepository = new MockRepository<Room>();
            _bookingManager = new BookingManager(_bookingRepository, _roomRepository);

            // Add sample rooms
            _roomRepository.Add(new Room { Id = 1, Description = "Room 1" });
           // _roomRepository.Add(new Room { Id = 2, Description = "Room 2" });
        }

        [Given("a room booked from (.*) to (.*)")]
        public void GivenARoomBookedFrom(string startDate, string endDate)
        {
            DateTime start = ParseDate(startDate);
            DateTime end = ParseDate(endDate);
            _bookingRepository.Add(new Booking { Id = 1, RoomId = 1, StartDate = start, EndDate = end, IsActive = true });
        }

        [When(@"I book a room from ""(.*)"" to ""(.*)""")]
        public void WhenIBookARoomFrom(string startDate, string endDate)
        {
            _booking = new Booking { StartDate = ParseDate(startDate), EndDate = ParseDate(endDate) };

            try
            {
                _bookingResult = _bookingManager.CreateBooking(_booking);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
        }

        [Then("the booking should be created successfully")]
        public void ThenTheBookingShouldBeCreatedSuccessfully()
        {
            Assert.IsTrue(_bookingResult);
        }

        [Then("the booking should not be created")]
        public void ThenTheBookingShouldNotBeCreated()
        {
            Assert.IsFalse(_bookingResult);
        }

        [Then("I should see an error message (.*)")]
        public void ThenIShouldSeeAnErrorMessage(string expectedErrorMessage)
        {
            Assert.AreEqual(expectedErrorMessage, _errorMessage);
        }



        [When(@"I book the same room from ""(.*)"" to ""(.*)""")]
        public void WhenIBookTheSameRoomFromTo(string startDate, string endDate)
        {
           
            _booking = new Booking { StartDate = ParseDate(startDate), EndDate = ParseDate(endDate) };

            try
            {
                _bookingResult = _bookingManager.CreateBooking(_booking);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
        }

       






        private DateTime ParseDate(string dateString)
        {
            dateString = dateString.Trim('"');

            if (dateString.Equals("Today"))
            {
                return DateTime.Today;
            }
            else if (dateString.StartsWith("Today+"))
            {
                int daysToAdd = int.Parse(dateString.Substring("Today+".Length));
                return DateTime.Today.AddDays(daysToAdd);
            }
            else
            {
                return DateTime.Parse(dateString);
            }
        }
    }

    // A simple MockRepository class for demonstration purposes
    public class MockRepository<T> : IRepository<T> where T : class
    {
        private List<T> _data = new List<T>();

        public void Add(T item)
        {
            _data.Add(item);
        }

        public void Edit(T entity)
        {
            throw new NotImplementedException();
        }

        public T Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            return _data;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }


}
