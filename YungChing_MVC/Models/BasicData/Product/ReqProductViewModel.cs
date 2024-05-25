namespace YungChing_MVC.Models.BasicData;

public class ReqProductViewModel
{
    /// <summary>
    /// Product
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// QuantityPerUnit
    /// </summary>
    public string? QuantityPerUnit { get; set; }

    /// <summary>
    /// UnitPrice
    /// </summary>
    public decimal? UnitPrice { get; set; }
}
