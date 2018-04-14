using MHalas.WSI.Lab1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace MHalas.WSI.Lab1.Rest.Controllers
{
    public interface IBaseController<Object, IdentityType>
        where Object : IId<IdentityType>, new()
    {
        IEnumerable<Object> Get();
        IHttpActionResult Post([FromBody]Object newObject);
        IHttpActionResult Put(IdentityType ID, [FromBody]Object editedObject);
        Object Delete(IdentityType objectId);
    }
}