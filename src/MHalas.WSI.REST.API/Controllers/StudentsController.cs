using MHalas.WSI.REST.Models;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Web.Http;

namespace MHalas.WSI.Web.Controllers.API
{
    [RoutePrefix("api/students")]
    public class StudentsController : BaseApiController<Student>
    {
        public StudentsController()
            :base("Student")
        {

        }

        [Route()]
        [HttpGet]
        public IHttpActionResult Get(string index = null, string firstName=null, string lastname = null, string birthdateFrom = null, string birthdateTo = null)
        {
            var builder = Builders<Student>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(firstName))
                filter &= builder.Eq(x => x.FirstName, firstName);
            if (!string.IsNullOrEmpty(lastname))
                filter &= builder.Eq(x => x.LastName, lastname);
            if (!string.IsNullOrEmpty(index))
                filter &= builder.Eq(x => x.Index, index);

            var list = GetMethod(filter);

            if(!string.IsNullOrEmpty(birthdateFrom))
            {
                var dateFrom = DateTime.Parse(birthdateFrom);
                list = list.Where(x => DateTime.Parse(x.BirthDate) >= dateFrom);
            }
            if (!string.IsNullOrEmpty(birthdateTo))
            {
                var dateTo = DateTime.Parse(birthdateTo);
                list = list.Where(x => DateTime.Parse(x.BirthDate) <= dateTo);
            }

            if (list.Count() == 0)
                return NotFound();

            return Ok(list);
        }

        [Route("{studentIndex}")]
        [HttpGet]
        public IHttpActionResult Get(string studentIndex)
        {

            var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

            if (student == null)
                return NotFound();

            return Ok(student);
        }
            

        [Route()]
        [HttpPost]
        public IHttpActionResult Post([FromBody]Student student)
        {
            try
            {
                var created = PostMethod(student);
                return Created(string.Format("{0}/{1}", Request.RequestUri, created.Id), created);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{studentIndex}")]
        [HttpPut]
        public IHttpActionResult Put(string studentIndex, [FromBody] Student student)
        {
            var studentToUpdate = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

            var updateDefinition = Builders<Student>.Update
                .Set(x => x.BirthDate, student.BirthDate)
                .Set(x => x.FirstName, student.FirstName)
                .Set(x => x.LastName, student.LastName);

            return PutMethod(studentToUpdate.Id, updateDefinition);
        }

        [Route("{studentIndex}")]
        [HttpDelete]
        public IHttpActionResult Delete(string studentIndex)
        {
            var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();
            return DeleteMethod(student.Id.ToString());
        }
    }
}
