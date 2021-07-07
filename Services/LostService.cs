using APS.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APS.Services
{
    public class LostService
    {
        private readonly IMongoCollection<Lost> _losts;

        public LostService(IAspDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _losts = database.GetCollection<Lost>(settings.LostsCollectionName);
        }

        public List<Lost> Get() =>
            _losts.Find(lost => true).ToList();

        public Lost Get(string id) =>
            _losts.Find(lost => lost.Id == id).FirstOrDefault();

        public Lost Create(Lost lost)
        {
            _losts.InsertOne(lost);
            return lost;
        }

        public void Update(string id, Lost lostIn) =>
            _losts.ReplaceOne(lost => lost.Id == id, lostIn);

        public void Remove(Lost lostIn) =>
            _losts.DeleteOne(lost => lost.Id == lostIn.Id);

        public void Remove(string id) =>
            _losts.DeleteOne(lost => lost.Id == id);
    }
}
