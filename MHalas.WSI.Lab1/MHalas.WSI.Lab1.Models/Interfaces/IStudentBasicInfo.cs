using MongoDB.Bson;
using System;

namespace MHalas.WSI.Lab1.Models.Interfaces
{
    public interface IStudentBasicInfo: IId<ObjectId>
    {
        string Index { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime BirthDate { get; set; }
    }
}
