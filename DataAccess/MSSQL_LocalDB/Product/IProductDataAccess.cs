using DataAccess.CustomModels.BasicData;
using DataAccess.Models;
using Model.CommonModels;

namespace DataAccess.MSSQL_LocalDB.BasicData;

public interface IProductDataAccess
{
    public Task<ResponseModel<IEnumerable<RespProductDataAccessModel>>> QueryProductAsync(ReqProductDataAccessModel reqModel);
    public Task<ResponseModel<RespProductDataAccessModel>> QueryProductByIdAsync(string ProductID);
    public Task<ResponseModel<IEnumerable<RespProductDataAccessModel>>> QueryProductForValidatorAsync(ReqProductDataAccessModel reqModel);
    public Task<ResponseModel<bool>> CreateProductAsync(Products product);
    public Task<ResponseModel<bool>> UpdateProductAsync(ReqUpdateProductDataAccessModel reqModel);
    public Task<ResponseModel<bool>> DeleteProductAsync(string ProductID);
}

