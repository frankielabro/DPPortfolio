using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Models;

namespace Portfolio.Services
{
    public interface IAuthRepository
    {
        Task<Admin> Register(Admin admin, string password);
        Task<Admin> Login(string username, string password);
        Task<bool> UserExists(string username);


    }
}
