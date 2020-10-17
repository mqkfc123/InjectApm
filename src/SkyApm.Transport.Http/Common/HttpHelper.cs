using System;
using System.IO;
using System.Net;
using System.Text;

namespace SkyApm.Transport.Http.Common
{
    public class HttpHelper
    {

        /// <summary>
        /// 发起通信请求(GET方式)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetMode(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            myRequest.Timeout = 6000;
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();
            reader.Close();
            return content;
        }

        /// <summary>
        /// 发起通信请求(Post方式)
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string PostMode(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Http下载文件
        /// </summary>
        public static void HttpDownloadFile(string url, string path, Action<double> action)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.AllowAutoRedirect = true;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream st = response.GetResponseStream();
                var stLength = response.ContentLength;

                int total = (int)(stLength / 100);
                Stream so = new FileStream(path, FileMode.Create);

                byte[] by = new byte[4096];
                int osize = st.Read(by, 0, by.Length);
                var si = 0;
                while (osize > 0)
                {
                    so.Write(by, 0, osize);
                    osize = st.Read(by, 0, by.Length);

                    si += osize;
                    if (si >= total)
                    {
                        si = 0;
                        action.Invoke(1);
                    }
                }
                so.Close();
                st.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Http上传文件
        /// </summary>
        /// <param name="clientId">客户端标识（用户id，mac，ip）</param>
        /// <param name="dateTime"></param>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        public static string HttpUploadFile(string clientId, int clientType, string dateTime, string url, string filePath)
        {
            try
            {
                #region 将文件转成二进制
                var fileName = Path.GetFileName(filePath);

                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileContentByte = new byte[fs.Length]; // 二进制文件
                fs.Read(fileContentByte, 0, Convert.ToInt32(fs.Length));
                fs.Close();

                #endregion

                #region 定义请求体中的内容 并转成二进制

                string boundary = $"------------------{DateTime.Now.ToString("yyyyMMddHHmmss")}";
                string Enter = "\r\n";

                //var clientIdStr = $"--{boundary}{Enter}Content-Disposition: form-data; name=\"clientId\"{Enter}{Enter}{clientId}{Enter}";
                //var clientTypeStr = $"--{boundary}{Enter}Content-Disposition: form-data; name=\"clientType\"{Enter}{Enter}{clientType}{Enter}";
                var fileContentStr = $"--{boundary}{Enter}Content-Type:multipart/form-data{Enter}Content-Disposition: form-data; name=\"file\"; filename=\"{fileName}\"{Enter}{Enter}";
                //var dateTimeStr = $"--{boundary}{Enter}Content-Disposition: form-data; name=\"dateTime\"{Enter}{Enter}{dateTime}{Enter}";

                //var modelIdStrByte = Encoding.UTF8.GetBytes(clientIdStr);//modelId所有字符串二进制
                //var clientTypeStrByte = Encoding.UTF8.GetBytes(clientTypeStr);
                var fileContentStrByte = Encoding.UTF8.GetBytes(fileContentStr);//fileContent一些名称等信息的二进制（不包含文件本身）
                //var dateTimeStrByte = Encoding.UTF8.GetBytes(dateTimeStr);//dateTime所有字符串二进制 
                var footerStrByte = Encoding.UTF8.GetBytes($"--{boundary}--{Enter}");//结尾
                #endregion
                url += $"?clientId={clientId}&clientType={clientType}&dateTime={dateTime}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "multipart/form-data;boundary=" + boundary;

                Stream myRequestStream = request.GetRequestStream();

                #region 将各个二进制 按顺序写入请求流 clientIdStr -> clientTypeStr -> (fileContentStr + fileContent) -> dateTimeStrByte

                //myRequestStream.Write(modelIdStrByte, 0, modelIdStrByte.Length);
                // myRequestStream.Write(clientTypeStrByte, 0, clientTypeStrByte.Length);
                myRequestStream.Write(fileContentStrByte, 0, fileContentStrByte.Length);
                myRequestStream.Write(fileContentByte, 0, fileContentByte.Length);
                //myRequestStream.Write(dateTimeStrByte, 0, dateTimeStrByte.Length);
                myRequestStream.Write(footerStrByte, 0, footerStrByte.Length);

                #endregion

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

                string retStr = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                myResponseStream.Close();

                return retStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
