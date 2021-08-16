using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
namespace EFramework.Network.Http
{
    /// <summary>
    ///HttpRequestHelper.HttpPostRequestAsync("http://api.ugmax.cn:8000/ugwpage/echo.php", "str=ddd", (string x)=> {Debug.Log(x);});
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// Http Get Request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGetRequest(string url)
        {
            string strGetResponse = string.Empty;
            try
            {
                var getRequest = CreateHttpRequest(url, "GET");
                var getResponse = getRequest.GetResponse() as HttpWebResponse;
                strGetResponse = GetHttpResponse(getResponse, "GET");
            }
            catch (Exception ex)
            {
                strGetResponse = ex.Message;
            }
            return strGetResponse;
        }

        /// <summary>
        /// Http Get Request Async
        /// </summary>
        /// <param name="url"></param>
        public static async void HttpGetRequestAsync(string url, Action<string> action)
        {
            string strGetResponse = string.Empty;
            try
            {
                var getRequest = CreateHttpRequest(url, "GET");
                var getResponse = await getRequest.GetResponseAsync() as HttpWebResponse;
                strGetResponse = GetHttpResponse(getResponse, "GET");
            }
            catch (Exception ex)
            {
                strGetResponse = ex.Message;
            }
            action(strGetResponse);
            //Debug.Log("reslut:" + strGetResponse);
        }

        /// <summary>
        /// Http Post Request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postJsonData"></param>
        /// <returns></returns>
        public static string HttpPostRequest(string url, string postJsonData)
        {
            string strPostReponse = string.Empty;
            try
            {
                var postRequest = CreateHttpRequest(url, "POST", postJsonData);
                var postResponse = postRequest.GetResponse() as HttpWebResponse;
                strPostReponse = GetHttpResponse(postResponse, "POST");
            }
            catch (Exception ex)
            {
                strPostReponse = ex.Message;
            }
            return strPostReponse;
        }

        /// <summary>
        /// Http Post Request Async
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postJsonData"></param>
        public static async void HttpPostRequestAsync(string url, string postData)
        {
            string strPostReponse = string.Empty;
            try
            {
                var postRequest = CreateHttpRequest(url, "POST", postData);
                var postResponse = await postRequest.GetResponseAsync() as HttpWebResponse;
                strPostReponse = GetHttpResponse(postResponse, "POST");
                Debug.Log(strPostReponse.Length);
            }
            catch (Exception ex)
            {
                strPostReponse = ex.Message;
            }
            if (strPostReponse != "true")
            {
                Debug.Log("--> reslut : " + strPostReponse);
                Debug.Log(postData);
            }
        }
        public static async void HttpPostRequestAsync(string url, string postData, Action<string> action)
        {
            string strPostReponse = string.Empty;
            try
            {
                var postRequest = CreateHttpRequest(url, "POST", postData);
                var postResponse = await postRequest.GetResponseAsync() as HttpWebResponse;
                strPostReponse = GetHttpResponse(postResponse, "POST");
                Debug.Log(strPostReponse.Length);
            }
            catch (Exception ex)
            {
                strPostReponse = ex.Message;
            }
            if (strPostReponse != "true")
            {
                action(strPostReponse);
                //Debug.Log("--> reslut : " + strPostReponse);
                //Debug.Log(postData);
            }
        }
        private static HttpWebRequest CreateHttpRequest(string url, string requestType, params object[] strJson)
        {
            HttpWebRequest request = null;
            const string get = "GET";
            const string post = "POST";
            if (string.Equals(requestType, get, StringComparison.OrdinalIgnoreCase))
            {
                request = CreateGetHttpWebRequest(url);
            }
            if (string.Equals(requestType, post, StringComparison.OrdinalIgnoreCase))
            {
                request = CreatePostHttpWebRequest(url, strJson[0].ToString());
            }
            return request;
        }

        private static HttpWebRequest CreateGetHttpWebRequest(string url)
        {
            var getRequest = HttpWebRequest.Create(url) as HttpWebRequest;
            getRequest.Method = "GET";
            getRequest.Timeout = 5000;
            getRequest.ContentType = "text/html;charset=UTF-8";
            getRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return getRequest;
        }

        private static HttpWebRequest CreatePostHttpWebRequest(string url, string postData)
        {
            var postRequest = HttpWebRequest.Create(url) as HttpWebRequest;
            postRequest.KeepAlive = false;
            postRequest.Timeout = 5000;
            postRequest.Method = "POST";
            postRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            postRequest.ContentLength = postData.Length;
            postRequest.AllowWriteStreamBuffering = false;
            StreamWriter writer = new StreamWriter(postRequest.GetRequestStream(), Encoding.ASCII);
            writer.Write(postData);
            writer.Flush();
            return postRequest;
        }

        private static string GetHttpResponse(HttpWebResponse response, string requestType)
        {
            var responseResult = "";
            const string post = "POST";
            string encoding = "UTF-8";
            if (string.Equals(requestType, post, StringComparison.OrdinalIgnoreCase))
            {
                encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8";
                }
            }
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding)))
            {
                responseResult = reader.ReadToEnd();
            }
            return responseResult;
        }

        private static string GetHttpResponseAsync(HttpWebResponse response, string requestType)
        {
            var responseResult = "";
            const string post = "POST";
            string encoding = "UTF-8";
            if (string.Equals(requestType, post, StringComparison.OrdinalIgnoreCase))
            {
                encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8";
                }
            }
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding)))
            {
                responseResult = reader.ReadToEnd();
            }
            return responseResult;
        }
    }
}
