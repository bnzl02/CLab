using System;
using System.Collections.Generic;

namespace ChemiseLab.Models;

public partial class Couleur
{
    public int IdCouleur { get; set; }

    public string LibelleCouleur { get; set; } = null!;

    public List<string>? HexaCouleur { get; set; }

    public virtual ICollection<LigneOrder> LigneOrders { get; set; } = new List<LigneOrder>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
