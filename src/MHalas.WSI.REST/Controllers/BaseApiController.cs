using MHalas.WSI.REST.Models;
using MHalas.WSI.REST.Repository.Base;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace MHalas.WSI.REST.Controllers
{
    public abstract class BaseApiController<ObjectType> : ApiController
        where ObjectType: IId<ObjectId>
    {
        protected IBaseRepository<ObjectType> Repository { get; }

        public BaseApiController(string collectionName)
        {
            Repository = new BaseMongoRepository<ObjectType>(collectionName);
        }

        protected IHttpActionResult DeleteMethod(string objectID)
        {
            var result = Repository.Delete(x => x.Id.Equals(ObjectId.Parse(objectID)));

            if (result.DeletedCount == 1)
                return Ok();

            return NotFound();
        }

        protected IEnumerable<ObjectType> GetMethod()
        {
            return Repository.Retrieve();
        }
        protected IEnumerable<ObjectType> GetMethod(params MongoDBRef[] dbRefsParams)
        {
            return Repository.Retrieve(dbRefsParams.ToList());
        }
        protected IEnumerable<ObjectType> GetMethod(Expression<Func<ObjectType, bool>> whereClause)
        {
            return Repository.Retrieve(whereClause);
        }
        protected IEnumerable<ObjectType> GetMethod(FilterDefinition<ObjectType> filter)
        {
            return Repository.Retrieve(filter);
        }

        protected IHttpActionResult PostMethod([FromBody] ObjectType newObject)
        {
            Repository.Create(newObject);
            var created = GetObject(newObject.Id);

            return Created(string.Format("{0}/{1}", Request.RequestUri, created.Id), created);
        }

        protected IHttpActionResult PutMethod(string objectID, [FromBody] ObjectType editedObject)
        {
            return PutMethod(ObjectId.Parse(objectID), editedObject);
        }
        protected IHttpActionResult PutMethod(ObjectId objectID, [FromBody] ObjectType editedObject)
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