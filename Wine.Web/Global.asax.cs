﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Wine.Core.DataAccess;
using Wine.Core.Entities;

namespace Wine.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var wineDbContext = new WineDbContext();
            wineDbContext.Database.CreateIfNotExists();
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        var formsAuthenticationTicket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);

                        if (formsAuthenticationTicket != null)
                        {
                            string username = formsAuthenticationTicket.Name;
                            IList<Role> roles;

                            using (WineDbContext dbContext = new WineDbContext())
                            {
                                var user = dbContext.Users.Include("Roles").FirstOrDefault(u => u.Username == username);

                                roles = user?.Roles?.ToList();
                                if (roles != null && roles.Any(x => x.Name == "Administrator"))
                                {
                                    roles = dbContext.Roles.ToList();
                                }
                            }

                            if (roles != null)
                            {
                                HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                                    new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Select(x => x.Name).ToArray());
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw new HttpException((int)HttpStatusCode.Unauthorized, "Unathorized");
                    }
                }
            }
        }
    }
}
