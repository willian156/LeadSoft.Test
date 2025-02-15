using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeadSoft.Test.Commom.Models;
using LeadSoft.Test.DAO;
using LeadSoft.Test.Models.DTO.User;

namespace LeadSoft.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }



        [HttpGet("GetUsers/{id}")]
        public async Task<ActionResult<List<User>>> GetUsers(int id)
        {
            try
            {
                var verifyRole = _context.Users.Find(id).Role;

                if ((int)verifyRole == 0)
                {

                    var user = await _context.Users.ToListAsync();

                    if (user == null)
                    {
                        return NotFound();
                    }

                    return user;
                }
                else
                {
                    return Unauthorized(new { message = "Você não possui permissão!" });
                }


            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Does the login and returns the Id and Role as coockies
        [HttpPost("Login")]
        public async Task<ActionResult<Cookies>> Login(string username, string password)
        {
            try
            {
                var login = _context.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);

                if (login == null)
                {
                    return Unauthorized(new { message = "Login e senha incorretos!" });
                }
                else
                {
                    var cookies = new Cookies()
                    {
                        Id = login.Result.Id,
                        Role = login.Result.Role
                    };
                    return cookies;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        //Revisar com o front
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(CreateUser user)
        {
            var usr = new User();
            var newUser = usr.CreateUser(user);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            //fazer uma função para isso
            var newUserId = _context.Users.FirstOrDefault<User>(x => x.Username == user.Username).Id;

            return CreatedAtAction("GetUser", new { id = newUserId }, user);
        }

        //Revisar com o front
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
