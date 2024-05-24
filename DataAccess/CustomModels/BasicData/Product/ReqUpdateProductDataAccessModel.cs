using DataAccess.Models;

namespace DataAccess.CustomModels.BasicData
{
    public class ReqUpdateProductDataAccessModel
    {
        public int ProductID { get; set; }

        public string? ProductName { get; set; }

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        public string? QuantityPerUnit { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public virtual Categories? Category { get; set; }

        public virtual Suppliers? Supplier { get; set; }
    }
}