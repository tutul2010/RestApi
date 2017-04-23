using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ExpenseTracker.WebClient.Helpers
{
    //helper cls that use for api calling 
    public static class ExpenseTrackerHttpClient
    {
        
        public static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(ExpenseTrackerConstants.ExpenseTrackerAPI); //set the api resource uri
            client.DefaultRequestHeaders.Accept.Clear();
            //accept only json format
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}