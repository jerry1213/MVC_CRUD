using System.ComponentModel;

namespace YungChing_MVC.Models.BasicData
{
    internal class RespProductViewModel
    {
        public string Oid { get; set; } = null!;

        /// <summary>
        /// 名稱
        /// </summary>
        [Description("名稱")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 編碼
        /// </summary>
        [Description("編碼")]
        public string Code { get; set; } = null!;

        /// <summary>
        /// 創建人
        /// </summary>
        [Description("創建人")]
        public string Creator { get; set; } = string.Empty;
        /// <summary>
        /// 頁面用No
        /// </summary>
        public int No { get; set; }
    }
}