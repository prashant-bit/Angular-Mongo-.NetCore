using DatingApp.DTOs;
using DatingApp.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Repositories
{
    public class UserDisplayRepository
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoClient _mongoClient;


        public UserDisplayRepository(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            _users = mongoClient.GetDatabase("DatingDb").GetCollection<User>("Users");

        }

        public async Task<List<UserDisplayDto>> GetUsersAsync()
        {
            var Projectionfilter = Builders<User>.Projection
                .Include(u => u.UserName);
            var Filter = Builders<User>.Filter.Empty;
            var result = await _users.Aggregate()
                .Match(Filter)
                .Project<UserDisplayDto>(Projectionfilter)
                .ToListAsync();

            return result;

        }
    }
}
