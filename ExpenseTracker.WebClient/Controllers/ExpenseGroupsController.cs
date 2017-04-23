using ExpenseTracker.DTO;
using ExpenseTracker.WebClient.Helpers;
using ExpenseTracker.WebClient.Models;
using Marvin.JsonPatch;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTracker.WebClient.Controllers
{
    public class ExpenseGroupsController : Controller
    {

        public async Task< ActionResult> Index(int? page=1)
        {
            //calling get or post happen as async mode 
            var client = ExpenseTrackerHttpClient.GetClient();

            var model = new ExpenseGroupsViewModel();
            // GET Expense Group Status
            var egsResponse = await client.GetAsync("api/expensegroupstatusses");

            if (egsResponse.IsSuccessStatusCode)
            {
                string egsContent = await egsResponse.Content.ReadAsStringAsync();
                //deserelized the response to get oreginal data
                var lstExpenseGroupStatusses = 
                                              JsonConvert.DeserializeObject<IEnumerable<ExpenseGroupStatus>>(egsContent);
                model.ExpenseGroupStatusses = lstExpenseGroupStatusses;
            }
            else
            {
                return Content("An error occurred.");
            }

            // GET Expense Group 

            HttpResponseMessage response = 
                        await client.GetAsync("api/expensegroups?sort=expensegroupstatusid,title&page="+page+"&pagesize=5"); //done sorting
            if(response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                //paging 
                var pagingInfo = HeaderParser.FindAndParsePagingInfo(response.Headers);
                //deserelized the response and get the data as IEnumerable

                var lstExpenseGroups = 
                            JsonConvert.DeserializeObject<IEnumerable<ExpenseGroup>>(content);
                //then it convert to IPagedList inerface for pageing 
                var pagedExpenseGroupsList = new StaticPagedList<ExpenseGroup>(lstExpenseGroups,
                                                      pagingInfo.CurrentPage,
                                                      pagingInfo.PageSize, pagingInfo.TotalCount);

                model.ExpenseGroups = pagedExpenseGroupsList;
                model.PagingInfo = pagingInfo;

            }
            else
            {
                return Content("An error occured.");
            }
            return View(model);

        }

 
        // GET: ExpenseGroups/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var client = ExpenseTrackerHttpClient.GetClient();
            // wer get required  the id,description,title fo expensegroup and related expenses  for fatching (data association)
            HttpResponseMessage response = await client.GetAsync("api/expensegroups/" + id
            + "?fields=id,description,title,expenses");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<ExpenseGroup>(content);
                return View(model);
            }

            return Content("An error occurred");
        }

        // GET: ExpenseGroups/Create 
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExpenseGroups/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // generate  AntiForgery Token for preventing the CSRF hack(cousin of XSS)
        public async Task<ActionResult> Create(ExpenseGroup expenseGroup)
        {
            try
            {
                var client = ExpenseTrackerHttpClient.GetClient();

                // an expensegroup is created with default status as "Open", for the current user
                expenseGroup.ExpenseGroupStatusId = 1;
                expenseGroup.UserId = @"https://expensetrackeridsrv3/embedded_1";
                //serelized the data for sending 
                var serializedItemToCreate = JsonConvert.SerializeObject(expenseGroup);
                //POST
                var response = await client.PostAsync("api/expensegroups", new StringContent(serializedItemToCreate,
                        System.Text.Encoding.Unicode,   "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("An error occurred.");
                }
            }
            catch
            {
                return Content("An error occurred.");
            }
        }

        // GET: ExpenseGroups/Edit/5 
        public async Task<ActionResult> Edit(int id)
        {
            //fill up the view of existing data
            var client = ExpenseTrackerHttpClient.GetClient();

            HttpResponseMessage response = await client.GetAsync("api/expensegroups/" + id+"?fields=id,title,description"); //filter with 

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                //deserelized the response 
                var model = JsonConvert.DeserializeObject<ExpenseGroup>(content);
                return View(model);
            }

            return Content("An error occurred.");
        }

        // PUT: ExpenseGroups/Edit/5   
        [HttpPost]
        [ValidateAntiForgeryToken] // generate  AntiForgery Token for preventing the CSRF hack(cousin of XSS)
        public  async Task<ActionResult> Edit(int id, ExpenseGroup expenseGroup)
        {
            try
            {
                var client = ExpenseTrackerHttpClient.GetClient();
                //replaceing the value of Title,Description by JsonPatch
                JsonPatchDocument<DTO.ExpenseGroup> patchDoc = new JsonPatchDocument<ExpenseGroup>();
                patchDoc.Replace(eg => eg.Title, expenseGroup.Title);
                patchDoc.Replace(eg => eg.Description, expenseGroup.Description);

                var serializedItemToUpdate = JsonConvert.SerializeObject(patchDoc);
                //partical update for Patch 
                var response = await client.PatchAsync("api/expensegroups/" + id,
                    new StringContent(serializedItemToUpdate, System.Text.Encoding.Unicode, "application/json"));

                // serialize & PUT,,fullupdate
                //var serializedItemToUpdate = JsonConvert.SerializeObject(expenseGroup);

                //var response = await client.PutAsync("api/expensegroups/" + id, new StringContent(serializedItemToUpdate, System.Text.Encoding.Unicode, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("An error occurred");
                }

            }
            catch
            {
                return Content("An error occurred");
            }
        }


        // POST: ExpenseGroups/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var client = ExpenseTrackerHttpClient.GetClient();

                var response = await client.DeleteAsync("api/expensegroups/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("An error occurred");
                }

            }
            catch
            {
                return Content("An error occurred");
            }
        }
    }
}
