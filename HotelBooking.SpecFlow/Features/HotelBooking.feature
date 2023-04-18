Feature: HotelBooking
    As a user
    I want to create and view hotel bookings
    So that I can book a room and check availability

# Decision Table Testing
Scenario: Create booking with valid start and end dates, no overlapping bookings
    Given a hotel with available rooms
    When I book a room from "2023-05-01" to "2023-05-10"
    Then the booking should be created successfully

Scenario: Create booking with start date in the past
    Given a hotel with available rooms
    When I book a room from "2022-05-01" to "2023-05-10"
    Then I should see an error message The start date cannot be in the past or later than the end date.

Scenario: Create booking with end date before or equal to the start date
    Given a hotel with available rooms
    When I book a room from "2023-05-01" to "2023-04-30"
    Then I should see an error message The start date cannot be in the past or later than the end date.

Scenario: Create booking with overlapping booking dates
    Given a hotel with available rooms
    And a room booked from "2023-05-01" to "2023-05-10"
    When I book the same room from "2023-05-05" to "2023-05-15"
    Then the booking should not be created

# Equivalence Partitioning
Scenario: Create booking with valid start and end dates, room available
    Given a hotel with available rooms
    When I book a room from "2023-05-20" to "2023-05-25"
    Then the booking should be created successfully

Scenario: Create booking with start date equal to end date
    Given a hotel with available rooms
    When I book a room from "2023-05-20" to "2023-05-20"
    Then I should see an error message The start date cannot be in the past or later than the end date.

# Boundary Value Analysis
Scenario: Create booking with start date as today
    Given a hotel with available rooms
    When I book a room from "Today+1" to "Today+5"
    Then the booking should be created successfully

Scenario: Create booking with end date as start date plus one day
    Given a hotel with available rooms
    When I book a room from "2023-05-20" to "2023-05-21"
    Then the booking should be created successfully
