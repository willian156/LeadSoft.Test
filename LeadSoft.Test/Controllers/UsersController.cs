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
using LeadSoft.Test.Commom.Enums;

namespace LeadSoft.Test.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }


        //Get Users only with AdminId
        [HttpGet("GetUsers/{AdminId}")]
        public async Task<ActionResult<List<User>>> GetUsers(int AdminId)
        {
            try
            {
                var verifyRole = _context.Users.Find(AdminId).Role;

                if (verifyRole == RolesEnum.Admin)
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

        //Does the login and returns the Id and Role as cookies
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
                        username = login.Result.Username,
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateUser/{AdminId}")]
        public async Task<IActionResult> PostUser(int AdminId, CreateUser user)
        {
            try
            {
                var roleVerify = (int)_context.Users.FindAsync(AdminId).Result.Role;

                if (roleVerify == 0)
                {

                    var usr = new User();
                    var newUser = usr.CreateUser(user);
                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();


                    var newUserId = _context.Users.FirstOrDefault<User>(x => x.Username == user.Username).Id;

                    return Ok(new { message = $@"Usuário cadastrado com sucesso! Usuário ID: {newUserId}" });
                }

                return Unauthorized(new { message = $@"Você não tem autorização para cadastrar usuários!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Creates First login to manage the system
        [HttpPost("CreateStarterUser")]
        public async Task<IActionResult> PostStarteUser(CreateUser user)
        {
            try
            {

                user.Role = RolesEnum.Admin;
                var usr = new User();
                var newUser = usr.CreateUser(user);
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();


                var newUserId = _context.Users.FirstOrDefault<User>(x => x.Username == user.Username).Id;

                return Ok(new { message = $@"Administrador cadastrado com sucesso! Administrador ID: {newUserId}" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
