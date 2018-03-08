using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EpicGameAPI.Models
{
    public class Character
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public User User { get; set; }

        public int UnitClassId { get; set;}
        public UnitClass UnitClass { get; set; }

        public string ProfileImgUrl { get; set; }

        [Required]
        public int HP { get; set; }

    }
}
