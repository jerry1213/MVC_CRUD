using DataAccess.CustomModels.BasicData;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.CommonModels;
using Platform.LogHelper;
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
    public async Task<ResponseModel<IEnumerable<RespProductDataAccessModel>>> QueryBuAsync(ReqProductDataAccessModel reqModel)
    {
        var result = new ResponseModel<IEnumerable<RespProductDataAccessModel>>();

        try
        {
            var query =
                from b in _context.plt_base_bu
                join u in _context.plt_org_user on b.plt_creator equals u.plt_oid into gj
                from subu in gj.DefaultIfEmpty()
                orderby b.plt_code
                select new RespProductDataAccessModel
                {
                    Oid = b.plt_oid,
                    Name = b.plt_name,
                    Code = b.plt_code,
                    Creator = subu.plt_displayname
                };

            if (!string.IsNullOrWhiteSpace(reqModel.Code))
            {
                query = query.Where(w => w.Code.Contains(reqModel.Code));
            }

            if (!string.IsNullOrWhiteSpace(reqModel.Name))
            {
                query = query.Where(w => w.Name.Contains(reqModel.Name));
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
    public async Task<ResponseModel<RespProductDataAccessModel>> QueryBuByIdAsync(string buId)
    {
        var result = new ResponseModel<RespProductDataAccessModel>();
        try
        {
            var query =
                from b in _context.plt_base_bu
                join u in _context.plt_org_user on b.plt_creator equals u.plt_oid into gj
                from subu in gj.DefaultIfEmpty()
                where b.plt_oid == buId
                select new RespProductDataAccessModel
                {
                    Oid = b.plt_oid,
                    Name = b.plt_name,
                    Code = b.plt_code,
                    Creator = subu.plt_displayname
                };

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
                ReqModel = buId,
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
            var query =
                from b in _context.plt_base_bu
                join u in _context.plt_org_user on b.plt_creator equals u.plt_oid into gj
                from subu in gj.DefaultIfEmpty()
                select new RespProductDataAccessModel
                {
                    Oid = b.plt_oid,
                    Name = b.plt_name,
                    Code = b.plt_code,
                    Creator = subu.plt_displayname
                };

            query = query.Where(w => w.Oid != reqModel.Oid && (w.Code == reqModel.Code || w.Name == reqModel.Name));

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
    public async Task<ResponseModel<bool>> CreateBuAsync(plt_base_bu bu)
    {
        var result = new ResponseModel<bool>();
        try
        {
            _context.plt_base_bu.Add(bu);
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
                ReqModel = bu,
                RspModel = result
            });
        }
        return result;
    }
    public async Task<ResponseModel<bool>> UpdateBuAsync(ReqUpdateProductDataAccessModel reqModel)
    {
        var result = new ResponseModel<bool>();
        try
        {
            var buToUpdate = await _context.plt_base_bu.FindAsync(reqModel.Oid);

            if (buToUpdate == null)
            {
                result.Data = false;
                return result;
            }
            buToUpdate.plt_code = reqModel.Code;
            buToUpdate.plt_name = reqModel.Name;
            buToUpdate.plt_lastmodifier = reqModel.LastModifier;
            buToUpdate.plt_lastmodifytime = DateTime.Now;

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
    public async Task<ResponseModel<bool>> DeleteBuAsync(string buId)
    {
        var result = new ResponseModel<bool>();
        try
        {
            var bu = await _context.plt_base_bu.FindAsync(buId);
            if (bu == null)
            {
                result.Data = false;
                return result;
            }
            _context.plt_base_bu.Remove(bu);
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
                ReqModel = buId,
                RspModel = result
            });
        }
        return result;
    }
}

