using System;
using System.Collections.Generic;

namespace ChemiseLab.Models;

public partial class Taille
{
    public int IdTaille { get; set; }

    public string? LibelleSport { get; set; }

    public string? LibelleFr { get; set; }

    public string? LibelleUsa { get; set; }

    public virtual ICollection<LigneOrder> LigneOrders { get; set; } = new List<LigneOrder>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
