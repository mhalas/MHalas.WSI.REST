using System.Collections.Generic;
using MHalas.WSI.REST.Models;
using System.Web.Http;
using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MHalas.WSI.REST.Repository.Base;
using System.Text.RegularExpressions;

namespace MHalas.WSI.Web.Controllers.API
{
    [RoutePrefix("api/students/{studentIndex}")]
    public class StudentGradesController : BaseApiController<Student>
    {
        private IBaseRepository<Course> _courseRepo;

        public StudentGradesController()
            :base(nameof(Student))
        {
            _courseRepo = new BaseMongoRepository<Course>(nameof(Course));

        }

        [Route("grades")]
        [HttpGet]
        public IHttpActionResult GetGradesByStudentId(string studentIndex, decimal? gradeFrom = null, decimal? gradeTo = null)
        {
            var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

            if (student == null)
                return NotFound();

            var grades = student.Grades.AsEnumerable();
            if (gradeFrom.HasValue)
                grades = grades.Where(x => x.GradeValue >= gradeFrom);
            if(gradeTo.HasValue)
                grades = grades.Where(x => x.GradeValue <= gradeTo);


            return Ok(student.Grades);
        }

        [Route("grades/{gradeID}")]
        [HttpGet]
        public IHttpActionResult Get(string studentIndex, string gradeID)
        {
            var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

            if (student == null || student.Grades == null)
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

            if (student == null || student.Grades == null)
                return NotFound();

            var grades = student.Grades.Where(x => x.CourseID.Id == ObjectId.Parse(courseID));

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
            var regex = new Regex(@"^[2-5]\,[05]$");
            var gradeMatch = regex.Match(grade.GradeValue.ToString("0.0"));

            if (!gradeMatch.Success)
                throw new FormatException("Grade format is incorrect");

            ObjectId courseObjectId = ObjectId.Parse(courseID);

            var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();
            var course = _courseRepo.Retrieve(x => x.Id == courseObjectId).SingleOrDefault();
            var studentCourse = student.SignedUpCourses.SingleOrDefault(x => x.Id == courseObjectId);

            if (student == null || course == null || studentCourse == null)
                return NotFound();

            grade.Id = ObjectId.GenerateNewId();
            grade.AddedDate = DateTime.Now;

            grade.CourseID = new MongoDBRef(nameof(Course), ObjectId.Parse(courseID));
            grade.CourseName = course.Name;
            grade.GradeValue = grade.GradeValue;

            if (student.Grades == null)
                student.Grades = new List<Grade>();

            student.Grades.Add(grade);
            
            var updateDefinition = Builders<Student>.Update
                .Set(x => x.Grades, student.Grades);

            return PutMethod(student.Id, updateDefinition);
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

            var updateDefinition = Builders<Student>.Update
                .Set(x => x.Grades, student.Grades);

            return PutMethod(student.Id, updateDefinition);
        }

        [Route("courses/{courseID}/grades/{gradeID}")]
        [HttpDelete]
        public IHttpActionResult Delete(string studentIndex, string gradeID)
        {
            try
            {
                var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

                if (student == null)
                    return NotFound();

                var gradeToRemove = student.Grades.SingleOrDefault(x => x.Id == ObjectId.Parse(gradeID));
                student.Grades.Remove(gradeToRemove);

                var updateDefinition = Builders<Student>.Update
                    .Set(x => x.Grades, student.Grades);

                return PutMethod(student.Id, updateDefinition);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
