CREATE TABLE Antworten (
	AntwortID UNSIGNED INT AUTO_INCREMENT NOT NULL,
	FrageID UNSIGNED INT NOT NULL,
	Antwort TEXT NOT NULL,
	Richtig BOOLEAN NOT NULL,
	PRIMARY KEY (AntwortID),
	FOREIGN KEY (FrageID) REFERENCES Fragen(FrageID)
);