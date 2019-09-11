using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using {{cookiecutter.project_name}}.Helpers;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace {{cookiecutter.project_name}}.Controllers
{
    public class FaceCompareController : Controller
    {
        DataContext dbContext = new DataContext();

        #region 获取人脸信息
        /// <summary>
        /// 获取全量人脸信息
        /// </summary>
        /// <param name="siteId">客户端Id</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DownLoadFeature(Guid siteId)
        {
            try
            {
                var list = dbContext.Set<SysUser>().Where(x => x.DepartmentId == siteId);
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

                foreach (var user in list)
                {
                    var face = dbContext.SysUserFaceFeature.FirstOrDefault(x => x.UserId == user.UserId);
                    if (face == null) continue;

                    var item = new Dictionary<string, string>
                    {
                        ["ActionName"] = "AddFace",
                        ["Id"] = user.UserId.ToString(),
                        ["Name"] = user.UserName,
                        ["ImageUrl"] = user.UserCode,
                        ["FaceFeature"] = face.FaceFeature,
                        ["TimeOut"] = DateTime.Now.AddYears(100).ToString()
                    };
                    result.Add(item);
                }

                return Json(new { Code = 0, Data = result });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 获取增量人脸信息
        /// </summary>
        /// <param name="siteId">客户端Id</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DownLoadFeatureIncrement(string siteId)
        {
            try
            {
                var list = dbContext.Set<SysQueue>().Where(x => x.ClientId == siteId && (x.ActionName == "AddFace" || x.ActionData== "DeleteFace"));
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
                foreach (var item in list)
                {
                    result.Add(JsonConvert.DeserializeObject<Dictionary<string, string>>(item.ActionData));
                }

                return Json(new { Code = 0, Data = result });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        /// <summary>
        /// 增量人脸处理结果（提交结果确保闭环）
        /// </summary>
        /// <param name="siteId">客户端Id</param>
        /// <param name="id">消息对象Id</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DownLoadFeatureIncrementResult(string siteId, string id)
        {
            try
            {
                //更新队列消息状态。
                var item = dbContext.Set<SysQueue>().FirstOrDefault(x => x.ClientId == siteId && x.ActionObjectId == id);
                if (item != null)
                {
                    dbContext.Set<SysQueue>().Remove(item);
                    dbContext.SaveChanges();

                }

                return Json(new { Code = 0, Msg = "状态更新成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        #endregion

        #region 保存识别结果
        /// <summary>
        /// 识别到人脸后上传图片信息
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<JsonResult> UploadFaceResult(string siteId)
        {
            try
            {
                var files = HttpContextCore.Current.Request.Form.Files;
                if (files.Count != 2)
                    return Json(new { Code = 2, Msg = "请上传图片！" });

                DateTime dateTime = DateTime.Now;
                string file1 = HttpContextCore.MapPath(string.Format("/uploadfiles/FaceResult/{0}/{1:yyyy}/{1:yyyyMMdd}/faces/{1:HHmmssfff}_face_full.jpg", siteId, dateTime));
                string file2 = HttpContextCore.MapPath(string.Format("/uploadfiles/FaceResult/{0}/{1:yyyy}/{1:yyyyMMdd}/faces/{1:HHmmssfff}_face_cut.jpg", siteId, dateTime));

                if (!Directory.Exists(Path.GetDirectoryName(file1)))
                    Directory.CreateDirectory(Path.GetDirectoryName(file1));

                using (var stream = System.IO.File.Create(file1))
                    await files[0].CopyToAsync(stream);
                using (var stream = System.IO.File.Create(file2))
                    await files[1].CopyToAsync(stream);

                return Json(new { Code = 0, Msg = "保存成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 识别到人后上传图片和用户信息
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="id"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public async Task<JsonResult> UploadFaceForUserResult(string siteId, string id,string name, string score)
        {
            try
            {
                var files = HttpContextCore.Current.Request.Form.Files;
                if (files.Count != 2)
                    return Json(new { Code = 2, Msg = "请上传图片！" });

                string file1 = HttpContextCore.MapPath(string.Format("/uploadfiles/FaceResult/{0}/{1:yyyy}/{1:yyyyMMdd}/users/{1:HHmmssfff}_{2}_{3}_full.jpg", siteId, DateTime.Now, name, score));
                string file2 = HttpContextCore.MapPath(string.Format("/uploadfiles/FaceResult/{0}/{1:yyyy}/{1:yyyyMMdd}/users/{1:HHmmssfff}_{2}_{3}_cut.jpg", siteId, DateTime.Now, name, score));

                if (!Directory.Exists(Path.GetDirectoryName(file1)))
                    Directory.CreateDirectory(Path.GetDirectoryName(file1));

                using (var stream = System.IO.File.Create(file1))
                    await files[0].CopyToAsync(stream);
                using (var stream = System.IO.File.Create(file2))
                    await files[1].CopyToAsync(stream);

                return Json(new { Code = 0, Msg = "保存成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
        #endregion
    }
}