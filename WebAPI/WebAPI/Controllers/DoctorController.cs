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
    public class DoctorController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DoctorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                SELECT DoctorId, DoctorFirstName, DoctorLastName FROM dbo.Doctors";
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

        [HttpPost]
        public JsonResult Post(Doctor dct)
        {
            string query = @"
                INSERT INTO dbo.Doctors values
                ('" + dct.DoctorFirstName + @"',
                 '" + dct.DoctorLastName + @"'
            )";
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

        //get all appointments for a doctor by id
        [Route("searchAppointment/{Doctorid}")]
        [HttpGet]
        public JsonResult SearchByDoctor(string Doctorid)
        {
            string query = @"
                SELECT Appointments.AppointmentId,Date,Hour, Status,ServiceName,BreedName  from dbo.Appointments
                JOIN dbo.AppointmentService ON Appointments.AppointmentId = AppointmentService.AppointmentId
                JOIN dbo.Services ON AppointmentService.ServiceId = Services.ServiceId
                JOIN dbo.Pets on Appointments.PetId = Pets.PetId
                JOIN dbo.Breeds on Breeds.BreedId = Pets.BreedId
                Where DoctorId = '" + Doctorid + @"'
                Order by Hour, Date
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
        //get all appointments for a doctor by name
        [Route("searchAppointmentTry/{Doctorlast}/{Doctorfirst}")]
        [HttpGet]
        public JsonResult SearchByDoctorTry(string Doctorlast, string Doctorfirst)
        { 
            string query = @"
                SELECT Appointments.AppointmentId,Date,Hour, Status,ServiceName,BreedName  from dbo.Appointments
                JOIN dbo.AppointmentService ON Appointments.AppointmentId = AppointmentService.AppointmentId
                JOIN dbo.Services ON AppointmentService.ServiceId = Services.ServiceId
                JOIN dbo.Pets on Appointments.PetId = Pets.PetId
                JOIN dbo.Breeds on Breeds.BreedId = Pets.BreedId
                JOIN dbo.Doctors on Appointments.DoctorId = Doctors.DoctorId
                Where DoctorFirstName = '" + Doctorfirst + @"'  AND DoctorLastName = '"+ Doctorlast +@"'
                Order by Hour, Date
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

        //get all doctorid from doctor names
        [Route("searchDoctorid/{Doctorlast}/{Doctorfirst}")]
        [HttpGet]
        public JsonResult SearchByDoctorname(string Doctorlast, string Doctorfirst)
        {
            string query = @"
                SELECT DoctorId from dbo.Doctors
                Where DoctorLastName = '" + Doctorlast + @"' and DoctorFirstName = '" + Doctorfirst + @"'
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
