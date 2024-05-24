using DataAccess.Models;

namespace DataAccess.CustomModels.BasicData;

public class ReqProductDataAccessModel
{
    public int ProductID { get; set; }

    public string? ProductName { get; set; }

    public int? SupplierID { get; set; }

    public int? CategoryID { get; set; }

    public string? QuantityPerUnit { get; set; }

    public decimal? UnitPrice { get; set; }

}

