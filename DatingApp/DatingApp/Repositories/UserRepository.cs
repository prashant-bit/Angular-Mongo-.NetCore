using DatingApp.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoClient _mongoClient;


        public UserRepository(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            _users = mongoClient.GetDatabase("DatingDb").GetCollection<User>("Users");

        }
        public async Task<bool> UserExists(string username) {
            var Filter = Builders<User>.Filter.Eq(u => u.UserName, username.ToLower());
            var found = await _users.Find<User>(Filter).FirstOrDefaultAsync();
            if (found == null) return false;
            return true;
            }
        public void CreateAsync(User user)
        {
            _users.InsertOneAsync(user);
        }

        public async Task<User> GetUser(string username)
        {
            var Filter = Builders<User>.Filter.Eq(u => u.UserName, username.ToLower());
            var result =  _users.Find<User>(Filter).ToList();
            return result.FirstOrDefault();
        }
    }
}
