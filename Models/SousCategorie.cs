using System;
using System.Collections.Generic;

namespace ChemiseLab.Models;

public partial class SousCategorie
{
    public int IdSousCategorie { get; set; }

    public string Libelle { get; set; } = null!;

    public int IdCategorie { get; set; }

    public virtual Categorie IdCategorieNavigation { get; set; } = null!;

    public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();
}
