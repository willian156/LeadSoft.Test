using LeadSoft.Test.Commom.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadSoft.Test.DAO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using LeadSoft.Test.Models.DTO.User;


namespace LeadSoft.Test.Commom.Models
{
    public class User
    {
        
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public RolesEnum Role { get; set; }
    
    
        public User CreateUser(CreateUser createuser)
        {
            var newUser = new User()
            {
                Username = createuser.Username,
                Password = createuser.Password,
                Role = createuser.Role
            };

            return newUser;
        }
    }


}
