using MHalas.WSI.REST.Models;
using MHalas.WSI.REST.Repository.Base;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MHalas.WSI.Web.Controllers.API
{
    [RoutePrefix("api/students/{studentIndex}")]
    public class StudentCoursesController : BaseApiController<Student>
    {
        private BaseMongoRepository<Course> _courseRepository = new BaseMongoRepository<Course>(nameof(Course));

        public StudentCoursesController() 
            : base(nameof(Student))
        {
        }

        [Route("courses")]
        [HttpGet]
        public IHttpActionResult GetStudentCourses(string studentIndex, string name= null, string leadTeacher = null)
        {

            var builder = Builders<Course>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(name))
                filter &= builder.Eq(x => x.Name, name);
            if (!string.IsNullOrEmpty(leadTeacher))
                filter &= builder.Eq(x => x.LeadTeacher, leadTeacher);

            var filteredCourses = _courseRepository.Retrieve(filter);
            var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

            if (student == null)
                return NotFound();

            var studentCourses = student.SignedUpCourses;

            if (studentCourses == null)
                return Ok(new List<Course>());

            var result = filteredCourses.Where(fc => studentCourses.Any(sc => sc.Id == fc.Id));

            return Ok(result);
        }

        [Route("courses")]
        [HttpPost]
        public IHttpActionResult PostStudentCourse(string studentIndex, [FromBody]CourseHelper course)
        {
            try
            {
                var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

                if (student == null)
                    return NotFound();

                if (student.SignedUpCourses == null)
                    student.SignedUpCourses = new List<MongoDBRef>();

                student.SignedUpCourses.Add(new MongoDBRef(nameof(Course), ObjectId.Parse(course.Id)));

                var updateDefinition = Builders<Student>.Update
                    .Set(x => x.SignedUpCourses, student.SignedUpCourses);

                return PutMethod(student.Id, updateDefinition);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("courses/{courseId}")]
        [HttpDelete]
        public IHttpActionResult DeleteStudentCourse(string studentIndex, string courseId)
        {
            try
            {
                var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

                if (student == null)
                    return NotFound();

                student.SignedUpCourses.Remove(new MongoDBRef(nameof(Course), ObjectId.Parse(courseId)));

                var updateDefinition = Builders<Student>.Update
                    .Set(x => x.SignedUpCourses, student.SignedUpCourses);

                return PutMethod(student.Id, updateDefinition);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }

    public class CourseHelper
    {
        public string Id { get; set; }
    }
}
