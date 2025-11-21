using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using LW_4_3_5_Daryev_PI231.Enumerations;

namespace LW_4_3_5_Daryev_PI231.Models
{
    public class GamingAsset
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)] 
        public string Id { get; set; }
        public string Name { get; set; } 
        public DeviceType Type { get; set; } 
        public DateTime HourlyRate { get; set; } 
        public Status Status { get; set; } 
    }
}