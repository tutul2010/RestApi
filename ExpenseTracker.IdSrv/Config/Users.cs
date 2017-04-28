﻿using IdentityServer3.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using IdentityServer3.Core;


namespace ExpenseTracker.IdSrv.Config
{
    public static class Users
    {
        //users with claims
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>() {
           
               new InMemoryUser
            {
                Username = "Kevin",
                Password = "secret",
                Subject = "1",

                Claims = new[]
                {
                    new Claim(Constants.ClaimTypes.GivenName, "Kevin"),
                    new Claim(Constants.ClaimTypes.FamilyName, "Dockx"),
               }
            }
            ,
            new InMemoryUser
            {
                Username = "Sven",
                Password = "secret",
                Subject = "2",

                Claims = new[]
                {
                    new Claim(Constants.ClaimTypes.GivenName, "Sven"),
                    new Claim(Constants.ClaimTypes.FamilyName, "Vercauteren"),
               }
            },
             
            new InMemoryUser
            {
                Username = "Nils",
                Password = "secret",
                Subject = "3",

                Claims = new[]
                {
                    new Claim(Constants.ClaimTypes.GivenName, "Nils"),
                    new Claim(Constants.ClaimTypes.FamilyName, "Missorten"),
               }
            },

            new InMemoryUser
            {
                Username = "Kenneth",
                Password = "secret",
                Subject = "4",

                Claims = new[]
                {
                    new Claim(Constants.ClaimTypes.GivenName, "Kenneth"),
                    new Claim(Constants.ClaimTypes.FamilyName, "Mills"),
               }
            }

           };
        }
    }
}