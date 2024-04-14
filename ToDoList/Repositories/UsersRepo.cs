using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ToDoList.Models;
using BCrypt.Net;

namespace ToDoList.Repositories
{
    public class UsersRepo
    {
        private ToDoListEntities _entities = new ToDoListEntities();
        public User GetUserByEmail(string email)
        {
            return _entities.Users.FirstOrDefault(x=>x.Email==email);
        }

        public bool RegisterUser(User user, ref string message)
        {
            User prevTask = _entities.Users.FirstOrDefault(x => x.Email == user.Email);
            if (prevTask == null)
            {
                user.Password = HashPassword(user.Password);
                _entities.Users.Add(user);
                _entities.SaveChanges();
                message = "User Added Successfully";
                return true;
            }
            message = "You have already registered. Please Try to Login";
            return false;
        }

        public string HashPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}