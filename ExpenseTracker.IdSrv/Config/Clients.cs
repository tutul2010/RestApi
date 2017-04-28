using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ExpenseTracker.IdSrv.Config
{
    public static class Clients
    {
        //what type of client app
        public static IEnumerable<Client> Get()
        {
            return new[]
             {
                new Client 
                {
                     Enabled = true,
                     ClientId = "mvc",
                     ClientName = "ExpenseTracker MVC Client (Hybrid Flow)",
                     Flow = Flows.Hybrid, 
                     RequireConsent = true,  
      
                    // redirect = URI of MVC app
                    RedirectUris = new List<string>
                    {
                        ExpenseTrackerConstants.ExpenseTrackerClient
                    }
                 }
             };

        }
    }
}