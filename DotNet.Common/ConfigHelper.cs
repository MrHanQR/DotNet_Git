using System;
using DotNet.Entity.Enum;
using System.Configuration;

namespace DotNet.Common
{
    public static class ConfigHelper
    {
        /// <summary>
        /// 采用的缓存方式
        /// </summary>
        public static CacheTypeEnum CacheType
        {
            get
            {
                CacheTypeEnum temp;
                return Enum.TryParse(ConfigurationManager.AppSettings["CacheType"],out temp)? temp: CacheTypeEnum.HttpRuntime;

            }
        }
        /// <summary>
        /// 验证码长度
        /// </summary>
        public static int CaptchaLength
        {
            get
            {
                int length;
                return int.TryParse(ConfigurationManager.AppSettings["CacheType"], out length) ? length : 4;
            }
        }
        /// <summary>
        /// Dal层程序集的名称
        /// </summary>
        public static string DalAssemblyName
        {
            get { return ConfigurationManager.AppSettings["AssemblyName"]; }
        }
    }
}