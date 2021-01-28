using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Pet
    {
        public int PetId { get; set; }
        public int BreedId { get; set; }
        public string PetName { get; set; }
        public string OwnerPhoneNumber { get; set; }
    }
}
