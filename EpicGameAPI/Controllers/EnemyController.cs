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

namespace EpicGameAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/enemy")]
    public class EnemyController : Controller
    {
        private ApplicationDbContext _context;

        public EnemyController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        //GET api/enemy
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var enemies = await _context.Enemy.ToListAsync();

            List<Enemy> EnemyList = new List<Enemy>();
            foreach(Enemy en in enemies)
            {
                var unitClass = from e in _context.Enemy
                            where e.Id == en.Id
                            join u in _context.UnitClass on e.UnitClassId equals u.Id
                            select u;

                en.UnitClass = unitClass.Single();
                EnemyList.Add(en);
            }

            if(enemies == null)
            {
                return NotFound();
            }

            return Ok(enemies);
        }

        //GET api/enemy/{id}
        [HttpGet("{id}", Name="GetSingleEnemy")]

        public async Task<IActionResult> Get(int id)
        {
            var enemy = await _context.Enemy.SingleOrDefaultAsync(e => e.Id == id);
            
            if(enemy == null)
            {
                return NotFound();
            }

            var unitClass = from e in _context.Enemy
            where e.Id == enemy.Id
            join u in _context.UnitClass on e.UnitClassId equals u.Id
            select u;

            enemy.UnitClass = unitClass.Single();


            return Ok(enemy);
        }


    }
}