using Microsoft.Extensions.Logging;

namespace Model.CommonModels
{
    public class LogModel
    {
        /// <summary>
        /// Log級別
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Log訊息
        /// </summary>
        public string Message { get; set; } = null!;

        /// <summary>
        /// 例外
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// 接收到的參數
        /// </summary>
        public object? ReqModel { get; set; }

        /// <summary>
        /// 回應的結果
        /// </summary>
        public object? RspModel { get; set; }
    }
}
