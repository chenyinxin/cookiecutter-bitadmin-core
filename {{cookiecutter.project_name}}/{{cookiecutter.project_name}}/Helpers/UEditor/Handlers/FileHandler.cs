using {{cookiecutter.project_name}}.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;

namespace {{cookiecutter.project_name}}.UEditor.Handlers
{
    public class FileHandler
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="uploadUrl"></param>
        /// <param name="filePath"></param>
        public FileServiceResult UploadFile(Stream fs,string uploadUrl, string filePath)
        {
            FileServiceResult param = new FileServiceResult();
            try
            {
                HttpClient client = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                StreamContent fileContent = new StreamContent(fs);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                fileContent.Headers.ContentDisposition.FileName = filePath;
                form.Add(fileContent);
                HttpResponseMessage res = client.PostAsync(uploadUrl, form).Result;
                var uploadModel = res.Content.ReadAsStringAsync().Result;
                param = JsonConvert.DeserializeObject<FileServiceResult>(uploadModel);
            }
            catch (Exception ex)
            {
                param.Code = 1;
                param.Msg = "服务器异常，请联系管理员！";
                LogHelper.SaveLog(ex);
            }
            return param;
        }

        public class FileServiceResult
        {
            public int Code { get; set; }

            public string Msg { get; set; }

            public List<string> Data { get; set; }
        }
    }
}
