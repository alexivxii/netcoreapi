using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Breed
    {
        public int BreedId { get; set; }
        public int SpeciesId { get; set; }
        public string BreedName { get; set; }
    }
}
