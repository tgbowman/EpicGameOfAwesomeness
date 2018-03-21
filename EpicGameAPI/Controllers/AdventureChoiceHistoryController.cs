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
using Microsoft.AspNetCore.Authorization;

namespace EpicGameAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/adventureChoice")]
    public class AdventureChoiceHistoryController : Controller
    {
       private ApplicationDbContext _context;
        public AdventureChoiceHistoryController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        //GET api/adventureChoice
        [HttpGet]

        public async Task<IActionResult> Get()
        {
            var choices = await _context.AdventureChoiceHistory.ToListAsync();
            
            if(choices == null)
            {
                return NotFound();
            }

            return Ok(choices);
        }

        //GET api/adventureChoice/{characterId}
        [HttpGet("{characterId}", Name="GetAllCharacterAdventureChoices")]
        public async Task<IActionResult> Get(int characterId)
        {
            var characterChoices = await _context.AdventureChoiceHistory.Where(a => a.CharacterId == characterId).ToListAsync();

            if(characterChoices == null)
            {
                return NotFound();
            }

            return Ok(characterChoices);
        }

        //GET api/advnentureChoice/{characterId}/{adventureId}
        [HttpGet("{characterId}/{adventureId}", Name = "GetCharacterAdventureChoicesPerStory")]
        // [Route("api/adventureChoice/{characterId}/{adventureId}")]
        public async Task<IActionResult> Get(int characterId, int adventureId)
        {
            var characterAdventureChoices = await _context.AdventureChoiceHistory.Where(a => a.CharacterId == characterId && a.AdventureId == adventureId).ToListAsync();

            if(characterAdventureChoices == null)
            {
                return NotFound();
            }
            if(characterAdventureChoices.Count > 0)
            {
            return Ok(characterAdventureChoices[characterAdventureChoices.Count -1]);
            } else {
                return Ok(characterAdventureChoices);
            }
        }

        //POST api/adventureChoice

        [HttpPost]
        // [Authorize]

        public async Task<IActionResult> Post([FromBody]AdventureChoiceHistory ach)
        {
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AdventureChoiceHistory.Add(ach);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                if(AdventureChoiceExists(ach.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else{
                    throw;
                }
            }

            return CreatedAtRoute("GetCharacterAdventureChoicesPerStory", new{characterId = ach.CharacterId, adventureId = ach.AdventureId}, ach);
        }

        private bool AdventureChoiceExists(int id)
        {
            return _context.AdventureChoiceHistory.Any(a => a.Id == id);
        }

        //DELETE delete all adventure Choices for a character on a particular adventure

        [HttpDelete("{characterId}/{adventureId}", Name="DeleteCharacterAdventureData")]
        [Authorize]

        public async Task<IActionResult> Delete(int characterId, int adventureId)
        {
            var characterDataPresent =  await _context.AdventureChoiceHistory.AnyAsync(a => a.AdventureId == adventureId && a.CharacterId == characterId);

            if(characterDataPresent)
            {
                var allCharacterChoices = await _context.AdventureChoiceHistory.Where(a => a.AdventureId == adventureId && a.CharacterId == characterId).ToListAsync();
                allCharacterChoices.ForEach(c => {
                    _context.Remove(c);
                });
                await _context.SaveChangesAsync();
                return Ok();
            }
            else{
                return NotFound();
            }
        }




    }
}