-- BRZI NAČIN: Kreiranje Admin korisnika
-- PRVO: Registrirajte se kroz aplikaciju, pa onda izvršite ovu komandu

-- Provjerite vaš Username nakon registracije i zamijenite 'admin' s vašim username-om
-- ILI koristite Email umjesto Username-a

-- Ažurirajte korisnika na Admin ulogu:
UPDATE Korisnik 
SET RolesId = (SELECT RolesId FROM Roles WHERE RolesName = 'Admin') 
WHERE Username = 'admin';  -- ZAMIJENITE 'admin' s vašim username-om

-- Provjera:
SELECT k.Username, k.Email, r.RolesName 
FROM Korisnik k 
INNER JOIN Roles r ON k.RolesId = r.RolesId 
WHERE r.RolesName = 'Admin';
