using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test_Task_CRM_lite.Models;

namespace Test_Task_CRM_lite.Controllers
{
    public class LoginController : Controller
    {
        public List<Profile> listProfile = new List<Profile>();
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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            using (SqliteConnection connection = new SqliteConnection(GetProjectConnectionString()))
            {
                connection.Open();
                //Вставить запрос на авторизацию:
                /*
                    SELECT *
                    FROM Profile
                    WHERE Profile.Login = @login 
                    AND Profile.Password = @password
                */
                String SQLquerry = @"SELECT *
                                    FROM Profile
                                    WHERE Profile.Login = $login
                                    AND Profile.Password = $password";
                //SqliteParameter loginParam = new SqliteParameter("$login", login);
                //SqliteParameter passwordParam = new SqliteParameter("$password", password);
                using (SqliteCommand command = new SqliteCommand(SQLquerry, connection))
                {
                    command.Parameters.AddWithValue("$login", login);
                    command.Parameters.AddWithValue("$password", password);
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Profile profile = new Profile();
                            profile.id = reader.GetInt32(0);
                            profile.login = reader.GetString(1);
                            profile.password = reader.GetString(2);
                            profile.id_role = reader.GetInt32(3);

                            listProfile.Add(profile);
                        }
                    }
                    if (listProfile.Count == 0)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
        }

        public IActionResult Registration()
        {
            using (SqliteConnection connection = new SqliteConnection(GetProjectConnectionString()))
            {
                connection.Open();
                List<Role> listRole = new List<Role>();
                string SQLquerry_roles = "SELECT * FROM Role";
                using (SqliteCommand command = new SqliteCommand(SQLquerry_roles, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Role role = new Role();
                            role.id = reader.GetInt32(0);
                            role.name = reader.GetString(1);
                            listRole.Add(role);
                        }
                        SelectList selectListRole = new SelectList(listRole, "id", "name");
                        ViewBag.listRole = selectListRole;
                    }
                }
            }
            return View();
        }
    }
}
