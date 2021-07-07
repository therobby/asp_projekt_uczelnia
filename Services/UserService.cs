using APS.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APS.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IAspDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public List<User> Get() =>
            _users.Find(user => true).ToList();

        public string Login(string email, string password)
        {
            var user = _users.Find(user => user.Email == email && user.Password == password).ToList();
            if (user.Count > 0)
            {
                return user[0].Id;
            }
            else return null;
        }
        
        public bool LoginExists(string email) =>
             _users.Find(user => user.Email == email).ToList().Count > 0;

        public User Get(string id) =>
            _users.Find(user => user.Id == id).FirstOrDefault();

        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public void Update(string id, User userIn) =>
            _users.ReplaceOne(user => user.Id == id, userIn);

        public void Remove(User userIn) =>
            _users.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id) =>
            _users.DeleteOne(user => user.Id == id);
    }
}

