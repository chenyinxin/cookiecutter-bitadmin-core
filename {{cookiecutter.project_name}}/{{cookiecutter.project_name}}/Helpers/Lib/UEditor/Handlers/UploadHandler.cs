using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace {{cookiecutter.project_name}}.UEditor.Handlers
{
    public class UploadHandler : Handler
    {

        public UploadConfig UploadConfig { get; private set; }
        public UploadResult Result { get; private set; }

        public UploadHandler(HttpContext context, UploadConfig config)
            : base(context)
        {
            this.UploadConfig = config;
            this.Result = new UploadResult() { State = UploadState.Unknown };
        }

        public override void Process()
        {
            byte[] uploadFileBytes = null;
            string uploadFileName = null;

            if (UploadConfig.Base64)
            {
                uploadFileName = UploadConfig.Base64Filename;
                uploadFileBytes = Convert.FromBase64String(Request.Form[UploadConfig.UploadFieldName]);
            }
            else
            {
                var file = Request.Form.Files[UploadConfig.UploadFieldName];
                uploadFileName = file.FileName;

                if (!CheckFileType(uploadFileName))
                {
                    Result.State = UploadState.TypeNotAllow;
                    WriteResult();
                    return;
                }
                if (!CheckFileSize((int)file.Length))
                {
                    Result.State = UploadState.SizeLimitExceed;
                    WriteResult();
                    return;
                }

                uploadFileBytes = new byte[file.Length];
                try
                {
                    file.OpenReadStream().Read(uploadFileBytes, 0, (int)file.Length);
                }
                catch (Exception)
                {
                    Result.State = UploadState.NetworkError;
                    WriteResult();
                }
            }

            Result.OriginFileName = uploadFileName;

            var savePath = PathFormatter.Format(uploadFileName, UploadConfig.PathFormat);
            var localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, savePath);
            try
            {
                if (UploadConfig.IsLocal)
                {

                    if (!Directory.Exists(Path.GetDirectoryName(localPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(localPath));
                    }
                    File.WriteAllBytes(localPath, uploadFileBytes);
                    Result.Url = savePath;
                    Result.State = UploadState.Success;
                }
                else
                {
                    FileHandler fTPHelper = new FileHandler();
                    MemoryStream stream = new MemoryStream(uploadFileBytes);
                    FileHandler.FileServiceResult result = fTPHelper.UploadFile(stream, UploadConfig.FileServerUrl, savePath);
                    Result.Url = result.Data[0];
                    Result.State = UploadState.Success;
                }
            }
            catch (Exception e)
            {
                Result.State = UploadState.FileAccessError;
                Result.ErrorMessage = e.Message;
            }
            finally
            {
                WriteResult();
            }
        }

        private void WriteResult()
        {
            this.WriteJson(new
            {
                state = GetStateMessage(Result.State),
                url = Result.Url,
                title = Result.OriginFileName,
                original = Result.OriginFileName,
                error = Result.ErrorMessage
            });
        }

        private string GetStateMessage(UploadState state)
        {
            switch (state)
            {
                case UploadState.Success:
                    return "SUCCESS";
                case UploadState.FileAccessError:
                    return "文件访问出错，请检查写入权限";
                case UploadState.SizeLimitExceed:
                    return "文件大小超出服务器限制";
                case UploadState.TypeNotAllow:
                    return "不允许的文件格式";
                case UploadState.NetworkError:
                    return "网络错误";
            }
            return "未知错误";
        }

        private bool CheckFileType(string filename)
        {
            var fileExtension = Path.GetExtension(filename).ToLower();
            return UploadConfig.AllowExtensions.Select(x => x.ToLower()).Contains(fileExtension);
        }

        private bool CheckFileSize(int size)
        {
            return size < UploadConfig.SizeLimit;
        }
    }

    public class UploadConfig
    {
        /// <summary>
        /// 文件命名规则
        /// </summary>
        public string PathFormat { get; set; }

        /// <summary>
        /// 上传表单域名称
        /// </summary>
        public string UploadFieldName { get; set; }

        /// <summary>
        /// 上传大小限制
        /// </summary>
        public int SizeLimit { get; set; }

        /// <summary>
        /// 上传允许的文件格式
        /// </summary>
        public string[] AllowExtensions { get; set; }

        /// <summary>
        /// 文件是否以 Base64 的形式上传
        /// </summary>
        public bool Base64 { get; set; }

        /// <summary>
        /// Base64 字符串所表示的文件名
        /// </summary>
        public string Base64Filename { get; set; }

        /// <summary>
        /// 文件服务器API地址
        /// </summary>
        public string FileServerUrl { get; set; }

        public bool IsLocal { get; set; }
    }

    public class UploadResult
    {
        public UploadState State { get; set; }
        public string Url { get; set; }
        public string OriginFileName { get; set; }

        public string ErrorMessage { get; set; }
    }

    public enum UploadState
    {
        Success = 0,
        SizeLimitExceed = -1,
        TypeNotAllow = -2,
        FileAccessError = -3,
        NetworkError = -4,
        Unknown = 1,
    }
}