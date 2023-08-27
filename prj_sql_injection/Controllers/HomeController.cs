using Dapper;
using prj_sql_injection.App_Start;
using prj_sql_injection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace prj_sql_injection.Controllers
{
    public class HomeController : Controller
    {
        public readonly ISqlConnectionFactory sqlConnectionFactory;

        public HomeController(ISqlConnectionFactory sqlConnectionFactory)
        {
            this.sqlConnectionFactory = sqlConnectionFactory;
        }

        [HttpPost]
        public JsonResult Login(string userName, string password)
        {
            try
            {
                string sql =
                    $"select * from Employees where LastName ='{userName}' and FirstName = '{password}' ";

                IEnumerable<Employees> result;
                using (var conn = sqlConnectionFactory.CreateSqlConnection())
                {
                    result = conn.Query<Employees>(sql);
                }
                return Json(new ReturnModel(
                    result.Any(),
                    result.Any() ? "Success" : "Fail",
                    result.FirstOrDefault()?.EmployeeID
                    ));
            }
            catch (Exception)
            {
                return Json(new ReturnModel(false, "Login_Fail"));
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Search(string keyWord)
        {
            try
            {
                IEnumerable<Orders> orders;
                using (var conn = sqlConnectionFactory.CreateSqlConnection())
                {
                    string sql =
                        "select OrderID, CustomerID, ShipName, ShipAddress, Freight " +
                        "from Orders where CustomerID ='" + keyWord + "'";

                    orders = conn.Query<Orders>(sql);

                }
                return Json(new ReturnModel(true, "True", orders));
            }
            catch (Exception ex) { return Json(new ReturnModel(false, ex.Message)); }
        }

        public ActionResult CustomerList(int employeeId)
        {
            string sql = "select distinct CustomerId from Orders where employeeId =" + employeeId;
            IEnumerable<Orders> result;
            using (var conn = sqlConnectionFactory.CreateSqlConnection())
            {
                result = conn.Query<Orders>(sql);
            }
            return View(result.Select(x => x.CustomerID).ToArray());
        }

    }
}