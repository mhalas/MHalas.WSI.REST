using MongoDB.Bson;

namespace MHalas.WSI.Lab1.Models.Interfaces
{
    public interface IStudent: IId<ObjectId>, IStudentBasicInfo, IStudentAdvanceInfo
    {
    }
}
