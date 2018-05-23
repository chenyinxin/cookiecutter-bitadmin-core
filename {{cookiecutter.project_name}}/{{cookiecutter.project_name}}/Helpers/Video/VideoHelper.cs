/***********************
 * BitAdmin2.0框架文件
 ***********************/
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace {{cookiecutter.project_name}}.Helpers
{
    public class VideoHelper
    {
        /// <summary>
        /// 下载ffmpeg，解压到以下路径。
        /// </summary>
        static string ffmpeg = "E:\\Video\\ffmpeg\\bin\\ffmpeg.exe";
        /// <summary>
        /// 工作目录，命令行使用基于工作目录的相对目录
        /// </summary>
        static string workingDirectory = "E:\\Video\\";

        /// <summary>
        /// 截取视频并加图片水印
        /// </summary>
        /// <param name="inputFile">输出文件名，相对工作目录路径。</param>
        /// <param name="outputFile">输出文件名，相对工作目录路径。</param>
        /// <param name="watermarkFile">图片水印文件｜png</param>
        /// <param name="startTime">开始时间｜00:00:00</param>
        /// <param name="endTime">结束时间｜00:00:15</param>
        /// <returns></returns>
        public static string CutAndWater(string inputFile, string outputFile, string watermarkFile, string startTime, string endTime)
        {
            //如需更多参数，请百度ffmpeg，路径符号不能使用“\”
            string config = string.Format("-y -i {0} -acodec copy -ss {2} -to {3} -vf \"movie={4}[watermark];[in][watermark]overlay=10:10[out]\" {1}",
                inputFile.Replace("\\","/"), outputFile.Replace("\\", "/"), startTime, endTime, watermarkFile.Replace("\\", "/"));

            LogHelper.SaveLog("video", config);

            return Exec(config);
        }

        /// <summary>
        /// 生成水印图片,返回工作目录相对路径。
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string CreateImage(string msg)
        {
            var fileName = Path.Combine("image", string.Format("{0:yyyyMMddHHmmss}{1}.png", DateTime.Now, Guid.NewGuid().ToString().Substring(0, 6)));

            int iImageWidth = msg.Length * 15;
            int iImageHeight = 20;
            Bitmap image = new Bitmap(iImageWidth, iImageHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.Transparent);
            Font font = new Font("微软雅黑", 10, (FontStyle.Regular));
            Rectangle rc = new Rectangle(0, 0, iImageWidth, image.Height);
            LinearGradientBrush brush = new LinearGradientBrush(rc, Color.White, Color.White, 0, false);
            g.DrawString(msg, font, brush, 0, 2);

            image.Save(workingDirectory + fileName, ImageFormat.Png);

            return fileName;
        }
        private static string Exec(string config)
        {
            Process p = new Process();
            p.StartInfo.FileName = ffmpeg;
            p.StartInfo.Arguments = config;
            p.StartInfo.WorkingDirectory = workingDirectory;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = false;
            p.Start();

            return p.StandardError.ReadToEnd();
        }
    }
}
