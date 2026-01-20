// C# kod za generiranje hash-a i salt-a za admin lozinku
// Pokrenite ovaj kod u C# konzoli ili dodajte u Program.cs privremeno

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

// Zamijenite "admin123" s vašom željenom lozinkom
string password = "admin123";

// Generiraj salt
byte[] saltBytes = RandomNumberGenerator.GetBytes(128 / 8);
string salt = Convert.ToBase64String(saltBytes);

// Generiraj hash
byte[] hashBytes = KeyDerivation.Pbkdf2(
    password: password,
    salt: saltBytes,
    prf: KeyDerivationPrf.HMACSHA256,
    iterationCount: 100000,
    numBytesRequested: 256 / 8);
string hash = Convert.ToBase64String(hashBytes);

Console.WriteLine("=== Admin korisnik podaci ===");
Console.WriteLine($"Password: {password}");
Console.WriteLine($"Salt: {salt}");
Console.WriteLine($"Hash: {hash}");
Console.WriteLine();
Console.WriteLine("=== SQL komanda ===");
Console.WriteLine($"INSERT INTO Korisnik (Username, Email, Ime, Prezime, PwdHash, Salt, RolesId, Mobitel)");
Console.WriteLine($"VALUES ('admin', 'admin@grillpizzeria.com', 'Admin', 'User', '{hash}', '{salt}', 1, NULL);");
