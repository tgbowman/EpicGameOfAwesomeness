using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EpicGameAPI.Models
{
    public class StoryPath
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RoadBlockId { get; set; }
        public RoadBlock RoadBlock { get; set; }

        [Required]
        public int PathOptionId { get; set; }
        public PathOption PathOption { get; set; }


    }
}
