using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int PetId { get; set; }
        public string Date { get; set; }
        public string Hour { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
    }
}
