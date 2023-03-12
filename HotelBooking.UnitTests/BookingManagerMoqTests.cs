using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;
using Moq;


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
        [InlineData("2022-01-01", "2022-01-01")]
        [InlineData("2022-02-28", "2022-03-01")]
        [InlineData("2024-12-30", "2022-12-31")]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException(DateTime startDate, DateTime endDate)
        {
            // Arrange
           

            // Act
            Action act = () => _bookingManager.FindAvailableRoom(startDate, endDate);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Theory]
        [InlineData("2026-01-01", "2026-02-01")]
        [InlineData("2025-02-28", "2025-03-01")]
        [InlineData("2024-12-30", "2023-12-31")]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne(DateTime startDate, DateTime endDate)
        {
            //TODO figure out how to only check the available room scenario
            // Arrange
            
            // Act
            int roomId = _bookingManager.FindAvailableRoom(startDate, endDate);
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