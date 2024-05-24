using DataAccess.CustomModels.BasicData;
using DataAccess.Models;
using DataAccess.MSSQL_LocalDB.BasicData;
using Microsoft.Extensions.Logging;
using Model.CommonModels;
using Omu.ValueInjecter;
using Platform.LogHelper;
using Platform.Utility;
using Platform.Utility.CommonMapping;
using Service.Models.BasicData;

namespace Service.BasicData
{
    public class ProductService : IProductService
    {
        private readonly LogService<ProductService> _logService;
        private readonly IProductDataAccess _productDataAccess;
        private readonly ProductServiceValidator _productServiceValidator;

        public ProductService(LogService<ProductService> logService, IProductDataAccess productDataAccess, ProductServiceValidator productServiceValidator)
        {
            _logService = logService;
            _productDataAccess = productDataAccess;
            _productServiceValidator = productServiceValidator;
        }
        public async Task<ResponseModel<IEnumerable<RespProductServiceModel>>> QueryProductAsync(ReqProductServiceModel reqModel)
        {
            var result = new ResponseModel<IEnumerable<RespProductServiceModel>>();

            try
            {
                var reqDalModel = Mapper.Map<ReqProductDataAccessModel>(reqModel);
                var productDalResult = await _productDataAccess.QueryProductAsync(reqDalModel);

                if (!productDalResult.IsOk)
                {
                    result.IsOk = false;
                    result.Message = productDalResult.Message;
                    return result;
                }

                if (productDalResult.Data != null && productDalResult.Data.Any())
                {
                    result.Data
                     = productDalResult.Data
                        .Select(s => Mapper.Map<RespProductServiceModel>(s)).ToList();
                }
                else
                {
                    result.Message = CommonMsgMapping.NoData;
                }

            }
            catch (Exception ex)
            {
                result.Message = CommonMsgMapping.Error;
                result.IsOk = false;
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
        public async Task<ResponseModel<RespProductServiceModel>> QueryProductByIdAsync(int productId)
        {
            var result = new ResponseModel<RespProductServiceModel>();

            try
            {
                var productDalResult = await _productDataAccess.QueryProductByIdAsync(productId);

                if (!productDalResult.IsOk)
                {
                    result.IsOk = false;
                    result.Message = productDalResult.Message;
                    return result;
                }

                if (productDalResult.Data != null)
                {
                    result.Data = Mapper.Map<RespProductServiceModel>(productDalResult.Data);
                }
                else
                {
                    result.Message = CommonMsgMapping.NoData;
                }

            }
            catch (Exception ex)
            {
                result.Message = CommonMsgMapping.Error;
                result.IsOk = false;
                _logService.AddLog(new LogModel()
                {
                    LogLevel = LogLevel.Critical,
                    Exception = ex,
                    Message = result.Message,
                    ReqModel = productId,
                    RspModel = result
                });
            }
            return result;
        }
        public async Task<ResponseModel<bool>> CreateProductAsync(ReqCreateProductServiceModel reqModel)
        {
            var result = new ResponseModel<bool>();

            try
            {
                var valModel = Mapper.Map<ReqProductServiceModel>(reqModel);
                var IsExist = await _productServiceValidator.ValidateIsExistsAsync(valModel);
                if (!IsExist.IsOk || !IsExist.Data)
                {
                    result.IsOk = IsExist.IsOk;
                    result.Data = IsExist.Data;
                    result.Message = IsExist.Message;
                    return result;
                }
                var dalModel = new Products()
                {
                    ProductID = reqModel.ProductID,
                    ProductName = reqModel.ProductName,
                    QuantityPerUnit = reqModel.QuantityPerUnit,
                    UnitPrice = reqModel.UnitPrice
                };

                var productDalResult = await _productDataAccess.CreateProductAsync(dalModel);

                if (!productDalResult.IsOk)
                {
                    result.IsOk = false;
                    result.Message = productDalResult.Message;
                    return result;
                }

                if (productDalResult.Data)
                {
                    result.Data = true;
                }
                else
                {
                    result.Data = false;
                    result.Message = CommonMsgMapping.InsertFail;
                }

            }
            catch (Exception ex)
            {
                result.Message = CommonMsgMapping.Error;
                result.IsOk = false;
                result.Data = false;
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
        public async Task<ResponseModel<bool>> UpdateProductAsync(ReqUpdateProductServiceModel reqModel)
        {
            var result = new ResponseModel<bool>();

            try
            {
                var valModel = Mapper.Map<ReqProductServiceModel>(reqModel);
                var IsExist = await _productServiceValidator.ValidateIsExistsAsync(valModel);
                if (!IsExist.IsOk || !IsExist.Data)
                {
                    result.IsOk = IsExist.IsOk;
                    result.Data = IsExist.Data;
                    result.Message = IsExist.Message;
                    return result;
                }
                var productDalResult = await _productDataAccess.UpdateProductAsync(Mapper.Map<ReqUpdateProductDataAccessModel>(reqModel));
                if (!productDalResult.IsOk)
                {
                    result.IsOk = false;
                    result.Message = productDalResult.Message;
                    return result;
                }

                if (productDalResult.Data)
                {
                    result.Data = true;
                }
                else
                {
                    result.Data = false;
                    result.Message = CommonMsgMapping.UpdateFail;
                }

            }
            catch (Exception ex)
            {
                result.Message = CommonMsgMapping.Error;
                result.IsOk = false;
                result.Data = false;
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
        public async Task<ResponseModel<bool>> DeleteProductAsync(int productId)
        {
            var result = new ResponseModel<bool>();

            try
            {
                var productDalResult = await _productDataAccess.DeleteProductAsync(productId);

                if (!productDalResult.IsOk)
                {
                    result.IsOk = false;
                    result.Message = productDalResult.Message;
                    return result;
                }

                if (productDalResult.Data)
                {
                    result.Data = true;
                }
                else
                {
                    result.Data = false;
                    result.Message = CommonMsgMapping.DeleteFail;
                }

            }
            catch (Exception ex)
            {
                result.Message = CommonMsgMapping.Error;
                result.IsOk = false;
                result.Data = false;
                _logService.AddLog(new LogModel()
                {
                    LogLevel = LogLevel.Critical,
                    Exception = ex,
                    Message = result.Message,
                    ReqModel = productId,
                    RspModel = result
                });
            }
            return result;
        }
    }
}
