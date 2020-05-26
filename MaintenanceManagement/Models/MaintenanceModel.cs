using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MaintenanceManagement.Models
{
    public class MaintenanceModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("Machanic")]
        public string Machanic { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; }

        [BsonElement("Car")]
        public string Car { get; set; }

        [BsonElement("Date")]
        public string Date { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("Price")]
        public int Price { get; set; }
    }
}
