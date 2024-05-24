namespace Platform.Utility
{
    /// <summary>
    /// 產ID工具
    /// </summary>
    public class IdGenerator
    {
        /// <summary>
        /// 產oid(GUID 長度32)
        /// </summary>
        /// <returns></returns>
        public string GetOid()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }     
        
        public List<T> SetViewModelNos<T>(List<T> dataList)
        {
            var curVal = 1;
            foreach (var data in dataList)
            {
                var prop = data!.GetType().GetProperty("No");
                prop?.SetValue(data, curVal++);
            }
            return dataList;
        }
    }
}
