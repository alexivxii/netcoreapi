using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //gets all users
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                SELECT UserId, Username, Password FROM dbo.Users";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        //sign up user
        [HttpPost]
        public JsonResult Post(User usr)
        {
            string query = @"
                INSERT INTO dbo.Users values
                ('" + usr.Username + @"',
                '" + usr.Password + @"')
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Succesfully");
        }

        //verify user credentials
        [Route("login")]
        [HttpPost]
        public JsonResult VerifyLogin(User usr)
        {
            string query = @"
                SELECT COUNT (*) FROM dbo.Users 
                WHERE Username='" + usr.Username + @"' AND Password='" + usr.Password + @"'
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        //verify user credentials
        [Route("userexists/{username}")]
        [HttpGet]
        public JsonResult VerifyUserExists(string username)
        {
            string query = @"
                SELECT COUNT (*) FROM dbo.Users 
                WHERE Username='" + username + @"'
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        

        //Breeds cu cele mai multe programari
        [Route("statistics/breedsmostapps")]
        [HttpGet]
        public JsonResult BreedsMostApps()
        {
            string query = @"
                SELECT B1.BreedName, Count(A1.AppointmentId) as Numar from dbo.Breeds B1 JOIN dbo.Pets P1 on P1.BreedId = B1.BreedId JOIN dbo.Appointments A1 on A1.PetId = P1.PetId
                Group by B1.BreedName
                HAVING Count(A1.AppointmentId) = (Select TOP 1 Count(A2.AppointmentId) from dbo.Breeds B2 JOIN dbo.Pets P2 on P2.BreedId = B2.BreedId JOIN dbo.Appointments A2 on A2.PetId = P2.PetId
                Group By B2.BreedName
                Order by Count(A2.AppointmentId) desc)
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        //clientii care au avut pretul la un appointment peste pretul mediu
        [Route("statistics/priceoveraverage")]
        [HttpGet]
        public JsonResult PriceOverAverage()
        {
            string query = @"
                SELECT DISTINCT OwnerPhoneNumber from dbo.Pets JOIN dbo.Appointments on Appointments.PetId = Pets.PetId
                WHERE PRICE > (SELECT AVG(A1.PRICE) from dbo.Appointments A1)
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        //cat la suta din programari din luna x sunt anulate
        [Route("statistics/percentagecanceledformonth/{luna}")]
        [HttpGet]
        public JsonResult PercentageForMonth(int luna)
        {
            string query = @"
                SELECT (100*(SELECT COUNT(Status) from dbo.Appointments 
                where DATEPART(MM,Date) = '" + luna + @"' and STATUS = 'cancelled'))
                /(SELECT COUNT(Status) from dbo.Appointments 
                where DATEPART(MM,Date) = '" + luna + @"') as Procentaj
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        //lunile cu cel mai mare numar de programari din anul curent
        [Route("statistics/monthsmostapps")]
        [HttpGet]
        public JsonResult MonthsMostApps()
        {
            string query = @"
                Select DATEPART(MM,Date) as LUNA, COUNT(DATEPART(MM,Date)) as NUMAR FROM dbo.Appointments
                GROUP BY DATEPART(MM,Date)
                HAVING COUNT(DATEPART(MM,Date)) = (
                SELECT TOP 1 COUNT(DATEPART(MM,A1.Date)) from dbo.Appointments A1 WHERE DATEPART(YYYY,A1.Date) = DATEPART(YYYY,GETDATE()) 
                GROUP BY DATEPART(MM,A1.Date) 
                ORDER BY COUNT(DATEPART(MM,A1.Date)) DESC)
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

    }
}
