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
        Object Post([FromBody]Object newObject);
        Object Put([FromBody]Object editedObject);
        Object Delete(IdentityType objectId);
    }
}