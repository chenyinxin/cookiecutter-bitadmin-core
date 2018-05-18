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
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.DrawingCore.Drawing2D;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace {{cookiecutter.project_name}}.Controllers
{
    public class FileServiceController : Controller
    {
        //private static readonly int _downloadSize = 100 * 1024 * 1024;//分块下载，每次100M（web服务器下载到客户端）
        //private static readonly int _uploadSize = 20 * 1024 * 1024;//分块上传，每次10M（web服务器上传到文件服务器）
        //private static readonly int _sDownloadSize = 20 * 1024 * 1024;//分块上传，每次10M（文件服务器下载到web服务器）
        DataContext dbContext = new DataContext();

        /// <summary>
        /// 上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Upload()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];

                Guid fileId = Guid.NewGuid();
                DateTime nowDate = DateTime.Now;
                string path = string.Format("/UploadFiles/{0:yyyyMMdd}/{1}", nowDate, fileId);
                string url = string.Format("{0}/{1}", path, file.FileName);
                // 服务器的存储全路径
                string destFileName = MapPath(url); 
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
                using (var stream = System.IO.File.Create(destFileName))
                {
                    await file.CopyToAsync(stream);
                }
                // 图片文件扩展名验证正则表达式
                Regex regexExtension = new Regex(@".*\.(jpg|jpeg|png|gif|bmp)");
                if (regexExtension.IsMatch(destFileName.ToLower()))
                {
                    string ThumbnailSizes = Request.Form["thumbnailSizes"].FirstOrDefault();
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
                            ThumbnailHelper.MakeThumbnail(Convert.ToInt32(size), MapPath(url), MapPath(ThumbPath));
                            fileNamesArr[i] = ThumbFileName;
                        }
                        fileData.Names = string.Join("|", fileNamesArr);
                    }
                }
                return Json(new { Code = 0, Msg = "", Data = fileData });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileResult Download()
        {
            string fileURL = Request.Query["fileURL"];
            string filePath = MapPath(fileURL);
            string fileName = Path.GetFileName(fileURL);
            return File(fileName, "application/octet-stream");
        }
        private string MapPath(string url)
        {
            return AppDomain.CurrentDomain.BaseDirectory + url.Replace("/", "\\");
        }
        #region 文件服务器（暂不实现）

        //[HttpPost]
        //public JsonResult FileServiceUpload()
        //{
        //    try
        //    {
        //        IFormFile file = Request.Form.Files[0];
        //        FileService fileservice = new FileService();

        //        int bufferSize = _uploadSize;
        //        byte[] buffer = null;

        //        long fileLength = file.InputStream.Length;//文件流的长度
        //        int tempCount = 0;//当前已经读取的次数

        //        UploadResult fileServiceResult = null;
        //        string sffilePath = "";
        //        SysAttachment fileData = new SysAttachment();

        //        while (file.InputStream.Position < fileLength)
        //        {
        //            bool IsLast = false;
        //            bufferSize = (int)Math.Min(fileLength - file.InputStream.Position, bufferSize);
        //            buffer = new byte[bufferSize];
        //            file.InputStream.Read(buffer, 0, bufferSize);

        //            if (file.InputStream.Position == fileLength)
        //            {
        //                IsLast = true;
        //            }

        //            if (tempCount == 0)
        //            {
        //                fileServiceResult = fileservice.Upload("admin", "admin", buffer, Request.Form["thumbnailSizes"], file.FileName, IsLast, "Default", null);
        //                if (fileServiceResult.Code == 1)
        //                {
        //                    return Json(new { Code = 1, Msg = fileServiceResult.Msg });
        //                }
        //                sffilePath = fileServiceResult.filePath;
        //                var uri = new Uri(fileservice.Url);
        //                string host = uri.AbsoluteUri.Replace(uri.AbsolutePath, "");
        //                fileData.Id = fileServiceResult.Data.ID;
        //                //fileDatas.RelationID = "";
        //                fileData.Name = fileServiceResult.Data.Name;
        //                fileData.Names = fileServiceResult.Data.Names;
        //                fileData.Url = host + fileServiceResult.Data.URL;
        //                fileData.Type = fileServiceResult.Data.Type;
        //                fileData.Suffix = fileServiceResult.Data.Suffix;
        //                fileData.Path = host + fileServiceResult.Data.Path;
        //                fileData.Status = fileServiceResult.Data.Status;
        //                fileData.Size = fileServiceResult.Data.Size;
        //                fileData.CreateBy = fileServiceResult.Data.CreateBy;
        //                fileData.CreateByName = SSOClient.User.UserName;
        //                fileData.CreateTime = fileServiceResult.Data.CreateDate;
        //            }
        //            else
        //            {
        //                fileServiceResult = fileservice.Upload("admin", "admin", buffer, Request.Form["thumbnailSizes"], file.FileName, IsLast, "Default", sffilePath);
        //                fileData.Names = fileServiceResult.Data.Names;
        //                if (fileServiceResult.Code == 1)
        //                {
        //                    return Json(new { Code = 1, Msg = fileServiceResult.Msg });
        //                }
        //            }
        //            tempCount++;
        //        }

        //        return Json(new { Code = 0, Msg = "", Data = fileData });
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.SaveLog(ex);
        //        return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
        //    }
        //}

        //[HttpGet]
        //public void FileServiceDownload()
        //{
        //    FileService fileservice = new FileService();
        //    string fileURL = Request.Query["fileURL"];
        //    var uri = new Uri(fileservice.Url);
        //    string filePath = fileURL.Replace(uri.AbsoluteUri.Replace(uri.AbsolutePath, ""), "");
        //    string fileName = Path.GetFileName(filePath);

        //    long Position = 0;
        //    DownloadResult result = fileservice.Download("admin", "admin", filePath, _sDownloadSize, Position);
        //    long totalLength = result.totalLength;

        //    Response.ContentType = "application/octet-stream";
        //    Response.Headers.Add("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName));
        //    Response.Headers.Add("Content-Length", totalLength.ToString());
        //    if (totalLength == 0)
        //    {
        //        Response.End();
        //    }
        //    while (totalLength > 0 && Response.IsClientConnected)
        //    {
        //        if (Position != 0)
        //        {
        //            result = fileservice.Download("admin", "admin", filePath, _sDownloadSize, Position);
        //        }

        //        Response.OutputStream.Write(result.Data, 0, result.Data.Length);//写入到响应的输出流
        //        Response.Flush();//刷新响应
        //        totalLength = totalLength - result.Data.Length;
        //        Position += result.Data.Length;
        //    }

        //    Response.Close();//文件传输完毕，关闭相应流
        //}

        #endregion

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="ExampleID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
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


        public ActionResult CKUpload()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            //result["140"] = "http://localhost:51671/uploadfiles/bitdao.png/w_140";
            //result["220"] = "http://localhost:51671/uploadfiles/bitdao.png/w_220";
            //result["300"] = "http://localhost:51671/uploadfiles/bitdao.png/w_300";
            result["default"] = "http://localhost:51671/uploadfiles/bitdao.png";
            return Json(result);
        }
    }

    public static class ThumbnailHelper
    {
        public static void MakeThumbnail(int thumbWidth, string oriImagePath, string thumbImagePath)
        {
            Image thumbBitmap = null;
            NewMethod(thumbWidth, oriImagePath, out thumbBitmap);

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

        private static void NewMethod(int thumbWidth, string oriImagePath, out Image thumbBitmap)
        {
            using (Image oriImage = Image.FromFile(oriImagePath))
            {
                int oriWidth = oriImage.Width;
                int oriHeight = oriImage.Height;

                //int thumbWidth = 360;
                int thumbHeight = oriImage.Height * thumbWidth / oriImage.Width;

                //新建一个指定大小的Bitmap图片
                thumbBitmap = new Bitmap(thumbWidth, thumbHeight);

                using (Graphics graphics = Graphics.FromImage(thumbBitmap))
                {
                    //设置高质量插值法
                    graphics.PixelOffsetMode = PixelOffsetMode.Half;
                    graphics.InterpolationMode = InterpolationMode.High;

                    //设置高质量,低速度呈现平滑程度
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    //清空画布并以透明背景色填充
                    graphics.Clear(System.DrawingCore.Color.Transparent);

                    //在指定位置并且按指定大小绘制原图片的指定部分
                    graphics.DrawImage(oriImage, new System.DrawingCore.Rectangle(0, 0, thumbWidth, thumbHeight), new System.DrawingCore.Rectangle(0, 0, oriWidth, oriHeight), GraphicsUnit.Pixel);
                }
            }
        }
    }
}