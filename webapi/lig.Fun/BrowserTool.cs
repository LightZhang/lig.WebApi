using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace lig.Fun
{
    public static class BrowserTool
    {
       static  HttpContextAccessor  context = new HttpContextAccessor();

        public static string GetRootFullPath()
        {
            string rootdir = AppContext.BaseDirectory;
            DirectoryInfo Dir = Directory.GetParent(rootdir);
            string root = Dir.Parent.Parent.FullName;
            return root;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public static string GetAppPath()
        {
           
            string result;
            if (AppContext.BaseDirectory != null)
            {
                result = BrowserTool.GetRootFullPath();
            }
            else
            {
                result = AppDomain.CurrentDomain.BaseDirectory;
            }
            return result;
        }

        public static string GetIp()
        {
            var ip = context.HttpContext?.Connection.RemoteIpAddress.ToString();
            return ip;
        }

        public static string GetOS()
        {

            var httpBrowserCapabilities = context.HttpContext.Request.Headers;

            return "";
         
        }

        public static string GetBrowser()
        {

            return "";
        }

        public static string GetUrl()
        {
            return "";
        }



    }
}
