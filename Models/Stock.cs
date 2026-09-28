using System;
using System.Collections.Generic;

namespace ChemiseLab.Models;

public partial class Stock
{
    public int IdProduit { get; set; }

    public int IdTaille { get; set; }

    public int IdCouleur { get; set; }

    public int Stock1 { get; set; }

    public virtual Couleur IdCouleurNavigation { get; set; } = null!;

    public virtual Produit IdProduitNavigation { get; set; } = null!;

    public virtual Taille IdTailleNavigation { get; set; } = null!;
}
