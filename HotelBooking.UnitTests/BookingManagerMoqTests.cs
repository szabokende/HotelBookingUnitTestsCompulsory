using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;
using Moq;
using System.Diagnostics;

namespace HotelBooking.UnitTests
{
    public class BookingManagerMoqTests
    {
        private readonly BookingManager _bookingManager;
        private readonly Mock<IRepository<Booking>> _bookingRepoMock = new Mock<IRepository<Booking>>();
        private readonly Mock<IRepository<Room>> _roomRepoMock = new Mock<IRepository<Room>>();

        public BookingManagerMoqTests()
        {
            _bookingManager = new BookingManager(_bookingRepoMock.Object, _roomRepoMock.Object);

        }

        [Theory]
        [InlineData("2023-02-01", "2023-02-05")]
        [InlineData("2023-03-05", "2023-03-15")]
        [InlineData("2023-03-13", "2023-03-15")]
        [InlineData("2023-03-15", "2023-04-02")]
        [InlineData("2023-04-03", "2023-04-10")]
        [InlineData("2023-03-14", "2023-03-20")]
        [InlineData("2023-03-20", "2023-03-25")]
        [InlineData("2023-03-20", "2023-04-05")]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException(DateTime startDate, DateTime endDate)
        {
            // Arrange
           

            // Act
            Action act = () => _bookingManager.FindAvailableRoom(startDate, endDate);
            var exception = Record.Exception(() => _bookingManager.FindAvailableRoom(startDate, endDate));
            // Assert
            if (startDate <= DateTime.Today || startDate > endDate)
            {
                Assert.Throws<ArgumentException>(act);
            }
            else
            {
                Assert.Null(exception);
            }
            
        }

        [Theory]
        [InlineData("2023-02-01", "2023-02-05")]
        [InlineData("2023-03-05", "2023-03-15")]
        [InlineData("2023-03-13", "2023-03-15")]
        [InlineData("2023-03-15", "2023-04-02")]
        [InlineData("2023-04-03", "2023-04-10")]
        [InlineData("2023-03-14", "2023-03-20")]
        [InlineData("2023-03-20", "2023-03-25")]
        [InlineData("2023-03-20", "2023-04-05")]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne(DateTime startDate, DateTime endDate)
        {
            //TODO figure out how to only check the available room scenario
            // Arrange
            IEnumerable<Booking> bookings = new Booking[] { new Booking { Id = 1, StartDate = new DateTime(2023, 03, 16), EndDate = new DateTime(2023, 03, 30), IsActive = true, CustomerId = 1, RoomId = 1, Customer = new Customer(), Room = new Room() }, new Booking { Id = 2, StartDate = new DateTime(2023, 03, 16), EndDate = new DateTime(2023, 03, 30), IsActive = true, CustomerId = 1, RoomId = 2, Customer = new Customer(), Room = new Room() }, new Booking { Id = 3, StartDate = new DateTime(2023, 03, 16), EndDate = new DateTime(2023, 03, 30), IsActive = true, CustomerId = 1, RoomId = 3, Customer = new Customer(), Room = new Room() }, };
            IEnumerable<Room> rooms = new Room[] { new Room { Id = 1, Description = "A"}, new Room { Id = 2, Description = "B" }, new Room { Id = 3, Description = "C" }, };
            
            _bookingRepoMock.Setup(x => x.GetAll()).Returns(bookings);
            _roomRepoMock.Setup(x => x.GetAll()).Returns(rooms);
            // Act
            int roomId = _bookingManager.FindAvailableRoom(DateTime.Today.AddDays(1), DateTime.Today.AddDays(1));
            Console.WriteLine("room id: " +  roomId.ToString());
            Console.WriteLine("This is the message ");
            // Assert
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        //
        [Fact]
        public void FindFullyOccupiedDates_RoomIsFullyBooked_ReturnsExpectedResult()
        {
            // Arrange
            DateTime startDate = new DateTime(2023, 3, 11);
            DateTime endDate = new DateTime(2023, 3, 21);
            int roomId = 1;

            // Act
            List<DateTime> fullyOccupiedDates = _bookingManager.GetFullyOccupiedDates(startDate, endDate);

            // Assert
            Assert.NotNull(fullyOccupiedDates);
            Assert.Equal(10, fullyOccupiedDates.Count);
            for (int i = 0; i < fullyOccupiedDates.Count; i++)
            {
                Assert.Equal(startDate.AddDays(i), fullyOccupiedDates[i]);
            }
        }

        //checks if the GetFullyOccupiedDates method returns a list of fully occupied dates
        [Fact]
        public void GetFullyOccupiedRooms_ReturnsExpectedResult()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(4);

            // Act
            var fullyOccupiedDates = _bookingManager.GetFullyOccupiedDates(startDate, endDate);

            // Assert
            Assert.Equal(new List<DateTime> { startDate, endDate }, fullyOccupiedDates);
        }

        [Fact]
        public void GetFullyOccupiedRooms_NoOccupiedDates_ReturnsEmptyList()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(14);

            // Act
            var fullyOccupiedDates = _bookingManager.GetFullyOccupiedDates(startDate, endDate);

            // Assert
            Assert.Empty(fullyOccupiedDates);
        }

        [Fact]
        public void GetFullyOccupiedRooms_InvalidDates_ThrowsArgumentException()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(5);

            // Act
            Action act = () => _bookingManager.GetFullyOccupiedDates(startDate, endDate);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

    }
}