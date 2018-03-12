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
    [Route("api/unitClass")]
    public class UnitClassController : Controller
    {
        private ApplicationDbContext _context;

        public UnitClassController(ApplicationDbContext ctx) 
        {
            _context = ctx;
        }

        //GET api/unitClass
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var classes = await _context.UnitClass.ToListAsync();
            
            if(classes==null)
            {
                return NotFound();
            }

            return Ok(classes);
        }

        //GET api/unitClass/{id}

        [HttpGet("{id}", Name="GetSingleUnitClass")]
        public async Task<IActionResult> Get(int id)
        {
            var unitClass = await _context.UnitClass.SingleOrDefaultAsync(u => u.Id == id);

            if (unitClass == null)
            {
                return NotFound();
            }

            return Ok(unitClass);
        }

        //POST api/unitClass

        [HttpPost]

        public async Task<IActionResult> Post(UnitClass unitClass)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UnitClass.Add(unitClass);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                if(UnitClassExists(unitClass.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }


            }
            return CreatedAtRoute("GetSingleUnitClass", new {id = unitClass.Id }, unitClass);
        }

        private bool UnitClassExists(int Id)
        {
            return _context.UnitClass.Any(u => u.Id == Id);
        }

    }
}