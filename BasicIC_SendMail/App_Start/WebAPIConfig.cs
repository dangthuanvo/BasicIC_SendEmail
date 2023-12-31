﻿using BasicIC_SendEmail.CustomAttributes;
using System.Web.Http;

namespace BasicIC_SendEmail
{
    public static class WebApiConfig
        {
            public static void Register(HttpConfiguration config)
            {
                config.MapHttpAttributeRoutes();

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{action}/{id}",
                    defaults: new { id = RouteParameter.Optional }

                );

                config.Filters.Add(new ValidateModelAttribute());
            }
        }
    }
