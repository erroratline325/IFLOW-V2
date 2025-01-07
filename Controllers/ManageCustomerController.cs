using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcAppV2.Models;
using System.Data.SqlClient;
using System.Configuration;
using MvcAppV2.processes;

namespace MvcAppV2.Controllers
{
    public class ManageCustomerController : Controller
    {
        //
        // GET: /ManageCustomer/
        [HttpGet]
        public ActionResult Index()
        {
            DBAccess db = new DBAccess();
            IEnumerable<CustomerUnit> dbListCustomer = db.GetDbManageCustomer();
            //var list = new List<string>();
            var queryable = dbListCustomer.Where(x => x.Customer_Name == "Human Resoources");
            return View(queryable);
        }

      
        


    }
}
