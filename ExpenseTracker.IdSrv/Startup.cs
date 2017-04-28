using ExpenseTracker.IdSrv.Config;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;


[assembly: OwinStartup(typeof(ExpenseTracker.IdSrv.Startup))]

namespace ExpenseTracker.IdSrv
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //config identityServer
            app.Map("/identity", idsrvApp =>
            {
                idsrvApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Embedded IdentityServer",
                    IssuerUri = ExpenseTrackerConstants.IdSrvIssuerUri,
                    SigningCertificate = LoadCertificate(),

                    //Factory = InMemoryFactory.Create(
                    //                        users: Users.Get(),
                    //                        clients: Clients.Get(),
                    //                        scopes: Scopes.Get())
                    Factory = new IdentityServerServiceFactory()
                            .UseInMemoryClients(Clients.Get())
                            .UseInMemoryScopes(Scopes.Get())
                            .UseInMemoryUsers(Users.Get())


                });

                /*
                var idServerServiceFactory = new IdentityServerServiceFactory()
                               .UseInMemoryClients(Clients.Get())
                               .UseInMemoryScopes(Scopes.Get())
                               .UseInMemoryUsers(Users.Get());

                var options = new IdentityServerOptions
                {
                    
                    SiteName = "Embedded IdentityServer",
                    IssuerUri = ExpenseTrackerConstants.IdSrvIssuerUri,
                    // PublicOrigin = TripGallery.Constants.TripGallerySTSOrigin,
                    SigningCertificate = LoadCertificate(),
                    Factory = idServerServiceFactory

                };
                idsrvApp.UseIdentityServer(options);
                */

            });
        }
        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\bin\idsrv3test.pfx",
                AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        }

    }
}