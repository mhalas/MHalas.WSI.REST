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
        
        protected ObjectType DeleteMethod(IdentityType objectId)
        {
            var objectToRemove = Items.SingleOrDefault(x => x.Identity.Equals(objectId));
            if (objectToRemove != null)
                Items.Remove(objectToRemove);

            return Items.SingleOrDefault(x => x.Identity.Equals(objectId));
        }

        protected IEnumerable<ObjectType> GetMethod()
        {
            return Items;
        }

        protected ObjectType GetMethod(IdentityType objectId)
        {
            return Items.SingleOrDefault(x => x.Identity.Equals(objectId));
        }

        protected ObjectType PostMethod([FromBody] ObjectType newObject)
        {
            Items.Add(newObject);
            return Items.SingleOrDefault(x => x.Identity.Equals(newObject.Identity));
        }

        protected ObjectType PutMethod([FromBody] ObjectType editedObject)
        {
            var objectToChange = Items.SingleOrDefault(x => x.Identity.Equals(editedObject.Identity));
            if (objectToChange != null)
            {
                Items.Remove(objectToChange);
                Items.Add(editedObject);
                return editedObject;
            }
            return default(ObjectType);
        }
    }
}