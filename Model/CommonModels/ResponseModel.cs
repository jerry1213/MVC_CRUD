namespace Model.CommonModels
{
    /// <summary>
    /// 通用的回傳Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseModel<T>
    {
        /// <summary>
        /// 程式運作正常
        /// </summary>
        public bool IsOk { get; set; } = true;

        public string ErrorCode { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 結果
        /// </summary>
        public T? Data { get; set; }
    }
}
