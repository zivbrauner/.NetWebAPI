using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.Collections;
using System.Xml.Linq;
using WishListWebApi.Model;

namespace WishListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly string _connctionString;
        private readonly string _dBName;
        private readonly string _collectionName;
        public WishListController(IConfiguration configuration) 
        {
            _configuration = configuration;
            _connctionString = _configuration.GetSection("ConnctionString").GetValue<string>("ConnctionString");
            _dBName = _configuration.GetSection("ConnctionString").GetValue<string>("DBName");
            _collectionName = _configuration.GetSection("ConnctionString").GetValue<string>("CollectionName");
        }
        [HttpGet]
        [Route("GetWishes")]
        public JsonResult GetWishes() 
        {
            JsonResult rslt = new JsonResult(null);
            try
            {
                var client = new MongoClient(_connctionString);
                var db = client.GetDatabase(_dBName);
                var collection = db.GetCollection<WishItem>(_collectionName);

                var response = collection.FindAsync<WishItem>(_ => true);
                response.Wait();
                rslt =  new JsonResult(response.Result.ToList());
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }
             return rslt;
            
        }

        [HttpPost]
        [Route("AddWish")]
        public async void AddWish([FromForm]string wishInfo, [FromForm] bool isComplete=false)
        {
            WishItem wish = new WishItem(wishInfo, null, isComplete);
            var client = new MongoClient(_connctionString);
            var db = client.GetDatabase(_dBName);
            var collection = db.GetCollection<WishItem>(_collectionName);

            await collection.InsertOneAsync(wish);
        }


        [HttpDelete()]
        [Route("RemoveWish/{id}")]
        public async void DeleteWish([FromRoute] string id)
        {
            var client = new MongoClient(_connctionString);
            var db = client.GetDatabase(_dBName);
            var collection = db.GetCollection<WishItem>(_collectionName);

            var filter = Builders<WishItem>.Filter
                .Eq(r => r.SerialNumber, id);

            collection.DeleteOne(filter);
        }

        [HttpPut]
        [Route("UpdateWish")]
        public void UpdateWish([FromForm]string wishInfo, [FromForm] string wishId, [FromForm] bool isComplete = false)
        {
            WishItem wish = new WishItem(wishInfo, wishId, isComplete);
            var client = new MongoClient(_connctionString);
            var db = client.GetDatabase(_dBName);
            var collection = db.GetCollection<WishItem>(_collectionName);

            var filter = Builders<WishItem>.Filter
                .Eq(r => r.SerialNumber, wish.SerialNumber);

            collection.ReplaceOne(filter, wish);
        }
    }
}
