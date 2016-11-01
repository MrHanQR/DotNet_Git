using System;
using DotNet.Entity.Enum;

namespace DotNet.Common
{
    public static class ConfigHelper
    {
        public static CacheTypeEnum CacheType
        {
            get
            {
                CacheTypeEnum temp;
                return Enum.TryParse(System.Configuration.ConfigurationManager.AppSettings["CacheType"],out temp)? temp: CacheTypeEnum.HttpRuntime;

            }
        }
    }
}