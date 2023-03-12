using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;

        public BookingManagerTests()
        {
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            // Arrange
            DateTime date = DateTime.Today;

            // Act
            Action act = () => bookingManager.FindAvailableRoom(date, date);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        //
        [Fact]
        public void FindFullyOccupiedDates_RoomIsFullyBooked_ReturnsExpectedResult()
        {
            // Arrange
            DateTime startDate = new DateTime(2023, 3, 1);
            DateTime endDate = new DateTime(2023, 3, 10);
            int roomId = 1;

            // Act
            List<DateTime> fullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);

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
            var fullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);

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
            var fullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);

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
            Action act = () => bookingManager.GetFullyOccupiedDates(startDate, endDate);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

    }
}