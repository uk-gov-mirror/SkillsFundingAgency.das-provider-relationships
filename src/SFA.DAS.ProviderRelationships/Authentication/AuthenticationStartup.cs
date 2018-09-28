﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.NLog.Logger;
using SFA.DAS.OidcMiddleware;
using SFA.DAS.ProviderRelationships.Authentication.Interfaces;
using SFA.DAS.ProviderRelationships.Configuration.Interfaces;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    public class AuthenticationStartup : IAuthenticationStartup
    {
        private readonly IAppBuilder _app;
        private readonly IIdentityServerConfiguration _config;
        private readonly IAuthenticationUrls _authenticationUrls;
        private readonly ConfigurationFactory _configurationFactory;
        private readonly ILog _logger;

        public AuthenticationStartup(
            IAppBuilder app,
            IIdentityServerConfiguration config,
            IAuthenticationUrls authenticationUrls,
            ConfigurationFactory configurationFactory,
            ILog logger)
        {
            _app = app;
            _config = config;
            _authenticationUrls = authenticationUrls;
            _configurationFactory = configurationFactory;
            _logger = logger;
        }

        public void Initialise()
        {
            _logger.Info("Initialising Authentication");

            _app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
                ExpireTimeSpan = new TimeSpan(0, 10, 0),
                SlidingExpiration = true
            });

            _app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "TempState",
                AuthenticationMode = AuthenticationMode.Passive
            });

            _app.UseCodeFlowAuthentication(new OidcMiddlewareOptions
            {
                BaseUrl = _config.BaseAddress,
                ClientId = _config.ClientId,
                ClientSecret = _config.ClientSecret,
                Scopes = _config.Scopes,
                AuthorizeEndpoint = _authenticationUrls.AuthorizeEndpoint,
                TokenEndpoint = _authenticationUrls.TokenEndpoint,
                UserInfoEndpoint = _authenticationUrls.UserInfoEndpoint,
                TokenSigningCertificateLoader = GetSigningCertificate(_config.UseCertificate),
                TokenValidationMethod = _config.UseCertificate
                    ? TokenValidationMethod.SigningKey
                    : TokenValidationMethod.BinarySecret
            });

            ConfigurationFactory.Current = _configurationFactory;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
        }

        internal Func<X509Certificate2> GetSigningCertificate(bool useCertificate)
        {
            if (!useCertificate)
            {
                _logger.Info("Not using certificate");
                return null;
            }

            return () =>
            {
                // we need to use CurrentUser as we'll be hosted as an app service
                // https://docs.microsoft.com/en-us/azure/app-service/app-service-web-ssl-cert-load
                var store = new X509Store(StoreLocation.CurrentUser);

                store.Open(OpenFlags.ReadOnly);

                try
                {
                    var thumbprint = CloudConfigurationManager.GetSetting("TokenCertificateThumbprint");
                    var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                    if (certificates.Count < 1)
                        throw new Exception($"Could not find certificate with thumbprint '{thumbprint}' in CurrentUser store");

                    _logger.Info($"Found and using certificate with thumbprint '{thumbprint}' in CurrentUser store");

                    return certificates[0];
                }
                finally
                {
                    store.Close();
                }
            };
        }
    }
}
