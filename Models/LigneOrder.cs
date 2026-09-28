using System;
using System.Collections.Generic;

namespace ChemiseLab.Models;

public partial class LigneOrder
{
    public int IdLigneOrder { get; set; }

    public int Quantite { get; set; }

    public decimal PrixUnitaire { get; set; }

    public int IdOrder { get; set; }

    public int IdProduit { get; set; }

    public int IdTaille { get; set; }

    public int IdCouleur { get; set; }

    public virtual Couleur IdCouleurNavigation { get; set; } = null!;

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual Produit IdProduitNavigation { get; set; } = null!;

    public virtual Taille IdTailleNavigation { get; set; } = null!;
}
