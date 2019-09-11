using {{cookiecutter.project_name}}.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace {{cookiecutter.project_name}}.BLL
{
    /// <summary>
    /// 人脸识别客户端辅助逻辑
    /// </summary>
    public class FaceCompareBLL
    {
        /// <summary>
        /// 更新客户端用户人脸特征库
        /// </summary>
        public static void UpdateUserFace(SysUser user)
        {
            DataContext dbContext = new DataContext();

            HttpClient client = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(CreateStreamContent(HttpContextCore.MapPath(user.UserImage), "face.jpg"));
            HttpResponseMessage res = client.PostAsync("http://api.bitdao.cn/FaceCompare/ExtractFeature", form).Result;
            var json = res.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<UploadResult>(json);

            if (string.IsNullOrEmpty(result.Data))
                return;

            //保存特征
            var feature = dbContext.Set<SysUserFaceFeature>().FirstOrDefault(x => x.UserId == user.UserId);
            if (feature == null)
            {
                feature = new SysUserFaceFeature
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    UserType = "User",
                    FaceImage = user.UserImage,
                    FaceFeature = result.Data,
                    FaceFeatureType = "",
                    FaceTimeOut = DateTime.Now.AddYears(100),
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                dbContext.Set<SysUserFaceFeature>().Add(feature);
            }
            else
            {
                feature.UserName = user.UserName;
                feature.FaceImage = user.UserImage;
                feature.FaceFeature = result.Data;
                feature.UpdateTime = DateTime.Now;
            }

            //更新客户端队列（根据业务修改）
            var data = new Dictionary<string, string>
            {
                ["Id"] = user.UserId.ToString(),
                ["Name"] = user.UserName,
                ["ImageUrl"] = user.UserImage,
                ["FaceFeature"] = result.Data,
                ["TimeOut"] = DateTime.Now.AddYears(100).ToString()
            };

            SysQueue queue = new SysQueue
            {
                Id = Guid.NewGuid(),
                ClientId = user.DepartmentId.ToString(),
                ActionName = "UpdateFace",
                ActionObjectId = user.UserId.ToString(),
                ActionData = JsonConvert.SerializeObject(data),
                CreateTime = DateTime.Now
            };

            dbContext.Set<SysQueue>().Add(queue);
            dbContext.SaveChanges();
        }
        private static StreamContent CreateStreamContent(string imgPath, string name)
        {
            StreamContent fileContent = new StreamContent(File.OpenRead(imgPath));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            fileContent.Headers.ContentDisposition.Name = "file";
            fileContent.Headers.ContentDisposition.FileName = name;
            return fileContent;
        }
        private class UploadResult
        {
            public int Code { get; set; }
            public string Msg { get; set; }
            public string Data { get; set; }
        }
    }
}
