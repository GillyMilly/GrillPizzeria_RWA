using System;
using System.Collections.Generic;

namespace GPWebAPI.Models;

public partial class Role
{
    public int RolesId { get; set; }

    public string RolesName { get; set; } = null!;

    public virtual ICollection<Korisnik> Korisniks { get; set; } = new List<Korisnik>();
}
