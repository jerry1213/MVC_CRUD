using DataAccess.CustomModels.BasicData;
using DataAccess.MSSQL_LocalDB.BasicData;
using Microsoft.Extensions.Logging;
using Model.CommonModels;
using Omu.ValueInjecter;
using Platform.LogHelper;
using Platform.Utility.CommonMapping;
using Service.Models.BasicData;

namespace Service.BasicData
{
    public class ProductServiceValidator
    {
        private readonly LogService<ProductServiceValidator> _logService;
        private readonly IProductDataAccess _productDataAccess;

        public ProductServiceValidator(LogService<ProductServiceValidator> logService, IProductDataAccess buDataAccess)
        {
            _logService = logService;
            _productDataAccess = buDataAccess;
        }

        public async Task<ResponseModel<bool>> ValidateIsExistsAsync(ReqProductServiceModel reqModel)
        {
            var result = new ResponseModel<bool>();
            try
            {
                var reqDalModel = Mapper.Map<ReqProductDataAccessModel>(reqModel);
                var productDalResult = await _productDataAccess.QueryProductForValidatorAsync(reqDalModel);

                if (!productDalResult.IsOk)
                {
                    result.IsOk = false;
                    result.Message = productDalResult.Message;
                    return result;
                }
                if (productDalResult.Data != null)
                {
                    result.Data = false;
                    result.Message = "該Product已存在，不可重複創建";
                    return result;
                }
                else
                {
                    result.Data = true;
                }

                
            }
            catch (Exception ex)
            {
                result.IsOk = false;
                result.Data = false;
                result.Message = CommonMsgMapping.Error;
                _logService.AddLog(new LogModel()
                {
                    LogLevel = LogLevel.Critical,
                    Exception = ex,
                    Message = result.Message,
                    ReqModel = reqModel,
                    RspModel = result
                });
            }
            return result;
        }

    }
}
