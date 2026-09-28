using System;
using System.Collections.Generic;

namespace ChemiseLab.Models;

public partial class Produit
{
    public int IdProduit { get; set; }

    public string Libelle { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Prix { get; set; }

    public string Reference { get; set; } = null!;

    public int IdSousCategorie { get; set; }

    public virtual SousCategorie IdSousCategorieNavigation { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<LigneOrder> LigneOrders { get; set; } = new List<LigneOrder>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
