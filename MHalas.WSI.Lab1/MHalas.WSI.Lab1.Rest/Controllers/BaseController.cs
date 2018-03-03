using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MHalas.WSI.Lab1.Rest.Controllers
{
    public class BaseController<T> : Controller
        where T: class, new()
    {
        List<T> Items { get; set; }

        public BaseController()
        {
            Items = new List<T>();
        }

        public ActionResult Index()
        {
            return View();
        }

        public IEnumerable<T> Get()
            => Items;

        public T Get(string value)
        {
            var keyProperty = GetKeyProperty();
            return Items.Where(x=> keyProperty.GetValue(x).ToString() == value).SingleOrDefault();
        }

        public void Post([FromBody]T newModel)
        {
            Items.Add(newModel);
        }

        public void Put(string value, [FromBody]T newModel)
        {
            Delete(value);
            Post(newModel);
        }

        public void Delete(string value)
        {
            var itemToDelete = Get(value);
            Items.Remove(itemToDelete);
        }

        private PropertyInfo GetKeyProperty()
        => typeof(T).GetProperties().Where(x => Attribute.GetCustomAttribute(x, typeof(KeyAttribute)) != null).SingleOrDefault();
    }
}