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
using Microsoft.AspNetCore.Authorization;

namespace EpicGameAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/character")]
    public class CharacterController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<User> _userMangaer;
        private Task<User> GetCurrentUserAsync() => _userMangaer.GetUserAsync(HttpContext.User);

        public CharacterController(ApplicationDbContext ctx, UserManager<User> usermanager)
        {
            _context = ctx;
            _userMangaer = usermanager;
        }

        //GET api/character
        [HttpGet]
        public IActionResult Get()
        {
            var characters = _context.Character.Include("User").ToList();
            List<Character> characterList = new List<Character>();
            foreach(Character ch in characters)
            {
                var unitClass = from c in _context.Character
                            where c.Id == ch.Id
                            join u in _context.UnitClass on c.UnitClassId equals u.Id
                            select u;
               
                            
                ch.UnitClass = unitClass.Single();
                characterList.Add(ch);
            }

            if(characters == null)
            {
                return NotFound();
            }
            return Ok(characterList);
        }

        //GET api/character/{id}
        [HttpGet("{id}", Name = "GetSingleCharacter")]
        public IActionResult Get(int id)
        {
            var character = _context.Character.Include("User").SingleOrDefault(c => c.Id == id);

            if(character == null)
            {
                return NotFound();
            }
            

           var unitClass = from c in _context.Character
                            where c.Id == character.Id
                            join u in _context.UnitClass on c.UnitClassId equals u.Id
                            select u;
                character.UnitClass = unitClass.Single();
            

            return Ok(character);
        }


        //POST api/character
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]Character character)
        {
            User user = await _context.User.Where(u => u.UserName == User.Identity.Name).SingleOrDefaultAsync();

            ModelState.Remove("User");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            character.User = user;

            _context.Character.Add(character);

            try
            {
               await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                if(CharacterExists(character.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtRoute("GetSingleCharacter", new {id = character.Id }, character);
        }

        private bool CharacterExists(int Id)
        {
            return _context.Character.Any(c => c.Id == Id);
        }

        //PATCH api/character/{id}
        //Use this to update the HP of the user's character after combat
        [HttpPatch("{characterId}", Name="UpdateCharacterHP")]
        [Authorize]
        public async Task<IActionResult> UpdateHP(int characterId, [FromBody]CharacterHPUpdate hPUpdate)
        {
            var character = await _context.Character.SingleOrDefaultAsync(c => c.Id == characterId);

            if(character == null)
            {
                return NotFound();
            }

            character.HP = hPUpdate.HP;

            _context.Character.Update(character);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException e)
            {
                throw e;
            }

            return CreatedAtRoute("GetSingleCharacter", new{id = character.Id}, character);
        }

    }
}