using MongoDB.Bson;

namespace MHalas.WSI.REST.Models.Interfaces
{
    public interface IStudent: IId<ObjectId>, IStudentBasicInfo, IStudentAdvanceInfo
    {
    }
}
