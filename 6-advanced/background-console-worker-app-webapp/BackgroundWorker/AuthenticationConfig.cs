using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.IO;

namespace BackgroundWorker
{
    // Class to read the keys from appsettings.json
    public class AuthenticationConfig
    {
        /// <summary>
        /// instance of Microsoft Entra, for example public Azure or a Sovereign cloud (Azure China, Germany, US government, etc ...)
        /// </summary>
        public string Instance { get; set; } = "https://login.microsoftonline.com/{0}";

        /// The Tenant is:
        /// - either the tenant ID of the Microsoft Entra tenant in which this application is registered (a guid)
        /// or a domain name associated with the tenant
        /// - or 'organizations' (for a multi-tenant application)
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Guid used by the application to uniquely identify itself to Microsoft Entra
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// URL of the authority
        /// </summary>
        public string Authority
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, Instance, TenantId);
            }
        }

        /// <summary>
        /// Client secret (application password)
        /// </summary>
        /// <remarks>Daemon applications can authenticate with AAD through two mechanisms: ClientSecret
        /// (which is a kind of application password: this property)
        /// or a certificate previously shared with AzureAD during the application registration
        /// (and identified by the CertificateName property belows)
        /// <remarks>
        public string ClientSecret { get; set; }

        public string TokenCacheDbConnStr { get; set; }
        public string TokenCacheRedisConnStr { get; set; }

        public string TokenCacheRedisInstaceName { get; set; }

        /// <summary>
        /// Reads the configuration from a json file
        /// </summary>
        /// <param name="path">Path to the configuration json file</param>
        /// <returns>AuthenticationConfig read from the json file</returns>
        public static AuthenticationConfig ReadFromJsonFile(string path)
        {
            IConfigurationRoot Configuration;

            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path);

            Configuration = builder.Build();
            return Configuration.Get<AuthenticationConfig>();
        }
    }
}