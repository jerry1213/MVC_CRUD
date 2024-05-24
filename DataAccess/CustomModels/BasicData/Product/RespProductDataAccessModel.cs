using DataAccess.Models;

namespace DataAccess.CustomModels.BasicData;

public class RespProductDataAccessModel
{
    public int ProductID { get; set; }

    public string ProductName { get; set; } = null!;

    public int? SupplierID { get; set; }

    public int? CategoryID { get; set; }

    public string QuantityPerUnit { get; set; } = null!;

    public decimal? UnitPrice { get; set; }

}

