using DataAccess.CustomModels.BasicData;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.CommonModels;
using Platform.LogHelper;
using Platform.Utility.CommonMapping;
using System;

namespace DataAccess.MSSQL_LocalDB.BasicData;

public class ProductDataAccess : IProductDataAccess
{
    private readonly LogService<ProductDataAccess> _logService;
    private readonly NorthwindContext _context;

    public ProductDataAccess(LogService<ProductDataAccess> logService, NorthwindContext context)
    {
        _logService = logService;
        _context = context;
    }
    public async Task<ResponseModel<IEnumerable<RespProductDataAccessModel>>> QueryProductAsync(ReqProductDataAccessModel reqModel)
    {
        var result = new ResponseModel<IEnumerable<RespProductDataAccessModel>>();

        try
        {
            var query = _context.Products.Select(p => new RespProductDataAccessModel
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                QuantityPerUnit = p.QuantityPerUnit,
                UnitPrice = p.UnitPrice

            });   

            if (!string.IsNullOrWhiteSpace(reqModel.ProductName))
            {
                query = query.Where(w => w.ProductName.Contains(reqModel.ProductName));
            }

            if (!string.IsNullOrWhiteSpace(reqModel.QuantityPerUnit))
            {
                query = query.Where(w => w.QuantityPerUnit.Contains(reqModel.QuantityPerUnit));
            }

            var dbResult = await query.ToListAsync();

            if (dbResult.Any())
            {
                result.Data = dbResult;
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
    public async Task<ResponseModel<RespProductDataAccessModel>> QueryProductByIdAsync(int productId)
    {
        var result = new ResponseModel<RespProductDataAccessModel>();
        try
        {
            var query = _context.Products.Select(p => new RespProductDataAccessModel
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                QuantityPerUnit = p.QuantityPerUnit,
                UnitPrice = p.UnitPrice

            }).Where(p => p.ProductID == productId);

            var dbResult = await query.FirstOrDefaultAsync();
            if (dbResult != null)
            {
                result.Data = dbResult;
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
    public async Task<ResponseModel<IEnumerable<RespProductDataAccessModel>>> QueryProductForValidatorAsync(
        ReqProductDataAccessModel reqModel)
    {
        var result = new ResponseModel<IEnumerable<RespProductDataAccessModel>>();

        try
        {
            var query = _context.Products.Select(p => new RespProductDataAccessModel
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                QuantityPerUnit = p.QuantityPerUnit,
                UnitPrice = p.UnitPrice

            });

            query = query.Where(w => w.ProductID != reqModel.ProductID && (w.ProductName == reqModel.ProductName));

            var dbResult = await query.ToListAsync();

            if (dbResult.Any())
            {
                result.Data = dbResult;
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
    public async Task<ResponseModel<bool>> CreateProductAsync(Products product)
    {
        var result = new ResponseModel<bool>();
        try
        {
            _context.Products.Add(product);
            var affected = await _context.SaveChangesAsync();
            result.Data = affected == 1;
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
                ReqModel = product,
                RspModel = result
            });
        }
        return result;
    }
    public async Task<ResponseModel<bool>> UpdateProductAsync(ReqUpdateProductDataAccessModel reqModel)
    {
        var result = new ResponseModel<bool>();
        try
        {
            var buToUpdate = await _context.Products.FindAsync(reqModel.ProductID);

            if (buToUpdate == null)
            {
                result.Data = false;
                return result;
            }
            buToUpdate.ProductName = reqModel.ProductName;
            buToUpdate.QuantityPerUnit = reqModel.QuantityPerUnit;
            buToUpdate.UnitPrice = reqModel.UnitPrice;

            var affected = await _context.SaveChangesAsync();
            result.Data = affected == 1;
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
            var bu = await _context.Products.FindAsync(productId);
            if (bu == null)
            {
                result.Data = false;
                return result;
            }
            _context.Products.Remove(bu);
            var affected = await _context.SaveChangesAsync();
            result.Data = affected == 1;
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

