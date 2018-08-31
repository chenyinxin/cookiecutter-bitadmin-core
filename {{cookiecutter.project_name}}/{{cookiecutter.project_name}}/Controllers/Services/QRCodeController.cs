/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}}.Helpers;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace {{cookiecutter.project_name}}.Controllers
{
    public class QRCodeController : Controller
    {
        public ActionResult Encode(string msg = "https://www.bitadmincore.com/")
        {
            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(msg, QRCodeGenerator.ECCLevel.L);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                
                MemoryStream ms = new MemoryStream();
                qrCodeImage.Save(ms, ImageFormat.Png);
                byte[] bytes = ms.GetBuffer();
                ms.Close();
                return File(bytes, "image/jpeg");
            }
            catch (Exception ex)
            {
                LogHelper.SaveLog(ex);
                return Json(new { Code = 1, Msg = "服务器异常，请联系管理员！" });
            }
        }
    }
}
