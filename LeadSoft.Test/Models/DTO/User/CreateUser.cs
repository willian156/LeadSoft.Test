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


namespace LeadSoft.Test.Models.DTO.User
{
    public class CreateUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public RolesEnum Role { get; set; }
    }
}


