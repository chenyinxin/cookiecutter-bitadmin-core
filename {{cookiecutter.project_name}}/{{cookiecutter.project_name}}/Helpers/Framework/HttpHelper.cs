using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace {{cookiecutter.project_name}}.Helpers
{
    public class HttpHelper
    {
        /// <summary>
        /// Post普通数据，可包括多个附件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fields"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string HttpPost(string url, Dictionary<string, string> fields, Dictionary<string, string> files)
        {
            HttpClient client = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();
            foreach (var field in fields)
                form.Add(CreateStreamContent(field.Key, field.Value));
            foreach (var file in files)
                form.Add(HttpHelper.CreateByteArrayContent(file.Key, file.Value));
            HttpResponseMessage res = client.PostAsync(url, form).Result;
            return res.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Post Json数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string json)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "application/json";
            request.ContentType = "application/json; charset=utf-8";

            byte[] buffer = Encoding.UTF8.GetBytes(json);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public static string HttpGet(string url)
        {
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json; charset=utf-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public static StreamContent CreateStreamContent(string name, Bitmap img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);
            ms.Position = 0;

            StreamContent fileContent = new StreamContent(ms);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            fileContent.Headers.ContentDisposition.Name = "file";
            fileContent.Headers.ContentDisposition.FileName = name;
            return fileContent;
        }

        public static StreamContent CreateStreamContent(string name, string path)
        {
            StreamContent fileContent = new StreamContent(File.OpenRead(path));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            fileContent.Headers.ContentDisposition.Name = "file";
            fileContent.Headers.ContentDisposition.FileName = name;
            return fileContent;
        }

        public static ByteArrayContent CreateByteArrayContent(string key, string value)
        {
            var dataContent = new ByteArrayContent(Encoding.UTF8.GetBytes(value));
            dataContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = key
            };
            return dataContent;
        }

        public static Bitmap DownLoadBitmap(string url)
        {
            Image _image = Image.FromStream(WebRequest.Create(url).GetResponse().GetResponseStream());
            return new Bitmap(_image, _image.Width, _image.Height);
        }
    }
}
