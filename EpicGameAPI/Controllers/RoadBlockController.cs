using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EpicGameAPI.Data;
using EpicGameAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;

namespace EpicGameAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/roadBlock")]
    public class RoadBlockController : Controller
    {
        private ApplicationDbContext _context;

        public RoadBlockController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        //GET api/roadBlock

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var roadBlocks = await _context.RoadBlock.ToListAsync();
            if(roadBlocks == null)
            {
                return NotFound();
            }

            List<RoadBlock> roadBlockList = new List<RoadBlock>();

            foreach(RoadBlock rb in roadBlocks)
            {
                var storyPaths = from r in _context.RoadBlock
                                where r.Id == rb.Id
                                join rbp in _context.StoryPath on r.Id equals rbp.RoadBlockId
                                select rbp;
                
                rb.StoryPaths = await storyPaths.ToListAsync();
                roadBlockList.Add(rb);
            }
            return Ok(roadBlockList);
        }

        //GET api/roadBlock/{id}
        [HttpGet("{id}")]

        public async Task<IActionResult> Get(int id)
        {
            var roadBlock =  await _context.RoadBlock.SingleOrDefaultAsync(r => r.PreviousOptionId == id);

            if(roadBlock == null)
            {
                return NotFound();
            }

            List<PathOption> pathOptionList = new List<PathOption>();

            var storyPaths = from r in _context.RoadBlock
                             join sp in _context.StoryPath on r.Id equals sp.RoadBlockId
                             where r.Id == roadBlock.Id
                             select sp;
            
            foreach(StoryPath s in storyPaths)
            {                  
                var pathOption = await _context.PathOption.Where(p => p.Id == s.PathOptionId).SingleOrDefaultAsync();
                pathOptionList.Add(pathOption);
            }

            // roadBlock.StoryPaths = await storyPaths.ToListAsync();
            return Ok(roadBlock);

        }
    }
}