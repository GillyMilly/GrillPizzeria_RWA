# Upute za kreiranje Admin korisnika

## Opcija 1: Kroz aplikaciju (Preporučeno)

1. **Registrirajte se kroz WebApp:**
   - Otvorite `http://localhost:5153/Korisnik/Register`
   - Unesite podatke (npr. Username: `admin`, Email: `admin@grillpizzeria.com`, itd.)
   - Lozinka: `admin123` (ili bilo koja druga)
   - Kliknite "Registriraj se"

2. **Promijenite ulogu u Admin kroz SQL:**
   ```sql
   UPDATE Korisnik 
   SET RolesId = (SELECT RolesId FROM Roles WHERE RolesName = 'Admin') 
   WHERE Username = 'admin';
   ```

3. **Prijavite se:**
   - Otvorite `http://localhost:5153/Korisnik/SignIn`
   - Username: `admin` (ili email koji ste unijeli)
   - Password: `admin123` (ili lozinka koju ste unijeli)

## Opcija 2: Direktno kroz SQL (Napredno)

Ako želite direktno kreirati admin korisnika kroz SQL, trebate generirati hash i salt za lozinku.

**Koraci:**
1. Pokrenite C# kod da generirate hash i salt (koristite PasswordHashProvider)
2. Umetnite korisnika s tim hash-om i salt-om

**Primjer C# koda za generiranje hash-a:**
```csharp
using WebApp.Security;

string password = "admin123";
string salt = PasswordHashProvider.GetSalt();
string hash = PasswordHashProvider.GetHash(password, salt);

Console.WriteLine($"Salt: {salt}");
Console.WriteLine($"Hash: {hash}");
```

Zatim u SQL:
```sql
DECLARE @AdminRoleId INT = (SELECT RolesId FROM Roles WHERE RolesName = 'Admin');

INSERT INTO Korisnik (Username, Email, Ime, Prezime, PwdHash, Salt, RolesId, Mobitel)
VALUES ('admin', 'admin@grillpizzeria.com', 'Admin', 'User', 'GENERIRANI_HASH', 'GENERIRANI_SALT', @AdminRoleId, NULL);
```

## Pristup Log stranici

Nakon što se ulogirate kao Admin:
- Otvorite `http://localhost:5153/Log` ili
- Kliknite na "Logovi" u navigaciji (samo vidljivo za Admin korisnike)
