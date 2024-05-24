using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model.CommonModels;
using Model.Enums;
using Model.OptionModels;
using Platform.LogHelper;
using Platform.Model;
using Platform.Utility.CommonMapping;
using System.Text.Json;

namespace Platform.Utility
{
    public class MediaHelper
    {
        private readonly LogService<MediaHelper> _logService;
        private readonly MediaOption _mediaOption;
        private readonly IdGenerator _idGenerator;

        public MediaHelper(LogService<MediaHelper> logService,IOptions<MediaOption> mediaOption, IdGenerator idGenerator)
        {
            _logService = logService;
            _mediaOption = mediaOption.Value;
            _idGenerator = idGenerator;
        }

        public ResponseModel<bool> VaildateMedia(IFormFileCollection files)
        {
            var result = new ResponseModel<bool>();

            try
            {
                if (files.Sum(s => s.Length) >= _mediaOption.MaxSize)
                {
                    result.IsOk = false;
                    result.Data = false;
                    result.Message = "總體檔案過大，無法上傳";
                    return result;
                }
                foreach (var file in files)
                {
                    var fileFormat = file.FileName.Split('.')[1].ToLower();
                    if (!_mediaOption.PhotoType.Contains(fileFormat) && 
                        !_mediaOption.VideoType.Contains(fileFormat))
                    {
                        result.IsOk = false;
                        result.Data = false;
                        result.Message = $"{file.FileName}格式不對，無法上傳";
                        return result;
                    }
                    else if (_mediaOption.PhotoType.Contains(fileFormat) &&
                             file.Length > _mediaOption.PhotoMaxSize)
                    {
                        result.IsOk = false;
                        result.Data = false;
                        result.Message = $"{file.FileName}檔案過大，無法上傳";
                        return result;
                    }
                    else if (_mediaOption.VideoType.Contains(fileFormat) &&
                             file.Length > _mediaOption.VideoMaxSize)
                    {
                        result.IsOk = false;
                        result.Data = false;
                        result.Message = $"{file.FileName}檔案過大，無法上傳";
                        return result;
                    }
                }
                result.Data = true;
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
                    ReqModel = files,
                    RspModel = result
                });
            }
            return result;
        }
        public ResponseModel<string> UploadMedia(IFormFileCollection files)
        {
            var result = new ResponseModel<string>();
            var path = _mediaOption.MobilePath + DateTime.Today.ToString("yyyy-MM-dd");
            var photoFullPath = _mediaOption.PhotoRootPath + path;
            var videoFullPath = _mediaOption.VideoRootPath + path;
            var media = new List<MediaModel>();
            try
            {
                if (!Directory.Exists(photoFullPath))
                {
                    Directory.CreateDirectory(photoFullPath);
                }
                if (!Directory.Exists(videoFullPath))
                {
                    Directory.CreateDirectory(videoFullPath);
                }

                var photos = files.Where(w =>
                                _mediaOption.PhotoType!.Contains(w.FileName.Trim().Split('.')[1].ToLower()));
                if (photos.Any())
                {
                    foreach (var photo in photos)
                    {
                        var oid = _idGenerator.GetOid().Substring(16);
                        var newName = $"{oid}.{photo.FileName.Split('.')[1]}";
                        var photoModel = new MediaModel()
                        {
                            name = photo.FileName,
                            type = (int)FileType.Photo,
                            file_id = oid,
                            size = FormatSize.ByteToString(photo.Length),
                            url = $"{path}/{newName}"
                        };
                        using (var stream = new FileStream(Path.Combine(photoFullPath, newName), FileMode.Create))
                        {
                            photo.CopyTo(stream);
                        }
                        media.Add(photoModel);
                    }
                }
                var videos = files.Where(w =>
                               _mediaOption.VideoType!.Contains(w.FileName.Trim().Split('.')[1].ToLower()));
                if (videos.Any())
                {
                    foreach (var video in videos)
                    {
                        var oid = _idGenerator.GetOid().Substring(16);
                        var newName = $"{oid}.{video.FileName.Split('.')[1]}";
                        var videoModel = new MediaModel()
                        {
                            name = video.FileName,
                            type = (int)FileType.Video,
                            file_id = oid,
                            size = FormatSize.ByteToString(video.Length),
                            url = $"{path}/{newName}"
                        };
                        using (var stream = new FileStream(Path.Combine(videoFullPath, newName), FileMode.Create))
                        {
                            video.CopyTo(stream);
                        }
                        media.Add(videoModel);
                    }
                }

                result.Data = JsonSerializer.Serialize(media);
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
                    ReqModel = files,
                    RspModel = result
                });
            }
            return result;
        }
        public ResponseModel<bool> CompareMedia(string? oldJsonString,string? newJsonString)
        {
            var result = new ResponseModel<bool>();
           
            try
            {
                var oldMedia = new List<MediaModel>();
                var newMedia = new List<MediaModel>();
                if (oldJsonString != null)
                {
                    oldMedia = JsonSerializer.Deserialize<List<MediaModel>>(oldJsonString);
                }
                if (newJsonString != null)
                {
                    newMedia = JsonSerializer.Deserialize<List<MediaModel>>(newJsonString);
                }
                if (oldMedia != null && oldMedia.Count > 0)
                {
                    var diffMedia = oldMedia.Except(newMedia!).ToList();
                    if (diffMedia.Count > 0)
                    {
                        result = DeleteMedia(diffMedia);
                        return result;
                    }
                }
                result.Data = true;
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
                    ReqModel = newJsonString,
                    RspModel = result
                });
            }
            return result;
        }
        public ResponseModel<bool> DeleteMedia(List<MediaModel> media)
        {
            var result = new ResponseModel<bool>();

            try
            {
                foreach (var item in media!)
                {
                    var path = string.Empty;
                    if (item.type == 0 && _mediaOption.VideoType.Contains(item.name.Split(".")[1]))
                    {
                        item.type = 1;
                    }
                    if (item.type == (int)FileType.Photo)
                    {
                        path = _mediaOption.PhotoRootPath + item.url;
                    }
                    else
                    {
                        path = _mediaOption.VideoRootPath + item.url;
                    }
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    result.Data = true;
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
                    ReqModel = media,
                    RspModel = result
                });
            }
            return result;
        }
        public string UpdateString(string? jsonString, string? newMediaString)
        {
            var media = new List<MediaModel>();
            if (jsonString != null)
            {
                media.AddRange(JsonSerializer.Deserialize<List<MediaModel>>(jsonString)!);
            }
            if (newMediaString != null)
            {
                media.AddRange(JsonSerializer.Deserialize<List<MediaModel>>(newMediaString)!);
            }
            return JsonSerializer.Serialize(media);
        }
    }
}
