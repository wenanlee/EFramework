using SufeiUtil;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFramework.Network
{
    class HTTPClient:IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Test ()
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "http://www.sufeinet.com",//URL这里都是测试     必需项
                Method = "get",//URL     可选项 默认为Get
            };
            //得到HTML代码
            HttpResult result = http.GetHtml(item);
            item = new HttpItem()
            {
                URL = "http://tool.sufeinet.com",//URL这里都是测试URl   必需项
                Encoding = null,//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别
                                //Encoding = Encoding.Default,
                Method = "post",//URL     可选项 默认为Get
                Postdata = "user=123123&pwd=1231313"
            };
            //得到新的HTML代码
            result = http.GetHtml(item);
        }
    }
}
