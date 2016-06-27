using System.Collections.Concurrent;
using System.Net;
using RestSharp;
using System;
using System.Linq;

namespace TG.CLS
{
    public class ApiReceive:IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {           
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~ApiReceive()
        {
            Dispose();
        }
      
        #endregion

        /// <summary>API路径</summary>
        public string URL { get; set; }

        /// <summary>参数集合</summary>
        public ConcurrentDictionary<string, string> Parameters { get; set; }

        /// <summary>字符数据</summary>
        public string Data { get; set; }
        /// <summary>资源字符数据</summary>
        public string Resource { get; set; }

        /// <summary>ApiReceive构造函数</summary>
        /// <param name="url">API路径</param>
        public ApiReceive(string url)
        {
            URL = url;
            Parameters = new ConcurrentDictionary<string, string>();
        }

        /// <summary>ApiReceive构造函数</summary>
        public ApiReceive() : this(string.Empty) { }

        /// <summary>获取请求JSON数据</summary>
        /// <returns>json字符串</returns>
        public string GetJsonToFromBody()
        {
            if (String.IsNullOrEmpty(URL)) return String.Empty;
            var client = new RestClient(URL);
            var request = CreateRequestJson(Method.GET);
            request.Resource = Resource;
            var response = Execute(client, request);
            var json = response.Content;
            return json;
        }

        /// <summary>获取请求JSON数据(FromBody)方式</summary>
        /// <returns>json字符串</returns>
        public string PostJsonToFromBody()
        {
            if (String.IsNullOrEmpty(URL)) return String.Empty;
            var client = new RestClient(URL);
            var request = CreateRequestPost(Method.POST);
            request.Resource = Resource;
            var response = Execute(client, request);
            var json = response.Content;
            return json;
        }

        /// <summary>获取请求JSON数据(参数方式)Parameter</summary>
        /// <returns>json字符串</returns>
        public string PostJsonToParameter()
        {
            if (String.IsNullOrEmpty(URL)) return String.Empty;
            var client = new RestClient(URL);
            var request = CreateRequestJson(Method.POST);
            request.Resource = Resource;
            if (Parameters.Any())
            {
                foreach (var item in Parameters)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }
            var response = Execute(client, request);
            var json = response.Content;
            return json;
        }


        /// <summary>执行请求</summary>
        /// <param name="client">HTTP请求和响应过程</param>
        /// <param name="request">HTTP请求</param>
        /// <returns>HTTP响应结果</returns>
        private IRestResponse Execute(RestClient client, IRestRequest request)
        {
            //返回的结果
            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(response.Content);
            }
            return response;
        }

        /// <summary>创建请求信息</summary>
        /// <param name="method">请求方式</param>
        /// <returns>用于请求的数据容器</returns>
        private RestRequest CreateRequestJson(Method method)
        {
            var request = new RestRequest { Method = method };
            //request.RequestFormat = DataFormat.Json; //请求传递参数为JSON
            request.AddHeader("Content-Type", "application/json"); //设置HTTP头
            request.AddHeader("Accept", "application/json, */*;"); //设置HTTP头
            return request;

        }

        private RestRequest CreateRequestPost(Method method)
        {
            var request = new RestRequest { Method = method };
            request.RequestFormat = DataFormat.Json; //请求传递参数为JSON
            request.AddHeader("Content-Type", "application/json"); //设置HTTP头
            request.AddHeader("Accept", "application/json");
            request.AddBody(Data);
            return request;

        }


        
    }
}