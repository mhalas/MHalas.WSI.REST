using System.Collections.Generic;
using MHalas.WSI.REST.Models;
using System.Web.Http;
using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MHalas.WSI.REST.Controllers
{
    [RoutePrefix("students/{studentIndex}")]
    public class StudentGradesController : BaseApiController<Student>
    {
        public StudentGradesController()
            :base(nameof(Student))
        {

        }

        [Route("grades")]
        [HttpGet]
        public IHttpActionResult GetGradesByStudentId(string studentIndex)
        {
            var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

            if (student == null)
                return NotFound();
            
            return Ok(student.Grades);
        }

        [Route("grades/{gradeID}")]
        [HttpGet]
        public IHttpActionResult Get(string studentIndex, string gradeID)
        {
            var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

            if (student == null)
                return NotFound();

            var grade = student.Grades.Where(x => x.Id == ObjectId.Parse(gradeID)).SingleOrDefault();

            if (grade == null)
                return NotFound();

            return Ok(grade);
        }

        [Route("courses/{courseID}/grades")]
        [HttpGet]
        public IHttpActionResult GetGradesByStudentIdAndCourseID(string studentIndex, string courseID)
        {
            var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

            if (student == null)
                return NotFound();

            var grades = student.Grades.Where(x => x.CourseID.Id == courseID.ToBson());

            if (grades.Count() == 0)
                return NotFound();

            return Ok(grades);
        }

        [Route("courses/{courseID}/grades/{gradeID}")]
        [HttpGet]
        public IHttpActionResult GetGradesByStudentIdAndCourseIDAndGradeID(string studentIndex, string courseID, string gradeID)
        {
            var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

            if (student == null)
                return NotFound();

            var grade = student.Grades.Where(x => x.CourseID.Id == courseID.ToBson() && x.Id == ObjectId.Parse(courseID)).SingleOrDefault();

            if (grade == null)
                return NotFound();

            return Ok(grade);
        }

        [Route("courses/{courseID}/grades")]
        [HttpPost]
        public IHttpActionResult Post(string studentIndex, string courseID, [FromBody] Grade grade)
        {
            var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

            if (student == null)
                return NotFound();

            grade.Id = ObjectId.GenerateNewId();
            grade.CourseID = new MongoDBRef(nameof(Course), ObjectId.Parse(courseID));
            grade.AddedDate = DateTime.Now;

            student.Grades.Add(grade);

            return PostMethod(student);
        }

        [Route("courses/{courseID}/grades/{gradeID}")]
        [HttpPut]
        public IHttpActionResult Put(string studentIndex, string courseID, string gradeID, [FromBody] Grade grade)
        {
            var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

            if (student == null)
                return NotFound();

            grade.Id = ObjectId.Parse(gradeID);

            var oldGrade = student.Grades.Where(x => x.CourseID.Id == courseID.ToBson() && x.Id == ObjectId.Parse(courseID)).SingleOrDefault();
            student.Grades.Remove(oldGrade);
            student.Grades.Add(grade);

            return PutMethod(student.Id.ToString(), student);
        }

        [Route("courses/{courseID}/grades/{gradeID}")]
        [HttpDelete]
        public IHttpActionResult Delete(string gradeID)
            => DeleteMethod(gradeID);
    }
}
