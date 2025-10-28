// W pliku Models/SeaCreature.cs
using System.ComponentModel.DataAnnotations;

namespace SeaCreatureApi.Models
{
    public class SeaCreature
    {
        public int Id { get; set; } // id

        [Required]
        public string Name { get; set; } // назва

        public int Lifespan { get; set; } // скільки років живе

        [Required]
        public string DietType { get; set; } // чим харчується (Carnivore, Herbivore)

        public string Habitat { get; set; } // місце де обітає
    }
}