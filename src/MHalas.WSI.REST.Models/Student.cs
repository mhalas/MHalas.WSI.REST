using MHalas.WSI.REST.Models.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace MHalas.WSI.REST.Models
{
    public class Student: IStudent
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Index { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }

        [BsonIgnoreIfDefault]
        public List<Grade> Grades { get; set; }
        [BsonIgnoreIfDefault]
        public List<MongoDBRef> SignedUpCourses { get; set; }
    }
}
