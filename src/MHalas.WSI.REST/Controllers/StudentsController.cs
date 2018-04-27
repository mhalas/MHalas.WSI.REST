using MHalas.WSI.REST.Models;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Web.Http;

namespace MHalas.WSI.REST.Controllers
{
    [RoutePrefix("students")]
    public class StudentsController : BaseApiController<Student>
    {
        public StudentsController()
            :base("Student")
        {

        }

        [Route()]
        [HttpGet]
        public IHttpActionResult Get(string firstName=null, string lastname = null)
        {
            var builder = Builders<Student>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(firstName))
                filter &= builder.Eq(x => x.FirstName, firstName);
            if (!string.IsNullOrEmpty(lastname))
                filter &= builder.Eq(x => x.LastName, lastname);

            var list = GetMethod(filter);

            if (list.Count() == 0)
                return NotFound();

            return Ok(list);
        }

        [Route("{studentIndex}")]
        [HttpGet]
        public IHttpActionResult Get(string studentIndex)
        {

            var list = GetMethod(x => x.Index == studentIndex);

            if (list.Count() == 0)
                return NotFound();

            return Ok(list);
        }
            

        [Route()]
        [HttpPost]
        public IHttpActionResult Post([FromBody]Student student)
        {
            try
            {
                return PostMethod(student);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{studentIndex}")]
        [HttpPut]
        public IHttpActionResult Put(string studentIndex, [FromBody] Student student)
            => PutMethod(studentIndex, student);

        [Route("{studentIndex}")]
        [HttpDelete]
        public IHttpActionResult Delete(string studentIndex)
            => DeleteMethod(studentIndex);
    }
}
