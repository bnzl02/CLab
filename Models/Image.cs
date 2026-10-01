using System;
using System.Collections.Generic;

namespace ChemiseLab.Models;

public partial class Image
{
    public int IdImage { get; set; }

    public string UrlImage { get; set; } = null!;

    public int IdProduit { get; set; }

    public int? IdCouleur { get; set; }

    public virtual Produit IdProduitNavigation { get; set; } = null!;
}
