using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WishListWebApi.Model
{
    public class WishItem : IEquatable<WishItem>
    {
        public string Wish { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SerialNumber { get; set; }
        public bool IsComplete { get; set; }

        public WishItem(string wishInfo, string serialNumber, bool isComplete)
        {
            Wish = wishInfo;
            SerialNumber = serialNumber;
            IsComplete = isComplete;
        }

        public bool Equals(WishItem? other)
        {
            return this.SerialNumber.Equals(other.SerialNumber);
        }
    }
}
