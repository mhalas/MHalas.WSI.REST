using MongoDB.Driver;
using System.Collections.Generic;

namespace MHalas.WSI.REST.Models.Interfaces
{
    public interface IStudentAdvanceInfo
    {
        List<Grade> Grades { get; set; }
        List<MongoDBRef> SignedUpCourses { get; set; }
    }
}
