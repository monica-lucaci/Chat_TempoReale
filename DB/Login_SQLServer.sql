USE db_login;

DROP TABLE IF EXISTS UserProfile;

CREATE TABLE UserProfile(
	userID INT PRIMARY KEY IDENTITY(1,1),
	code VARCHAR(250),
	username NVARCHAR(250) NOT NULL UNIQUE,
	passwrd NVARCHAR(250) NOT NULL,
	usImg TEXT,
	email VARCHAR(250),
	isDeleted BIT,
	chatRoomsCode VARCHAR(250),
	usRole VARCHAR(50)
);

SELECT * FROM UserProfile;