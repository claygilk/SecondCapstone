-- Switch to master
USE master;

-- Switch to excelsio_venues database
USE excelsior_venues;

DBCC CHECKIDENT (reservation, RESEED, 0)

-- Delete all rows from all tables
DELETE FROM category_venue;
DELETE FROM category;
DELETE FROM reservation;
DELETE FROM space;
DELETE FROM venue;
DELETE FROM city;
DELETE FROM state;

-- Begin inserting test data
-- STATE --
INSERT INTO state (abbreviation, name) VALUES ('MI', 'Michigan');
INSERT INTO state (abbreviation, name) VALUES ('OH', 'Ohio');

-- City --
SET IDENTITY_INSERT city ON
INSERT INTO city (id, name, state_abbreviation) VALUES (1, 'Bona', 'MI');
INSERT INTO city (id, name, state_abbreviation) VALUES (2, 'Srulbury', 'OH');
SET IDENTITY_INSERT city OFF

-- VENUE --

SET IDENTITY_INSERT venue ON 

-- Hidden Owl Eatery
INSERT INTO venue (id, name, city_id, description) VALUES (1, 'Hidden Owl Eatery', 1, 'This venue has plenty of "property" to enjoy. Roll the dice and check out all of our spaces.');

-- Painted Squirrel Club
INSERT INTO venue (id, name, city_id, description) VALUES (2, 'Painted Squirrel Club', 1, 'Lock in your reservation now! This venue has dungeons and an underground theme. Not for the faint of heart!');
INSERT INTO venue (id, name, city_id, description) VALUES (3, 'Test Venue', 1, 'This venue is booked out for the year');

SET IDENTITY_INSERT venue OFF 

-- SPACE --
SET IDENTITY_INSERT space ON 
-- Hidden Owl Eatery Spaces
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (1, 1, 'Boardwalk1', 1, NULL, NULL, '5250', 210);
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (2, 1, 'Boardwalk2', 1, NULL, NULL, '5250', 210);
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (3, 1, 'Boardwalk3', 1, NULL, NULL, '5250', 210);
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (4, 1, 'Boardwalk4', 1, NULL, NULL, '5250', 210);
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (5, 1, 'Boardwalk5', 1, NULL, NULL, '5250', 210);
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (6, 1, 'Boardwalk6', 1, NULL, NULL, '5250', 210);
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (7, 1, 'Park Place', 1, 2, 8, '900', 60);
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (8, 1, 'Test Place', 1, 2, 8, '900', 60);

-- Painted Squirrel Club Spaces
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (9, 2, 'The Dungeon', 0, 3, 8, '7200', 240);
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (10, 2, 'Cruel Convention Hall', 0, 3, 8, '1400', 70);

-- Test Space
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) 
	       VALUES (11, 3, 'Test Space', 1, NULL, NULL, '5250', 210);


SET IDENTITY_INSERT space OFF


-- CATEGORY --
SET IDENTITY_INSERT category ON

INSERT INTO category (id, name) VALUES (1, 'Family Friendly');
INSERT INTO category (id, name) VALUES (2, 'Outdoors');

SET IDENTITY_INSERT category OFF

-- CATEGORY VENUE --
INSERT INTO category_venue (venue_id, category_id) VALUES (1, 1);
INSERT INTO category_venue (venue_id, category_id) VALUES (1, 2);
INSERT INTO category_venue (venue_id, category_id) VALUES (2, 1);

-- RESERVATION -- 
SET IDENTITY_INSERT reservation ON

-- Hidden Owl Eatery Spaces
INSERT INTO reservation (reservation_id, space_id, number_of_attendees, start_date, end_date, reserved_for) VALUES (1, 7, 165, '01-01-2021', '01-02-2021', 'Smith Family Reservation');
INSERT INTO reservation (reservation_id, space_id, number_of_attendees, start_date, end_date, reserved_for) VALUES (2, 8, 140, GETDATE()-6, GETDATE()-3, 'Lockhart Family Reservation');

-- Painted Squirrel Club Spaces
INSERT INTO reservation (reservation_id, space_id, number_of_attendees, start_date, end_date, reserved_for) VALUES (5, 9, 67, GETDATE(), GETDATE()+1, 'Claus Family Reservation');
INSERT INTO reservation (reservation_id, space_id, number_of_attendees, start_date, end_date, reserved_for) VALUES (4, 9, 40, GETDATE()-7, GETDATE()-5, 'Taylor Family Reservation');

-- Test Venue Spaces
INSERT INTO reservation (reservation_id, space_id, number_of_attendees, start_date, end_date, reserved_for) 
				 VALUES (3, 11, 165, '01-01-2021', '12-31-2021', 'The Whole Year Reservation');

SET IDENTITY_INSERT reservation OFF
