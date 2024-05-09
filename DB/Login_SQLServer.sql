USE db_login;

DROP TABLE IF EXISTS UserProfile;

CREATE TABLE UserProfile(
	userID INT PRIMARY KEY IDENTITY(1,1),
	code VARCHAR(250),
	username NVARCHAR(250) NOT NULL UNIQUE,
	passwrd NVARCHAR(250) NOT NULL,
	usImg VARCHAR(250),
	email VARCHAR(250),
	isDeleted BIT,
	chatRoomsCode VARCHAR(250) UNIQUE,
	usRole VARCHAR(50)
);

SELECT * FROM UserProfile;
