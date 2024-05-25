using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Model.ValidationAttributes;

namespace YungChing_MVC.Models.BasicData
{
    [ValidateNever]
    public class ReqCreateProductViewModel
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [Display(Name = "名稱")]
        [CustomRequired]
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// 編碼
        /// </summary>
        [Display(Name = "編碼")]
        [CustomRequired]
        public string QuantityPerUnit { get; set; } = null!;

        /// <summary>
        /// 編碼
        /// </summary>
        [Display(Name = "編碼")]
        [CustomRequired]
        public decimal UnitPrice { get; set; }
    }
}