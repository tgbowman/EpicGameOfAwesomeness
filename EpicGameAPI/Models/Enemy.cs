using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EpicGameAPI.Models
{
    public class Enemy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        
        public int UnitClassId { get; set; }
        public UnitClass UnitClass { get; set; }

        [Required]
        public int HP { get; set; }

        [Required]
        public bool Boss { get; set; }

    }
}
