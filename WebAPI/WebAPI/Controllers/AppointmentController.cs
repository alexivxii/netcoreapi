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
    public class AppointmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AppointmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //gets all appointments
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                SELECT Date, Hour, Status, Description, Price from dbo.Appointments";
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

        //post appointment
        [HttpPost]
        public JsonResult Post(Appointment Ap)
        {
            string query = @"
                INSERT INTO dbo.Appointments values
                ('" + Ap.DoctorId + @"',
                 '" + Ap.PetId + @"',
                 '" + Ap.Date + @"',
                 '" + Ap.Hour + @"',
                 '" + Ap.Status + @"',
                 '" + Ap.Description + @"',
                 '" + Ap.Price + @"')
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

        [HttpPut]
        public JsonResult Put(Appointment Ap)
        {
            string query = @"
                UPDATE dbo.Appointments set 
                Status = '" + Ap.Status + @"'
                Where AppointmentId = '" + Ap.AppointmentId + @"'
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
            return new JsonResult("Update Succesfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                DELETE FROM dbo.Appointments
                Where AppointmentId = '" + id + @"'
    
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
            return new JsonResult("Deleted Succesfully");
        }

        //get appointment by date
        [Route("bydate/{apdate}")]
        [HttpGet]
        public JsonResult AppointmentByDate(string ApDate)
        {
            string query = @"
                SELECT Appointments.AppointmentId,Date,Hour, Status, ServiceName, DoctorLastName from dbo.Appointments
                JOIN dbo.AppointmentService ON Appointments.AppointmentId = AppointmentService.AppointmentId
                JOIN dbo.Services ON AppointmentService.ServiceId = Services.ServiceId
                JOIN dbo.Doctors ON Appointments.DoctorId = Doctors.DoctorId
                Where Date = '" + ApDate + @"'
                Order by Hour asc
                
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

        //get appointment details
        [Route("details/{id}/{sname}")]
        [HttpGet]
        public JsonResult AppointmentDetails(int id, string sname)
        {
            string query = @"
                SELECT Appointments.AppointmentId, Appointments.Date,Appointments.Hour, Appointments.Status, Appointments.Description, Appointments.Price, Appointments.DoctorId, Appointments. PetId, Pets.PetName, Services.ServiceName, Doctors.DoctorLastName, Pets.OwnerPhoneNumber, Breeds.BreedName, Species.SpeciesName from dbo.Appointments
                JOIN dbo.PETS on Appointments.PetId = Pets.PetId
                JOIN dbo.Breeds ON Pets.BreedId = Breeds.BreedId
                JOIN dbo.Species ON Breeds.SpeciesId = Species.SpeciesId
                JOIN dbo.Doctors ON Appointments.DoctorId = Doctors.DoctorId
                JOIN dbo.AppointmentService ON Appointments.AppointmentId = AppointmentService.AppointmentId
                JOIN dbo.Services ON AppointmentService.ServiceId = Services.ServiceId
                Where Appointments.AppointmentId = '" + id + @"' AND Services.ServiceName = '" + sname + @"'
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

        //get appointment id (dupa crearea unui appointment nou, ii gasesc id ul pentru a-l adauga si in tabela de legatura AppointmentServices)
        [Route("getappid/{doctorid}/{petid}/{date}/{hour}")]
        [HttpGet]
        public JsonResult GetAppointmentId(int doctorid, int petid, string date, string hour)
        {
            string query = @"
                Select AppointmentId from dbo.Appointments
                Where DoctorId = '" + doctorid + @"' and PetId = '" + petid + @"' and Date = '" + date + @"' and Hour = '" + hour + @"'
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
