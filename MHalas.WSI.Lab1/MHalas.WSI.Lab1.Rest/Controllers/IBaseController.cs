using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace MHalas.WSI.Lab1.Rest.Controllers
{
    public interface IBaseController<T>
        where T : class, new()
    {
        IEnumerable<T> Get();
        T Post([FromBody]T newModel);
        T Put([FromBody]T newModel);
        T Delete(string keyValue);
    }
}