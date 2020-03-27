CREATE TABLE KategorienSpiele (
	KategorienSpieleID UNSIGNED INT AUTO_INCREMENT NOT NULL,
	KategorieID UNSIGNED INT NOT NULL,
	SpielID UNSIGNED INT NOT NULL,
	PRIMARY KEY (KategorienSpieleID),
	FOREIGN KEY (KategorieID) REFERENCES Kategorien(KategorieID),
	FOREIGN KEY (SpielID) REFRENCES Spiele(SpielID)
);