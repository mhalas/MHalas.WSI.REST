using MHalas.WSI.Lab1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MHalas.WSI.Lab1.Rest.Controllers
{
    [RoutePrefix("students/{studentID}/courses")]
    public class StudentCoursesController : BaseApiController<StudentCourse, int>, IBaseController<StudentCourse, int>
    {
        private List<StudentCourse> _items = new List<StudentCourse>()
        {
            new StudentCourse() { ID=1, StudentID = 1, CourseID = 1 },
            new StudentCourse() { ID=2, StudentID = 1, CourseID = 2 },
            new StudentCourse() { ID=3, StudentID = 2, CourseID = 1 },
        };

        public override List<StudentCourse> Items
            => _items;

        [Route()]
        [HttpGet]
        public IEnumerable<StudentCourse> Get()
            => GetMethod();

        [Route()]
        [HttpGet]
        public IEnumerable<StudentCourse> GetCoursesByStudentId(int studentID)
            => GetMethod().Where(x => x.StudentID == studentID);

        [Route("{courseID}")]
        [HttpGet]
        public IEnumerable<StudentCourse> GetCoursesByStudentIdAndCourseName(int studentID, int courseID)
            => GetMethod().Where(x => x.StudentID == studentID && x.ID == courseID);

        [Route()]
        [HttpPost]
        public IHttpActionResult Post([FromBody] StudentCourse newObject)
            => PostMethod(newObject);

        [Route("{studentCourseID}")]
        [HttpPut]
        public IHttpActionResult Put(int studentCourseID, [FromBody] StudentCourse editedObject)
            => PutMethod(studentCourseID, editedObject);

        [Route("{courseID}")]
        [HttpDelete]
        public StudentCourse Delete(int courseID)
            => DeleteMethod(courseID);
    }
}
