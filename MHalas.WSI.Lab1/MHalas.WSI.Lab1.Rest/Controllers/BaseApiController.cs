using MHalas.WSI.Lab1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MHalas.WSI.Lab1.Rest.Controllers
{
    public abstract class BaseApiController<ObjectType, IdentityType> : ApiController
        where ObjectType : IId<IdentityType>, new()
    {
        public abstract List<ObjectType> Items { get; }
        
        protected ObjectType DeleteMethod(IdentityType objectID)
        {
            var objectToRemove = GetObject(objectID);
            if (objectToRemove != null)
                Items.Remove(objectToRemove);

            return Items.SingleOrDefault(x => x.ID.Equals(objectID));
        }

        protected IEnumerable<ObjectType> GetMethod()
        {
            return Items;
        }

        protected IHttpActionResult GetMethod(IdentityType objectID)
        {
            var obj = GetObject(objectID);

            if (obj == null)
                return NotFound();

            return Ok(obj);
        }

        protected IHttpActionResult GetMethod(Func<ObjectType, bool> whereClause)
        {
            var obj = Items.Where(whereClause);

            if (obj.Count() == 0)
                return NotFound();

            return Ok(obj);
        }

        protected IHttpActionResult PostMethod([FromBody] ObjectType newObject)
        {
            Items.Add(newObject);

            var created = GetObject(newObject.ID);

            return Created(string.Format("{0}/{1}", Request.RequestUri, created.ID), created);
        }
        
        protected IHttpActionResult PutMethod(IdentityType objectID, [FromBody] ObjectType editedObject)
        {
            var objectToChange = GetObject(objectID);

            if (objectToChange == null)
                return NotFound();

            Items.Remove(objectToChange);
            Items.Add(editedObject);
            return Ok(editedObject);
        }

        private ObjectType GetObject(IdentityType objectID)
        {
            var searchedObject = Items.SingleOrDefault(x => x.ID.Equals(objectID));

            return searchedObject;
        }
    }
}