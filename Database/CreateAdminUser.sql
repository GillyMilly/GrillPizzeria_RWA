-- SQL skripta za kreiranje Admin korisnika
-- Koristite ovu skriptu da kreirate admin korisnika u bazi podataka

-- Provjerite da li već postoji admin korisnik
IF NOT EXISTS (SELECT 1 FROM Korisnik WHERE Username = 'admin')
BEGIN
    -- Kreirajte admin korisnika
    -- Lozinka: admin123 (možete promijeniti)
    -- Salt i Hash se generiraju automatski kroz aplikaciju, ali možete koristiti ovu skriptu
    
    DECLARE @AdminRoleId INT;
    SELECT @AdminRoleId = RolesId FROM Roles WHERE RolesName = 'Admin';
    
    IF @AdminRoleId IS NOT NULL
    BEGIN
        -- OVAJ DIO TREBA BITI IZVRŠEN KROZ APLIKACIJU ILI KORISTITI PasswordHashProvider
        -- Za sada, možete registrirati korisnika kroz aplikaciju, pa onda ažurirati RolesId
        
        -- ILI možete koristiti ovu privremenu lozinku (ne preporuča se za produkciju):
        -- Lozinka: admin123
        -- Hash i Salt će biti generirani kroz aplikaciju
        
        PRINT 'Admin korisnik će biti kreiran kroz aplikaciju.';
        PRINT 'Koraci:';
        PRINT '1. Registrirajte se kroz aplikaciju s bilo kojim korisničkim imenom';
        PRINT '2. Zatim izvršite ovu SQL komandu da promijenite ulogu u Admin:';
        PRINT '';
        PRINT 'UPDATE Korisnik SET RolesId = (SELECT RolesId FROM Roles WHERE RolesName = ''Admin'') WHERE Username = ''VAŠ_USERNAME'';';
    END
    ELSE
    BEGIN
        PRINT 'Uloga Admin nije pronađena u bazi!';
    END
END
ELSE
BEGIN
    PRINT 'Admin korisnik već postoji!';
END

-- ALTERNATIVNO: Ako želite direktno kreirati admin korisnika s poznatim hash-om
-- (Ovo zahtijeva da znate hash i salt koji se generiraju kroz PasswordHashProvider)
-- Možete koristiti C# kod da generirate hash i salt, pa ih umetnuti ovdje
