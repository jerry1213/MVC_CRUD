using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Model.ValidationAttributes;

namespace YungChing_MVC.Models.BasicData
{
    [ValidateNever]
    public class ReqUpdateProductViewModel
    {
        [Display(Name = "ProductID")]
        [CustomRequired]
        public string ProductID { get; set; } = null!;

        /// <summary>
        /// ProductName
        /// </summary>
        [Display(Name = "ProductName")]
        [CustomRequired]
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// QuantityPerUnit
        /// </summary>
        [Display(Name = "QuantityPerUnit")]
        [CustomRequired]
        public string QuantityPerUnit { get; set; } = null!;

        /// <summary>
        /// UnitPrice
        /// </summary>
        [Display(Name = "UnitPrice")]
        [CustomRequired]
        public decimal UnitPrice { get; set; }
    }
}