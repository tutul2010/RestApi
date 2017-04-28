﻿using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ExpenseTracker.IdSrv.Config
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            var scopes = new List<Scope>
                {
 
                    // identity scopes

                    StandardScopes.OpenId,
                    StandardScopes.Profile 

                 };

            return scopes;
        }
 
    }
}