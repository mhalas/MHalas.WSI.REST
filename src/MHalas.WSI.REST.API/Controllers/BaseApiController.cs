using MHalas.WSI.REST.Models;
using MHalas.WSI.REST.Repository.Base;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace MHalas.WSI.Web.Controllers.API
{
    public abstract class BaseApiController<ObjectType> : ApiController
        where ObjectType: IId<ObjectId>
    {
        public IBaseRepository<ObjectType> Repository { get; }

        public BaseApiController(string collectionName)
        {
            Repository = new BaseMongoRepository<ObjectType>(collectionName);
        }

        public IHttpActionResult DeleteMethod(string objectID)
        {
            var result = Repository.Delete(x => x.Id.Equals(ObjectId.Parse(objectID)));

            if (result.DeletedCount == 1)
                return Ok();

            return NotFound();
        }

        public IEnumerable<ObjectType> GetMethod()
        {
            return Repository.Retrieve();
        }
        public IEnumerable<ObjectType> GetMethod(params MongoDBRef[] dbRefsParams)
        {
            return Repository.Retrieve(dbRefsParams.ToList());
        }
        public IEnumerable<ObjectType> GetMethod(Expression<Func<ObjectType, bool>> whereClause)
        {
            return Repository.Retrieve(whereClause);
        }
        public IEnumerable<ObjectType> GetMethod(FilterDefinition<ObjectType> filter)
        {
            return Repository.Retrieve(filter);
        }

        public ObjectType PostMethod([FromBody] ObjectType newObject)
        {
            Repository.Create(newObject);
            var created = GetObject(newObject.Id);

            return created;
        }

        public IHttpActionResult PutMethod(string objectID, [FromBody] ObjectType editedObject)
        {
            return PutMethod(ObjectId.Parse(objectID), editedObject);
        }
        public IHttpActionResult PutMethod(ObjectId objectID, [FromBody] ObjectType editedObject)
        {
            var result = Repository.Update(x => x.Id.Equals(objectID), editedObject);

            if (result.ModifiedCount == 0)
                return NotFound();

            return Ok(editedObject);
        }

        private ObjectType GetObject(ObjectId objectID)
        {
            return Repository.Retrieve(x=>x.Id.Equals(objectID)).SingleOrDefault();
        }
    }
}