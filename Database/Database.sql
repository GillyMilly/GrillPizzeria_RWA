-- GrillPizzeria narudzbe tablice

CREATE TABLE Roles (
    RolesId INT PRIMARY KEY IDENTITY(1,1),
    RolesName VARCHAR(100) NOT NULL
) 

CREATE TABLE Korisnik (
    Idkorisnik INT PRIMARY KEY IDENTITY(1,1),
    Ime NVARCHAR(100) NOT NULL,
    Prezime NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PwdHash NVARCHAR(255) NOT NULL,
    Salt NVARCHAR(255) NOT NULL,
    Username NVARCHAR(50) NOT NULL,
    Mobitel NVARCHAR(255),
    RolesId INT NOT NULL,
    FOREIGN KEY (RolesId) REFERENCES Roles(RolesId)
)

-- 1-N veza s Hrana
CREATE TABLE KategorijaHrane (
    IdkategorijaHrane INT PRIMARY KEY IDENTITY(1,1),
    Naziv NVARCHAR(100) NOT NULL,
    Opis NVARCHAR(255)
)

-- Glavni entitet
CREATE TABLE Hrana (
    Idhrana INT PRIMARY KEY IDENTITY(1,1),
    Naslov NVARCHAR(100) NOT NULL,
    Opis NVARCHAR(255),
    Cijena DECIMAL(10,2),
    KategorijaHraneId INT,
    FOREIGN KEY (KategorijaHraneId) REFERENCES KategorijaHrane(IdkategorijaHrane) ON DELETE CASCADE
)

CREATE TABLE Alergen (
    Idalergen INT PRIMARY KEY IDENTITY(1,1),
    Naziv NVARCHAR(100) NOT NULL
)

-- M-N izme�u Hrana i Alergen
CREATE TABLE HranaAlergen (
    IdhranaAlergen INT PRIMARY KEY IDENTITY(1,1),
    HranaId INT,
    AlergenId INT,
    FOREIGN KEY (HranaId) REFERENCES Hrana(Idhrana) ON DELETE CASCADE,
    FOREIGN KEY (AlergenId) REFERENCES Alergen(Idalergen) ON DELETE CASCADE
)

-- M-N veza s Korisnik
CREATE TABLE Narudzba (
    Idnarudzba INT PRIMARY KEY IDENTITY(1,1),
    KorisnikId INT,
    Datum DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (KorisnikId) REFERENCES Korisnik(Idkorisnik) ON DELETE CASCADE
)

-- M-N izme�u narudzba i hrana
CREATE TABLE NarudzbaHrana (
    IdnarudzbaHrana INT PRIMARY KEY IDENTITY(1,1),
    NarudzbaId INT,
    HranaId INT,
    Kolicina INT NOT NULL,
    FOREIGN KEY (NarudzbaId) REFERENCES Narudzba(Idnarudzba),
    FOREIGN KEY (HranaId) REFERENCES Hrana(Idhrana) ON DELETE CASCADE
)

-- logging funkcionalnost
CREATE TABLE Log (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Timestamp DATETIME DEFAULT GETDATE(),
    Level NVARCHAR(20) NOT NULL,
    Message NVARCHAR(500) NOT NULL
)


-- PODACI

INSERT INTO Roles (RolesName) 
VALUES('Admin'), ('User')

-- Kategorije hrane (11)
INSERT INTO KategorijaHrane (Naziv, Opis) VALUES
('Klasi�ne pizze', 'Tradicionalne pizze s osnovnim sastojcima'), --1
('Meso na �aru', 'Razna mesna jela na �aru'),
('Piletina', 'Jela od piletine'),
('Morski plodovi', 'Jela iz mora'),
('Gazirana pi�a', 'Gazirana bezalkoholna pi�a'),	--5
('Alkoholna pi�a', 'Pivo, vino i ostala alkoholna pi�a'),
('Topli napitci', 'Kava, �aj, topli �okoladni napitci'),
('Pala�inke', 'Slatke i slane pala�inke'),
('Sezonski', 'Sezonske ponude i akcije'),
('Vege', 'Vegetarijanske opcije'),	 --10
('Burgeri', 'Doma�i burgeri')		 -- 11


-- Hrana (19)
INSERT INTO Hrana (Naslov, Opis, Cijena, KategorijaHraneId) VALUES
('Capricciosa', 'Pizza s sunkom, gljivama i maslinama', 8.50, 1), -- 1
('Hawaii', 'Pizza s sunkom i ananasom', 7.50, 1),
('Quattro Stagioni', 'Pizza s cetiri razlicita dijela', 9.00, 1),
('Raznjici', 'Porcija 8 kom s lepinjom i lukom', 9.50, 2),
('Kobasice', 'Doma�e kobasice s ro�tilja', 8.00, 2),	 --5
('Pileci file', 'Pile�i file s ro�tilja i prilogom', 7.50, 3),
('Losos na zaru', 'Losos s ro�tilja i povr�em', 12.00, 4),
('Tuna salata', 'Svje�a salata s tunom i povr�em', 6.50, 4),
('Pomfrit', 'Doma�i pomfrit s umakom', 3.50, 2),
('Sprite 0.5L', 'Gazirano limunsko pi�e', 2.50, 5),	 --10
('Pivo 0.5L', 'Doma�e pivo', 3.00, 6),
('Espresso', 'Jaka kava', 1.50, 7),
('Palacinke s nutellom', 'Slatke pala�inke s nutellom', 4.50, 8),
('Zimska pizza', 'Posebna zimska pizza s toplim sastojcima', 8.00, 9),
('Vege burger', 'Burger od povr�a i soje', 6.50, 10),	 --15
('Cheeseburger', 'Burger sa sirom i prilozima', 7.50, 11),
('Bacon burger', 'Burger s slaninom i sirom', 8.50, 11),
('Vege pizza', 'Pizza s povrcem bez mesa', 7.00, 10),
('Classic burger', 'Klasicni burger s mesom', 7.00, 11) --19


-- Alergeni (16)
INSERT INTO Alergen (Naziv) VALUES
('Gluten'), -- 1
('Mlijeko'),
('Jaja'),
('Soja'),
('Kikiriki'), --5
('Ora�asti plodovi'),
('Riba'),
('�koljke'),
('Sezam'),
('Celer'), --10
('Laktoza'),
('Med'),
('Bjelanjak'),
('Riza'),
('Kukuruz') --15

-- Hrana i alergen (M-N)
INSERT INTO HranaAlergen (HranaId, AlergenId) VALUES
(1, 1),  (1, 2),  -- Capricciosa
(2, 1),  (2, 2),  -- Hawaii: Gluten
(3, 1),  (3, 2),  -- Quattro Stagioni
(14, 1), (14, 2), -- Zimska pizza
(18, 1), (18, 2), -- Vege pizza
(4, 1),			  -- Ra�nji�i + lepinja
(5, 1),			  -- Kobasice + lepinja
(6, 1),			  -- Pile�i file
(7, 7),			  -- Losos
(8, 7),			  -- Tuna salata
(11, 1),		  -- Pivo
(13, 1), (13, 2), -- Palacinke s nutellom
(15, 4),		  -- Vege burger od soje
(16, 1), (16, 2), -- Cheeseburger
(17, 1), (17, 2), -- Bacon burger
(19, 1)           -- Classic burger

   --UPDATE Korisnik 
   --SET RolesId = (SELECT RolesId FROM Roles WHERE RolesName = 'Admin') 
   --WHERE Username = 'admin';