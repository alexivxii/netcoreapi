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
    public class PetController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PetController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                SELECT PetId, BreedId, PetName, OwnerPhoneNumber FROM dbo.Pets";
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
        public JsonResult Post(Pet pet)
        {
            string query = @"
                INSERT INTO dbo.Pets values
                ('" + pet.BreedId + @"',
                 '" + pet.PetName + @"',
                 '" + pet.OwnerPhoneNumber + @"'
                
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

        [HttpPut]
        public JsonResult Put(Pet pet)
        {
            string query = @"
                UPDATE dbo.Services set 
                BreedId = '" + pet.BreedId + @"',
                PetName = '" + pet.PetName + @"',
                OwnerPhoneNumber = '" + pet.OwnerPhoneNumber + @"'
                Where ServiceId = '" + pet.PetId + @"'
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
                DELETE FROM dbo.Pets
                Where PetId = '" + id + @"'
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

        //get pets for a phone number
        [Route("searchPhone/{phonenumber}")]
        [HttpGet]
        public JsonResult SearchByPhone(string phonenumber)
        {
            string query = @"
                SELECT PetId ,PetName, BreedName from dbo.Pets
                JOIN Breeds on Pets.BreedId = Breeds.BreedId
                Where OwnerPhoneNumber = '" + phonenumber + @"'
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

        //get all appointments for a pet
        [Route("searchAppointment/{PetId}")]
        [HttpGet]
        public JsonResult SearchByPet(int petid)
        {
            string query = @"
                SELECT Appointments.AppointmentId,PetId,Date,Hour, Status, ServiceName from dbo.Appointments
                JOIN dbo.AppointmentService ON Appointments.AppointmentId = AppointmentService.AppointmentId
                JOIN dbo.Services ON AppointmentService.ServiceId = Services.ServiceId
                Where PetId = '" + petid + @"'
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

        //verify if pet exists
        [Route("exists/{petname}/{phone}")]
        [HttpGet]
        public JsonResult VerifyPetExists(string petname, string phone)
        {
            string query = @"
                SELECT COUNT (*) FROM dbo.Pets
                WHERE PetName='" + petname + @"' AND OwnerPhoneNumber='" + phone + @"'
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

        //get pet id
        [Route("getidpet/{petname}/{phonenumber}")]
        [HttpGet]
        public JsonResult GetIdPet(string petname,string phonenumber)
        {
            string query = @"
                SELECT PetId from dbo.Pets
                Where OwnerPhoneNumber = '" + phonenumber + @"' AND PetName = '" + petname + @"'
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

        //get species and breeds
        [Route("getspeciesbreeds")]
        [HttpGet]
        public JsonResult GetSpeciesBreeds()
        {
            string query = @"
                SELECT Species.SpeciesName, Breeds.BreedName from dbo.Species join dbo.Breeds on Species.SpeciesId = Breeds.SpeciesId
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
