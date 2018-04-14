using System.Collections.Generic;
using MHalas.WSI.Lab1.Models;
using System.Web.Http;
using System;
using System.Linq;

namespace MHalas.WSI.Lab1.Rest.Controllers
{
    [RoutePrefix("students/{studentID}")]
    public class GradesController : BaseApiController<Grade, int>, IBaseController<Grade, int>
    {
        private List<Grade> _items = new List<Grade>()
        {
            new Grade() { ID= 1, GradeValue = 4, StudentID = 1, CourseID=1, AddedDate = new DateTime(2018,01,23) },
            new Grade() { ID= 2, GradeValue = 5, StudentID = 1, CourseID=1, AddedDate = new DateTime(2018,01,23) },
            new Grade() { ID= 3, GradeValue = 5, StudentID = 1, CourseID=2, AddedDate = new DateTime(2018,01,23) },
        };
        public override List<Grade> Items
            => _items;

        [Route("~/grades")]
        [HttpGet]
        public IEnumerable<Grade> Get()
            => GetMethod();

        [Route("grades/{gradeID}")]
        [HttpGet]
        public IHttpActionResult Get(int gradeID)
            => GetMethod(x=> x.ID == gradeID);

        [Route("grades")]
        [HttpGet]
        public IHttpActionResult GetGradesByStudentId(int studentID)
            => GetMethod(x => x.StudentID == studentID);

        [Route("courses/{courseID}/grades")]
        [HttpGet]
        public IHttpActionResult GetGradesByStudentIdAndCourseID(int studentID, int courseID)
            => GetMethod(x => x.StudentID == studentID && x.CourseID == courseID);

        [Route("courses/{courseID}/grades/{gradeID}")]
        [HttpGet]
        public IHttpActionResult GetGradesByStudentIdAndCourseIDAndGradeID(int studentID, int courseID, int gradeID)
            => GetMethod(x => x.StudentID == studentID && x.CourseID == courseID && x.ID == gradeID);

        [Route("courses/{courseID}/grades")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] Grade grade)
            => PostMethod(grade);

        [Route("courses/{courseID}/grades/{gradeID}")]
        [HttpPut]
        public IHttpActionResult Put(int gradeID, [FromBody] Grade grade)
            => PutMethod(gradeID, grade);

        [Route("courses/{courseID}/grades/{gradeID}")]
        [HttpDelete]
        public Grade Delete(int gradeID)
            => DeleteMethod(gradeID);
    }
}
