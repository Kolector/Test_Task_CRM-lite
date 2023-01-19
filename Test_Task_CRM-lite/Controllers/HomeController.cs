using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Test_Task_CRM_lite.Database;
using Test_Task_CRM_lite.Models;
using Test_Task_CRM_lite;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Test_Task_CRM_lite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public List<Tasks> listTasks = new List<Tasks>();
        public string GetProjectConnectionString()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            string connectionString = configuration.GetConnectionString("CRMContext");

            return connectionString;
        }
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            using (SqliteConnection connection = new SqliteConnection(GetProjectConnectionString()))
            {
                connection.Open();
                //7 columns:
                //id, Customer_profile_name, Implementer_profile_name, name, description, status, deadline
                String SQLquerry = @"SELECT Task.id,
                    Profile_Customer.Login as Customer_profile_name,
                    Profile_Implementer.Login as Implementer_profile_name,
                    Task.name, Task.description, Task_status.name as status,
                    Task.date_deadline as deadline FROM
                    Task JOIN Task_status on Task.id_status = Task_status.id
                    JOIN Customer on Task.id_customer = Customer.id
                    JOIN Implementer on Task.id_implementer = Implementer.id
                    JOIN Profile Profile_Customer on Customer.id_profile = Profile_Customer.id
                    JOIN Profile Profile_Implementer on Implementer.id_profile = Profile_Implementer.id";
                using (SqliteCommand command = new SqliteCommand(SQLquerry, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Tasks task = new Tasks();
                            task.id = reader.GetInt32(0);
                            task.customer_name = reader.GetString(1);
                            task.implementer_name = reader.GetString(2);
                            task.name = reader.GetString(3);
                            task.description = reader.GetString(4);
                            task.status = reader.GetString(5);
                            task.deadline = reader.GetString(6);

                            listTasks.Add(task);
                        }
                    }
                }
            }
            return View(listTasks);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
