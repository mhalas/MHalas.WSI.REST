using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace MHalas.WSI.REST.Models.Interfaces
{
    public interface IStudent: IId<ObjectId>
    {
        string Index { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string BirthDate { get; set; }

        List<Grade> Grades { get; set; }
        List<MongoDBRef> SignedUpCourses { get; set; }

    }
}
