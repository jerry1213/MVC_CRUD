using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model.CommonModels;
using Platform.LogHelper;
using Platform.Utility.CommonMapping;
using System.Security.Cryptography;
using System.Text;

namespace Platform.Utility
{
    public class EncryptionHelper
    {
        private readonly LogService<EncryptionHelper> _logService;
        private readonly IConfiguration _configuration;

        public EncryptionHelper(LogService<EncryptionHelper> logService, IConfiguration configuration)
        {
            _logService = logService;
            _configuration = configuration;
        }

        public string ToSHA256(string srcString)
        {
            var hasedString = string.Empty;
            try
            {
                var sha256 = SHA256.Create();
                var bytes = AddSalt(srcString);
                var hased = sha256.ComputeHash(bytes);
                hasedString = BitConverter.ToString(hased).Replace("-", string.Empty).ToUpper();
            }
            catch(Exception ex)
            {
                _logService.AddLog(new LogModel()
                {
                    LogLevel = LogLevel.Critical,
                    Exception = ex,
                    Message = CommonMsgMapping.Error,
                    ReqModel = srcString
                });
            }
            return hasedString;
        }
        public string ToMD5(string srcString)
        {
            var hasedString = string.Empty;
            try
            {
                var md5 = MD5.Create();
                var bytes = AddSalt(srcString);
                var hased = md5.ComputeHash(bytes);
                hasedString = BitConverter.ToString(hased).Replace("-", string.Empty).ToUpper();
            }
            catch (Exception ex)
            {
                _logService.AddLog(new LogModel()
                {
                    LogLevel = LogLevel.Critical,
                    Exception = ex,
                    Message = CommonMsgMapping.Error,
                    ReqModel = srcString
                });
            }
            return hasedString;
        }
        public byte[] AddSalt(string srcString)
        {
            var bytes = new byte[srcString.Length];
            try
            {
                var salt = _configuration["EncryptSalt"];
                var saltbytes = StringToByte(salt!);
                bytes = StringToByte(srcString);
                Array.Resize(ref bytes, bytes.Length + saltbytes.Length);
                Array.Copy(saltbytes, 0, bytes, bytes.Length - saltbytes.Length, saltbytes.Length);
            }
            catch (Exception ex)
            {
                _logService.AddLog(new LogModel()
                {
                    LogLevel = LogLevel.Critical,
                    Exception = ex,
                    Message = CommonMsgMapping.Error,
                    ReqModel = srcString
                });
            }
            return bytes;
        }
        public byte[] StringToByte(string srcString)
        {
            if (string.IsNullOrEmpty(srcString)) return null!;
            return Encoding.UTF8.GetBytes(srcString);
        }
    }
}
