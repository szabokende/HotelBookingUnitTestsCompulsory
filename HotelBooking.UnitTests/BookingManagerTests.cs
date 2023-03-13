using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;
using Moq;
using System.Diagnostics;
using System.Linq;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        // THIS IS A COMMENT TO TEST PIPELINE TRIGGER
        private readonly BookingManager _bookingManager;
        private readonly Mock<IRepository<Booking>> _bookingRepoMock = new Mock<IRepository<Booking>>();
        private readonly Mock<IRepository<Room>> _roomRepoMock = new Mock<IRepository<Room>>();

        public BookingManagerTests()
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

        [InlineData("2023-04-03", "2023-04-10")]
   
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne(DateTime startDate, DateTime endDate)
        {

            // Arrange
            IEnumerable<Booking> bookings = new Booking[] { new Booking { Id = 1, StartDate = new DateTime(2023, 03, 16), EndDate = new DateTime(2023, 03, 30), IsActive = true, CustomerId = 1, RoomId = 1, Customer = new Customer(), Room = new Room() }, new Booking { Id = 2, StartDate = new DateTime(2023, 03, 16), EndDate = new DateTime(2023, 03, 30), IsActive = true, CustomerId = 1, RoomId = 2, Customer = new Customer(), Room = new Room() }, new Booking { Id = 3, StartDate = new DateTime(2023, 03, 16), EndDate = new DateTime(2023, 03, 30), IsActive = true, CustomerId = 1, RoomId = 3, Customer = new Customer(), Room = new Room() }, };
            IEnumerable<Room> rooms = new Room[] { new Room { Id = 1, Description = "A"}, new Room { Id = 2, Description = "B" }, new Room { Id = 3, Description = "C" }, };
            // Act
            _bookingRepoMock.Setup(x => x.GetAll()).Returns(bookings);
            _roomRepoMock.Setup(x => x.GetAll()).Returns(rooms);

            int roomId = _bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
  
            Assert.NotEqual(-1, roomId);
        }

        [Theory]
        [InlineData("2023-03-15", "2023-03-20")]
        [InlineData("2023-03-15", "2023-04-02")]
        [InlineData("2023-03-14", "2023-03-20")]
        [InlineData("2023-03-20", "2023-03-25")]
        [InlineData("2023-03-20", "2023-04-05")]// all rooms occupied
        public void FindAvailableRoom_NoRoomsAvailable_ReturnsMinusOne(DateTime startDate, DateTime endDate)
{
    // Arrange
    IEnumerable<Booking> bookings = new Booking[]
    {
        new Booking { Id = 1, StartDate = new DateTime(2023, 03, 16), EndDate = new DateTime(2023, 03, 30), IsActive = true, CustomerId = 1, RoomId = 1, Customer = new Customer(), Room = new Room() },
        new Booking { Id = 2, StartDate = new DateTime(2023, 03, 16), EndDate = new DateTime(2023, 03, 30), IsActive = true, CustomerId = 1, RoomId = 2, Customer = new Customer(), Room = new Room() },
        new Booking { Id = 3, StartDate = new DateTime(2023, 03, 16), EndDate = new DateTime(2023, 03, 30), IsActive = true, CustomerId = 1, RoomId = 3, Customer = new Customer(), Room = new Room() },
    };
    IEnumerable<Room> rooms = new Room[]
    {
        new Room { Id = 1, Description = "A"},
        new Room { Id = 2, Description = "B" },
        new Room { Id = 3, Description = "C" },
    };

    // Act
    _bookingRepoMock.Setup(x => x.GetAll()).Returns(bookings);
    _roomRepoMock.Setup(x => x.GetAll()).Returns(rooms);
    int roomId = _bookingManager.FindAvailableRoom(startDate, endDate);

    // Assert
    Assert.Equal(-1, roomId);
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
        public void GetFullyOccupiedDates_InvalidDates_ThrowsArgumentException(DateTime startDate, DateTime endDate)
        {

            // Arrange


            // Act
            Action act = () => _bookingManager.GetFullyOccupiedDates(startDate, endDate);
            var exception = Record.Exception(() => _bookingManager.GetFullyOccupiedDates(startDate, endDate));
            // Assert
            if ( startDate > endDate)
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
        public void GetFullyOccupiedDates_ValidDates_ReturnsFullyOccupiedDates(DateTime startDate, DateTime endDate)
        {

            
            // Arrange
            IEnumerable<Booking> bookings = new Booking[] { new Booking { Id = 1, StartDate = new DateTime(2023, 03, 16), EndDate = new DateTime(2023, 03, 30), IsActive = true, CustomerId = 1, RoomId = 1, Customer = new Customer(), Room = new Room {Id= 1, Description="A" } }, new Booking { Id = 2, StartDate = new DateTime(2023, 03, 16), EndDate = new DateTime(2023, 03, 30), IsActive = true, CustomerId = 1, RoomId = 2, Customer = new Customer(), Room = new Room { Id = 2, Description = "B" } }, new Booking { Id = 3, StartDate = new DateTime(2023, 03, 16), EndDate = new DateTime(2023, 03, 30), IsActive = true, CustomerId = 1, RoomId = 3, Customer = new Customer(), Room = new Room { Id = 3, Description = "C" } }, };
            IEnumerable<Room> rooms = new Room[] { new Room { Id = 1, Description = "A" }, new Room { Id = 2, Description = "B" }, new Room { Id = 3, Description = "C" }, };
         


            _bookingRepoMock.Setup(x => x.GetAll()).Returns(bookings);
            _roomRepoMock.Setup(x => x.GetAll()).Returns(rooms);
            // Act
            List<DateTime> fullyOccupiedDates = _bookingManager.GetFullyOccupiedDates(startDate, endDate);


            // Assert
            Assert.NotNull(fullyOccupiedDates);

        }


    }
}