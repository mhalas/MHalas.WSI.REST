using System.Collections.Generic;
using MHalas.WSI.REST.Models;
using System.Web.Http;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MHalas.WSI.Web.Controllers.API
{
    [RoutePrefix("api/courses")]
    public class CoursesController : BaseApiController<Course>
    {
        public CoursesController()
            :base(nameof(Course))
        {

        }

        [Route("{courseID}")]
        [HttpDelete]
        public IHttpActionResult Delete(string courseID)
            => DeleteMethod(courseID);

        [Route()]
        [HttpGet]
        public IHttpActionResult Get()
        {
            var list = GetMethod();

            if (list.Count() == 0)
                return NotFound();

            return Ok(list);
        }

        [Route("{courseID}")]
        [HttpGet]
        public IHttpActionResult Get(string courseID)
        {
            var course = GetMethod(x=>x.Id == ObjectId.Parse(courseID));

            if (course == null)
                return NotFound();

            return Ok(course);
        }

        [Route()]
        [HttpPost]
        public IHttpActionResult Post([FromBody] Course course)
        {
            var created = PostMethod(course);
            return Created(string.Format("{0}/{1}", Request.RequestUri, created.Id), created);
        }

        [Route("{courseID}")]
        [HttpPut]
        public IHttpActionResult Put(string courseID, [FromBody] Course course)
            => PutMethod(courseID, course);
    }
}
