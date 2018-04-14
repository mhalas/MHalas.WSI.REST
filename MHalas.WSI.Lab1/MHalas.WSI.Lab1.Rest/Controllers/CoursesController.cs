using System.Collections.Generic;
using MHalas.WSI.Lab1.Models;
using System.Web.Http;
using System.Linq;

namespace MHalas.WSI.Lab1.Rest.Controllers
{
    [RoutePrefix("courses")]
    public class CoursesController : BaseApiController<Course, int>, IBaseController<Course, int>
    {
        private List<Course> _items = new List<Course>()
        {
            new Course() {ID=1, Name="A", LeadTeacher="Tester1"},
            new Course() {ID=2, Name="B", LeadTeacher="Tester2"},
        };
        public override List<Course> Items
            => _items;

        [Route("{courseID}")]
        [HttpDelete]
        public Course Delete(int courseID)
            => DeleteMethod(courseID);

        [Route()]
        [HttpGet]
        public IEnumerable<Course> Get()
            => GetMethod();

        [Route("{courseID}")]
        [HttpGet]
        public IHttpActionResult Get(int courseID)
            => GetMethod(courseID);

        [Route()]
        [HttpPost]
        public IHttpActionResult Post([FromBody] Course course)
            => PostMethod(course);

        [Route("{courseID}")]
        [HttpPut]
        public IHttpActionResult Put(int courseID, [FromBody] Course course)
            => PutMethod(courseID, course);
    }
}
