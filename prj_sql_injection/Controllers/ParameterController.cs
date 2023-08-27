using Dapper;
using prj_sql_injection.App_Start;
using prj_sql_injection.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace prj_sql_injection.Controllers
{
    public class ParameterController : Controller
    {
        // GET: Parameter

        public readonly ISqlConnectionFactory sqlConnectionFactory;

        public ParameterController(ISqlConnectionFactory sqlconnection)
        {
            sqlConnectionFactory = sqlconnection;
        }


        [HttpPost]
        public JsonResult Login(string userName, string password)
        {
            try
            {
                using (var conn = sqlConnectionFactory.CreateSqlConnection())
                {
                    string sql = $"select * from Employees where LastName = @userName and FirstName = @password";

                    var employees = conn.Query<Employees>(sql, new { userName, password });

                    if (employees.Any())
                    {
                        return Json(new ReturnModel(true, "Login_Success", employees.FirstOrDefault()));
                    }
                    else
                    {
                        return Json(new ReturnModel(false, "Login_Fail"));
                    }
                }
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
                using (var conn = sqlConnectionFactory.CreateSqlConnection())
                {
                    string sql = "select OrderID, CustomerID, ShipName, ShipAddress, Freight from Orders where CustomerID =@keyWord ";
                    var result = conn.Query<Orders>(sql, new { keyWord });
                    return Json(new ReturnModel(true, "Below is your customers", result));
                }
            }
            catch (Exception ex) { return Json(new ReturnModel(false, ex.Message)); }
        }
        [HttpPost]
        public ActionResult CustomerList(int employeeId)
        {
            using (var conn = sqlConnectionFactory.CreateSqlConnection())
            {
                string sql = "select distinct CustomerId from Orders where employeeId = @employeeId";
                var result = conn.Query<Orders>(sql, new { employeeId });
                return View(result.Select(x => x.CustomerID).ToArray());
            }
        }
    }
}