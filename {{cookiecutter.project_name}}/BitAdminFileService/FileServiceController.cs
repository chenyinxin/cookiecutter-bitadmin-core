/***********************
 * BitAdmin2.0框架文件
 ***********************/
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using {{cookiecutter.project_name}}.Helpers;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace {{cookiecutter.project_name}}.Controllers
{
    public class FileServiceController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Content("这是一个文件服务器。");
        }

        [HttpPost]
        public async Task<JsonResult> Upload()
        {
            try
            {
                string host = string.Format("{0}://{1}", Request.Scheme, Request.Host);
                Dictionary<string, string> result = new Dictionary<string, string>();

                IFormFile file = Request.Form.Files[0];

                // 服务器的存储全路径
                string destFileName = MapPath(file.FileName);
                result.Add("url", host + file.FileName);
                result.Add("path", host + file.FileName.Substring(0, file.FileName.LastIndexOf("/")));
                
                // 创建目录路径
                if (!Directory.Exists(Path.GetDirectoryName(destFileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(destFileName));

                var suffix = Path.GetExtension(file.FileName).ToLower();
                string ThumbnailSizes = Request.Form["thumbnailSizes"].FirstOrDefault();

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
                            string ThumbFileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + size + suffix;
                            string ThumbPath = file.FileName.Replace(Path.GetFileName(file.FileName), ThumbFileName);
                            ThumbnailHelper.MakeThumbnail(Convert.ToInt32(size), MapPath(file.FileName), MapPath(ThumbPath));
                            fileNamesArr[i] = ThumbFileName;
                        }
                        result.Add("names", string.Join("|", fileNamesArr));
                    }
                }

                //返回所有地址
                return Json(new { Code = 0, Msg = "", Data = result });
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UEditor()
        {
            try
            {
                string host = string.Format("{0}://{1}", Request.Scheme, Request.Host);
                List<string> urls = new List<string>();

                IFormFile file = Request.Form.Files[0];

                Guid fileId = Guid.NewGuid();
                DateTime nowDate = DateTime.Now;
                // 服务器的存储全路径
                string destFileName = MapPath(file.FileName);
                urls.Add(host + file.FileName);
                // 创建目录路径
                if (!Directory.Exists(Path.GetDirectoryName(destFileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(destFileName));

                var suffix = Path.GetExtension(file.FileName).ToLower();

                string ThumbnailSizes = Request.Form["thumbnailSizes"].FirstOrDefault();

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
                        for (int i = 0; i < ThumbnailSizeArr.Length; i++)
                        {
                            string size = ThumbnailSizeArr[i];
                            string ThumbFileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + size + suffix;
                            string ThumbPath = file.FileName.Replace(Path.GetFileName(file.FileName), ThumbFileName);
                            ThumbnailHelper.MakeThumbnail(Convert.ToInt32(size), MapPath(file.FileName), MapPath(ThumbPath));

                            urls.Add(host + ThumbPath);
                        }
                    }
                }

                //返回所有地址
                return Json(new { Code = 0, Msg = "", Data = urls });
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
            string fileName = Path.GetFileName(fileURL);
            FileStream fs = new FileStream(MapPath(fileURL), FileMode.Open);
            return File(fs, "application/octet-stream", fileName);
        }
        private string MapPath(string url)
        {
            return AppDomain.CurrentDomain.BaseDirectory + url.Replace("/", "\\");
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
                    graphics.Clear(Color.Transparent);

                    //在指定位置并且按指定大小绘制原图片的指定部分
                    graphics.DrawImage(oriImage, new Rectangle(0, 0, thumbWidth, thumbHeight), new Rectangle(0, 0, oriWidth, oriHeight), GraphicsUnit.Pixel);
                }
            }
        }
    }
}