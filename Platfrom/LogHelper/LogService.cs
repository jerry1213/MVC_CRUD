using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.OptionModels;
using Microsoft.Extensions.Options;
using Model.CommonModels;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Platform.LogHelper;

public class LogService<T>
{
    private readonly ILogger _logger;
    private readonly SiteStatusOption _siteStatusOption;

    public LogService(ILogger<T> logger, IOptions<SiteStatusOption> siteStatusOption)
    {
        _logger = logger;
        _siteStatusOption = siteStatusOption.Value;
    }

    public void AddLog(LogModel logModel)
    {
        var logMsg = logModel.Message;
        var args = new List<object>();
        if (logModel.ReqModel != null)
        {
            logMsg += " | ReqModel = {@ReqModel}";
            args.Add(logModel.ReqModel);
        }
        if (logModel.RspModel != null)
        {
            logMsg += " | RspModel = {@RspModel}";
            args.Add(logModel.RspModel);
        }

        if (logModel.Exception?.GetType().Name == nameof(DbUpdateException))
        {
            logMsg += " | Exception = {@Exception}";
            args.Add(nameof(DbUpdateException));
            _logger.Log(logModel.LogLevel, logMsg, args.ToArray());
        }
        else
        {
            if (logModel.Exception != null)
            {
                logMsg += " | Exception = {@Exception}";
                args.Add(logModel.Exception);
            }
            _logger.Log(logModel.LogLevel, logModel.Exception, logMsg, args.ToArray());
        }
    }
}

