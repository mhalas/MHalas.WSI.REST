using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MHalas.WSI.REST.Models
{
    public class Course: IId<ObjectId>
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }
        public string LeadTeacher { get; set; }
        public string ECTS { get; set; }
    }
}
