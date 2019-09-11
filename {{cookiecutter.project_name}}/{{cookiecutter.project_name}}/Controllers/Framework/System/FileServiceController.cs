/***********************
 * BitAdmin2.0框架文件
 ***********************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.SqlClient;
using {{cookiecutter.project_name}}.Models;
using Microsoft.AspNetCore.Mvc;
using {{cookiecutter.project_name}}.Helpers;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace {{cookiecutter.project_name}}.Controllers
{
    /*
     * 如果考虑性能，可以调整逻辑，直接存到文件服务器。
     */
    public class FileServiceController : Controller
    {
        DataContext dbContext = new DataContext();

        readonly string fileServer = "http://localhost:54117/FileService/Upload";
        readonly bool isLocal = true;

        [HttpPost]
        public async Task<JsonResult> Upload()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];

                Guid fileId = Guid.NewGuid();
                DateTime nowDate = DateTime.Now;
                string path = string.Format("/uploadfiles/{0:yyyy/MMdd}/{1}", nowDate, fileId);
                string url = string.Format("{0}/{1}", path, file.FileName);
                // 服务器的存储全路径
                string destFileName = HttpContextCore.MapPath(url);
                // 创建目录路径
                if (!Directory.Exists(Path.GetDirectoryName(destFileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(destFileName));

                SysAttachment fileData = new SysAttachment();
                fileData.Id = fileId;
                //fileDatas.RelationID = "";
                fileData.Name = file.FileName;
                fileData.Names = "";
                fileData.Url = url;
                fileData.Type = 0;
                fileData.Suffix = Path.GetExtension(file.FileName).ToLower();
                fileData.Path = path;
                fileData.Status = 0;
                fileData.Size = 0;
                fileData.CreateBy = "";
                fileData.CreateByName = SSOClient.User.UserName;
                fileData.CreateTime = nowDate;

                string ThumbnailSizes = Request.Form["thumbnailSizes"].FirstOrDefault();
                if (isLocal)
                {
                    //保存本地
                    using (var stream = System.IO.File.Create(destFileName))
                    {
                        await file.CopyToAsync(stream);
                    }
                    // 图片文件扩展名验证正则表达式
                    Regex regexExtension = new Regex(@".*\.(jpg|jpeg|png|gif|bmp)");
                    if (regexExtension.IsMatch(destFileName.ToLower()))
                    {
                        string[] ThumbnailSizeArr = new string[] { };
                        //生成缩略图
                        if (!string.IsNullOrEmpty(ThumbnailSizes) && (ThumbnailSizeArr = ThumbnailSizes.Split(';')).Length > 0)
                        {
                            string[] fileNamesArr = new string[ThumbnailSizeArr.Length];
                            for (int i = 0; i < ThumbnailSizeArr.Length; i++)
                            {
                                string size = ThumbnailSizeArr[i];
                                string ThumbFileName = Path.GetFileNameWithoutExtension(url) + "_" + size + fileData.Suffix;
                                string ThumbPath = url.Replace(Path.GetFileName(url), ThumbFileName);
                                ThumbnailHelper.MakeThumbnail(Convert.ToInt32(size), HttpContextCore.MapPath(url), HttpContextCore.MapPath(ThumbPath));
                                fileNamesArr[i] = ThumbFileName;
                            }
                            fileData.Names = string.Join("|", fileNamesArr);
                        }
                    }
                }
                else
                {
                    //保存文件服务器
                    HttpClient client = new HttpClient();
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    
                    byte[] uploadFileBytes = new byte[file.Length];
                    file.OpenReadStream().Read(uploadFileBytes, 0, (int)file.Length);
                    MemoryStream stream = new MemoryStream(uploadFileBytes);                    
                    StreamContent fileContent = new StreamContent(stream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                    fileContent.Headers.ContentDisposition.FileName = url;
                    form.Add(fileContent);

                    StringContent thumbnailSizes = new StringContent(ThumbnailSizes);
                    thumbnailSizes.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                    thumbnailSizes.Headers.ContentDisposition.Name = "thumbnailSizes";
                    form.Add(thumbnailSizes);

                    HttpResponseMessage res = client.PostAsync(fileServer, form).Result;
                    var json = res.Content.ReadAsStringAsync().Result;
                    JObject result = JObject.Parse(json);

                    fileData.Path = (string)result["data"]["path"];
                    fileData.Names = (string)result["data"]["names"];
                    fileData.Url = (string)result["data"]["url"];

                }
                return Json(new { Code = 0, Msg = "", Data = fileData });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        [HttpGet]
        public ActionResult Download()
        {
            string fileURL = Request.Query["fileURL"];

            if (fileURL.StartsWith("http"))
            {
                return File(new WebClient().DownloadData(fileURL), "application/octet-stream", Path.GetFileName(fileURL));
            }
            else
            {
                string fileName = Path.GetFileName(fileURL);
                FileStream fs = new FileStream(HttpContextCore.MapPath(fileURL), FileMode.Open);
                return File(fs, "application/octet-stream", fileName);
            }
        }

        [HttpPost]
        public ActionResult SaveData(string RelationID, List<SysAttachment> files)
        {
            try
            {
                if (string.IsNullOrEmpty(RelationID))
                {
                    return Json(new { Code = 1, Msg = "保存失败，关联ID不能为空" });
                }
                //先删除
                var models = dbContext.SysAttachment.Where(x => x.RelationId == RelationID);
                if (models != null)
                    dbContext.SysAttachment.RemoveRange(models);

                dbContext.SaveChanges();
                if (files == null || files.Count == 0)
                {
                    return Json(new { Code = 0, Msg = "保存成功" });
                }

                var users = dbContext.SysUser.ToList();
                //后添加
                for (int i = 0; i < files.Count; i++)
                {
                    SysAttachment model = files[i];
                    model.RelationId = RelationID;
                    model.Status = 0;
                    model.Size = 0;
                    model.CreateBy = string.IsNullOrEmpty(model.CreateBy) ? Convert.ToString(SSOClient.UserId) : model.CreateBy;
                    model.CreateByName = users.FirstOrDefault(t => t.UserId.ToString() == model.CreateBy).UserName;
                    dbContext.SysAttachment.Add(model);
                }
                dbContext.SaveChanges();
                return Json(new { Code = 0, Msg = "保存成功", Data = files });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        [HttpPost]
        public ActionResult QueryData()
        {
            try
            {
                string strSQL = @"select t1.[id],t1.[relationID],t1.[name],t1.[url],t1.[type],t1.[suffix],t1.[path],t1.[names],t1.[status],t1.[size],t1.[createBy],t2.userName createByName,t1.[createTime] from SysAttachment t1 
left join SysUser t2 on t1.CreateBy=t2.UserID
where t1.RelationID=@RelationID order by t1.createTime asc";
                DataSet ds = SqlHelper.Query(strSQL, new SqlParameter("@RelationID", Request.Form["RelationID"].FirstOrDefault()));

                return Json(new { Code = 0, Data = QuerySuite.ToDictionary(ds.Tables[0]) });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
    }

    public static class ThumbnailHelper
    {
        public static void MakeThumbnail(int thumbWidth, string oriImagePath, string thumbImagePath)
        {
            using (Image oriImage = Image.FromFile(oriImagePath))
            {
                int oriWidth = oriImage.Width;
                int oriHeight = oriImage.Height;

                //int thumbWidth = 360;
                int thumbHeight = oriImage.Height * thumbWidth / oriImage.Width;

                //新建一个指定大小的Bitmap图片
                Image thumbBitmap = new Bitmap(thumbWidth, thumbHeight);

                using (Graphics graphics = Graphics.FromImage(thumbBitmap))
                {
                    //设置高质量插值法
                    graphics.PixelOffsetMode = PixelOffsetMode.Half;
                    graphics.InterpolationMode = InterpolationMode.High;

                    //设置高质量,低速度呈现平滑程度
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    //清空画布并以透明背景色填充
                    graphics.Clear(Color.Transparent);

                    //在指定位置并且按指定大小绘制原图片的指定部分
                    graphics.DrawImage(oriImage, new Rectangle(0, 0, thumbWidth, thumbHeight), new Rectangle(0, 0, oriWidth, oriHeight), GraphicsUnit.Pixel);
                }

                try
                {
                    if (!Directory.Exists(Path.GetDirectoryName(thumbImagePath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(thumbImagePath));

                    //以jpg格式保存缩略图
                    thumbBitmap.Save((thumbImagePath), ImageFormat.Jpeg);
                }
                catch
                { }
                finally
                {
                    if (thumbBitmap != null)
                        thumbBitmap.Dispose();
                }
            }
        }
    }
}