using System.Collections.Generic;
using MHalas.WSI.Lab1.Models;
using System.Web.Http;
using System.Linq;

namespace MHalas.WSI.Lab1.Rest.Controllers
{
    [RoutePrefix("students/{studentID}/courses")]
    public class CoursesController : BaseApiController<Course, string>, IBaseController<Course, string>
    {
        private List<Course> _items = new List<Course>()
        {
            new Course() {Identity="aaaa", LeadTeacher="Tester", StudentId="1234"},
            new Course() {Identity="aaaa", LeadTeacher="Tester", StudentId="3456"},
            new Course() {Identity="bbbb", LeadTeacher="Maciej", StudentId="1234"},
            new Course() {Identity="bbbb", LeadTeacher="Maciej", StudentId="2345"},
        };
        public override List<Course> Items
            => _items;

        [Route("{courseName}")]
        [HttpDelete]
        public Course Delete(string id)
            => DeleteMethod(id);

        [Route()]
        [HttpGet]
        public IEnumerable<Course> Get()
            => GetMethod();

        [Route()]
        [HttpGet]
        public IEnumerable<Course> GetCoursesByStudentId(string studentId)
        {
            return Items.Where(x => x.StudentId == studentId);
        }

        [Route("{courseName}")]
        [HttpGet]
        public IEnumerable<Course> GetCoursesByStudentIdAndCourseName(string studentId, string courseName)
        {
            return Items.Where(x => x.StudentId == studentId && x.Identity == courseName);
        }

        [Route()]
        [HttpPost]
        public Course Post([FromBody] Course course)
            => PostMethod(course);

        [Route()]
        [HttpPut]
        public Course Put([FromBody] Course course)
            => PutMethod(course);
    }
}
