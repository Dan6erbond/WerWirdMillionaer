CREATE DATABASE WhoWantsToBeAMillionaire;

USE WhoWantsToBeAMillionaire;

CREATE TABLE Categories (
	CategoryId INT UNSIGNED AUTO_INCREMENT NOT NULL,
	Name VARCHAR(100) NOT NULL,
	PRIMARY KEY (CategoryId)
);

CREATE TABLE Questions (
	QuestionId INT UNSIGNED AUTO_INCREMENT NOT NULL,
	CategoryId INT UNSIGNED NOT NULL,
	Question TEXT NOT NULL,
	PRIMARY KEY (QuestionId),
	CONSTRAINT FK_CategoryQuestion FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);

CREATE TABLE Answers (
	AnswerId INT UNSIGNED AUTO_INCREMENT NOT NULL,
	QuestionId INT UNSIGNED NOT NULL,
	Answer TEXT NOT NULL,
	Correct BOOLEAN NOT NULL,
	PRIMARY KEY (AnswerId),
	CONSTRAINT FK_QuestionAnswer FOREIGN KEY (QuestionId) REFERENCES Questions(QuestionId)
);

CREATE TABLE Users (
	UserID INT UNSIGNED AUTO_INCREMENT NOT NULL,
	Username VARCHAR(100) NOT NULL,
	IsAdmin BOOLEAN NOT NULL,
	Salt VARCHAR(32) NOT NULL,
	Password VARCHAR(64) NOT NULL,
	PRIMARY KEY (UserID)
);

CREATE TABLE Games (
	GameId INT UNSIGNED AUTO_INCREMENT NOT NULL,
	UserId INT UNSIGNED NOT NULL,
	Start DATETIME NOT NULL,
	Hidden BOOLEAN NOT NULL,
	PRIMARY KEY (GameId),
	CONSTRAINT FK_UserGame FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE Rounds (
	RoundId INT UNSIGNED AUTO_INCREMENT NOT NULL,
	GameId INT UNSIGNED NOT NULL,
	QuestionId INT UNSIGNED NOT NULL,
	AnswerId INT UNSIGNED,
	Duration INT UNSIGNED NOT NULL,
	UsedJoker BOOLEAN NOT NULL,
	PRIMARY KEY (RoundId),
	CONSTRAINT FK_GameRound FOREIGN KEY (GameId) REFERENCES Games(GameId),
	CONSTRAINT FK_QuestionRound FOREIGN KEY (QuestionId) REFERENCES Questions(QuestionId),
	CONSTRAINT FK_AnswerRound FOREIGN KEY (AnswerId) REFERENCES Answers(AnswerId)
);

CREATE TABLE CategoriesGames (
	CategoryGameId INT UNSIGNED AUTO_INCREMENT NOT NULL,
	CategoryId INT UNSIGNED NOT NULL,
	GameId INT UNSIGNED NOT NULL,
	PRIMARY KEY (CategoryGameId),
	CONSTRAINT FK_CategoryCategoryGame FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
	CONSTRAINT FK_GameCategoryGame FOREIGN KEY (GameId) REFERENCES Games(GameId)
);