using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EpicGameAPI.Models
{
    public class PathOption
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool LeadsToCombat { get; set; }

        public string EnemyType { get; set; }

        public virtual ICollection<StoryPath> StoryPaths { get; set; }


    }
}