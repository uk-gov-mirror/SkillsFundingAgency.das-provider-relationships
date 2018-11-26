﻿using System;
using System.Configuration;
using System.Web.Http;
using Microsoft.ApplicationInsights.Extensibility;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Api.DependencyResolution;
using WebApi.StructureMap;

namespace SFA.DAS.ProviderRelationships.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            IoC.Initialize(GlobalConfiguration.Configuration);
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];
        }

        protected void Application_End()
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var logger = GlobalConfiguration.Configuration.DependencyResolver.GetService<ILog>();

            logger.Error(exception, "Application error");
        }
    }
}