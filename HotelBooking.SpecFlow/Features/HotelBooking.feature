Feature: HotelBooking
    As a user
    I want to create and view hotel bookings
    So that I can book a room and check availability

# Decision Table Testing
Scenario: SD and ED before fully occupied days
    Given a hotel with available rooms
    And a room booked from "Today+10" to "Today+15"
    When I book the same room from "Today+1" to "Today+7"
    Then the booking should be created successfully

Scenario: SD and ED after fully occupied days
    Given a hotel with available rooms
    And a room booked from "Today+10" to "Today+15"
    When I book the same room from "Today+16" to "Today+19"
    Then the booking should be created successfully
    
Scenario: SD and ED in the fully occupied days
    Given a hotel with available rooms
    And a room booked from "Today+10" to "Today+15"
    When I book the same room from "Today+11" to "Today+13"
    Then the booking should not be created

Scenario: SD before and ED is the first occupied day
    Given a hotel with available rooms
    And a room booked from "Today+10" to "Today+15"
    When I book the same room from "Today+3" to "Today+10"
    Then the booking should not be created


Scenario: SD before and ED is the last occupied day
    Given a hotel with available rooms
    And a room booked from "Today+10" to "Today+15"
    When I book the same room from "Today+3" to "Today+15"
    Then the booking should not be created
    
    
Scenario: SD before and ED is after the last occupied day
    Given a hotel with available rooms
    And a room booked from "Today+10" to "Today+15"
    When I book the same room from "Today+3" to "Today+17"
    Then the booking should not be created
    
    
Scenario: SD on the first occupied day and ED is after the last occupied day
    Given a hotel with available rooms
    And a room booked from "Today+10" to "Today+15"
    When I book the same room from "Today+10" to "Today+17"
    Then the booking should not be created
    
Scenario: SD on the last occupied day and ED is after the last occupied day
    Given a hotel with available rooms
    And a room booked from "Today+10" to "Today+15"
    When I book the same room from "Today+15" to "Today+17"
    Then the booking should not be created
    
      

    