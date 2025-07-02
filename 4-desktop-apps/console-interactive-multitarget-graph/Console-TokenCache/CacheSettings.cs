using Microsoft.Identity.Client.Extensions.Msal;
using System.Collections.Generic;
using System.IO;

namespace Console_TokenCache
{
    public static class CacheSettings
    {
        // computing the root directory is not very simple on Linux and Mac, so a helper is provided
        private static readonly string s_cacheFilePath =
                   Path.Combine(MsalCacheHelper.UserRootDirectory, "msal.contoso.cache");

        public static readonly string CacheFileName = Path.GetFileName(s_cacheFilePath);
        public static readonly string CacheDir = Path.GetDirectoryName(s_cacheFilePath);


        public static readonly string KeyChainServiceName = "Contoso.MyProduct";
        public static readonly string KeyChainAccountName = "MSALCache";

        public static readonly string LinuxKeyRingSchema = "com.contoso.devtools.tokencache";
        public static readonly string LinuxKeyRingCollection = MsalCacheHelper.LinuxKeyRingDefaultCollection;
        public static readonly string LinuxKeyRingLabel = "MSAL token cache for all Contoso dev tool apps.";
        public static readonly KeyValuePair<string, string> LinuxKeyRingAttr1 = new KeyValuePair<string, string>("Version", "1");
        public static readonly KeyValuePair<string, string> LinuxKeyRingAttr2 = new KeyValuePair<string, string>("ProductGroup", "MyApps");
    }
}
