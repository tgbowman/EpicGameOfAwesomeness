using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EpicGameAPI.Models
{
    public class RoadBlock
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AdventureId { get; set; }
        public Adventure Adventure { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool StartingPoint { get; set; }

        public int? PreviousOptionId { get; set; }
        public PathOption PathOption { get; set; }

        public bool GameOver { get; set; }

        public virtual ICollection<StoryPath> StoryPaths { get; set; }
    }
}
