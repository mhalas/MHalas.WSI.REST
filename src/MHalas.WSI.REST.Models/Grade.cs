using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;

namespace MHalas.WSI.REST.Models
{
    public class Grade: IId<ObjectId>
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public double GradeValue { get; set; }
        public DateTime AddedDate { get; set; }

        public MongoDBRef CourseID { get; set; }
        public string CourseName { get; set; }

        public Grade()
        {
            if (Id.Equals(ObjectId.Empty))
                Id = ObjectId.GenerateNewId();

        }
    }
}
