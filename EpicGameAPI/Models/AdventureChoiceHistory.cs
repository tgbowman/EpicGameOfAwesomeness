using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EpicGameAPI.Models
{
    public class AdventureChoiceHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AdventureId { get; set; }
        public Adventure Adventure { get; set; }

        [Required]
        public int CharacterId { get; set; }
        public Character Character { get; set; }

        [Required]
        public int PathOptionId { get; set; }
        public PathOption PathOption { get; set; }

    }
}
