using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EpicGameAPI.Models
{
    public class UnitClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AbilityOneName { get; set; }
        [Required]
        public string AbilityOneDescription { get; set; }
        [Required]
        public int AbilityOneDamage { get; set; }

        [Required]
        public string AbilityTwoName { get; set; }
        [Required]
        public string AbilityTwoDescription { get; set; }
        [Required]
        public int AbilityTwoDamage { get; set; }



        public ICollection<Character> Characters { get; set; }

        public ICollection<Enemy> Enemies { get; set; }


    }
}