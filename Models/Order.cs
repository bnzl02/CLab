using System;
using System.Collections.Generic;

namespace ChemiseLab.Models;

public partial class Order
{
    public int IdOrder { get; set; }

    public DateTime DateOrder { get; set; }

    public string StatutOrder { get; set; } = null!;

    public decimal TotalOrder { get; set; }

    public string? ReferenceLivraison { get; set; }

    public string NomClient { get; set; } = null!;

    public string PrenomClient { get; set; } = null!;

    public string AdresseClient { get; set; } = null!;

    public string? TelephoneClient { get; set; }

    public string? PaysClient { get; set; }

    public string? VilleClient { get; set; }

    public DateTime? DateConfirmation { get; set; }

    public virtual ICollection<LigneOrder> LigneOrders { get; set; } = new List<LigneOrder>();
}
