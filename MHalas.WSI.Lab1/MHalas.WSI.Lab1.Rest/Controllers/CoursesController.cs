using System.Collections.Generic;
using MHalas.WSI.Lab1.Models;
using System.Web.Http;
using System.Linq;

namespace MHalas.WSI.Lab1.Rest.Controllers
{
    [RoutePrefix("students/{studentID}/courses")]
    public class CoursesController : ApiController, IBaseController<Course>
    {
        private List<Course> _list = new List<Course>()
        {
            new Course() {Name="aaaa", LeadTeacher="Tester", StudentId="1234"},
            new Course() {Name="aaaa", LeadTeacher="Tester", StudentId="3456"},
            new Course() {Name="bbbb", LeadTeacher="Maciej", StudentId="1234"},
            new Course() {Name="bbbb", LeadTeacher="Maciej", StudentId="2345"},
        };
        public List<Course> Items
            => _list;

        public Course Delete(string keyValue)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Course> Get()
        {
            throw new System.NotImplementedException();
        }

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
            return Items.Where(x => x.StudentId == studentId && x.Name == courseName);
        }

        public Course Post([FromBody] Course newModel)
        {
            throw new System.NotImplementedException();
        }

        public Course Put([FromBody] Course newModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
