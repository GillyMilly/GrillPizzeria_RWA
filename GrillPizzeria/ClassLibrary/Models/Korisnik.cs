using System;
using System.Collections.Generic;

namespace ClassLibrary.Models;

public partial class Korisnik
{
    public int Idkorisnik { get; set; }

    public string Ime { get; set; } = null!;

    public string Prezime { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PwdHash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? Mobitel { get; set; }

    public int RolesId { get; set; }

    public virtual ICollection<Narudzba> Narudzbas { get; set; } = new List<Narudzba>();

    public virtual Role Roles { get; set; } = null!;
}
