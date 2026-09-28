using System;
using System.Collections.Generic;

namespace ChemiseLab.Models;

public partial class Categorie
{
    public int IdCategorie { get; set; }

    public string Libelle { get; set; } = null!;

    public virtual ICollection<SousCategorie> SousCategories { get; set; } = new List<SousCategorie>();
}
