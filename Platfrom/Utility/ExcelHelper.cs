using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Model.CommonModels;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Platform.Http;
using Platform.LogHelper;
using Platform.Utility.CommonMapping;

namespace Platform.Utility
{
    public class ExcelHelper
    {
        private readonly LogService<ExcelHelper> _logService;

        public ExcelHelper(LogService<ExcelHelper> logService)
        {
            _logService = logService;
        }
        public async Task<MemoryStream?> ExportExcelStream<T>(IEnumerable<T> dataList)
        {
            MemoryStream? stream = null;
            var headers = new List<string>();
            var details = new List<List<string>>();
            try
            {
                var idxHeader = 0;
                var types = typeof(T);
                var fields = types.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    var fieldName = field.Name;
                    var attr = field.GetCustomAttribute<DescriptionAttribute>();
                    if (attr == null)
                        continue;

                    idxHeader++;
                    var headerName = attr.Description;
                    if (!string.IsNullOrEmpty(headerName))
                    {
                        fieldName = headerName;
                    }

                    int idxDetail = 0;
                    foreach (T data in dataList)
                    {
                        List<string> detail;
                        if (idxHeader == 1)
                        {
                            detail = new List<string>();
                            details.Add(detail);
                        }
                        else
                        {
                            detail = details[idxDetail];
                        }

                        string fieldValue = "";
                        if (field.PropertyType == typeof(DateTime))
                        {
                            if (field.GetValue(data) != null)
                            {
                                DateTime dateTimeValue = (DateTime)field.GetValue(data);
                                fieldValue = dateTimeValue.ToString("yyyy-MM-dd");
                            }
                        }
                        else
                            fieldValue = field.GetValue(data)?.ToString() ?? "";
                        detail.Add(fieldValue);

                        idxDetail++;
                    }
                    headers.Add(fieldName);
                }

                var workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Sheet1");
                if (headers.Count > 0)
                {
                    var row = sheet.CreateRow(0);
                    for (int i = 0; i < headers.Count; i++)
                    {
                        row.CreateCell(i, CellType.String).SetCellValue(headers[i]);
                    }
                }
                for (int i = 0; i < details.Count; i++)
                {
                    var detail = details[i];
                    var row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < detail.Count; j++)
                    {
                        row.CreateCell(j, CellType.String).SetCellValue(detail[j]);
                    }
                }

                stream = new MemoryStream();
                workbook.Write(stream);
                stream.Flush();
            }
            catch (Exception ex)
            {
                _logService.AddLog(new LogModel()
                {
                    LogLevel = LogLevel.Critical,
                    Exception = ex,
                    Message = CommonMsgMapping.Error,
                    ReqModel = dataList
                });
            }
            return stream;
        }
    }
}
