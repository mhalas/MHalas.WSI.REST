using MHalas.WSI.Lab1.Models;
using MHalas.WSI.Lab1.Repository.Base;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Web.Http;

namespace MHalas.WSI.Lab1.Rest.Controllers
{
    [RoutePrefix("students/{studentIndex}")]
    public class StudentCoursesController : BaseApiController<Student>
    {
        private BaseRepository<Course> _courseRepository = new BaseRepository<Course>(nameof(Course));

        public StudentCoursesController() 
            : base(nameof(Student))
        {
        }

        [Route("courses")]
        [HttpGet]
        public IHttpActionResult GetStudentCourses(string studentIndex)
        {
            var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

            if (student == null)
                return NotFound();

            var courses = _courseRepository.Retrieve(student.SignedUpCourses);

            return Ok(courses);
        }

        [Route("courses")]
        [HttpPost]
        public IHttpActionResult PostStudentCourse(string studentIndex, [FromBody]Course course)
        {
            try
            {
                var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();

                if (student == null)
                    return NotFound();

                student.SignedUpCourses.Add(new MongoDBRef(nameof(Course), course.Id));

                return PutMethod(student.Id, student);
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

                student.SignedUpCourses.Remove(new MongoDBRef(nameof(Course), courseId));

                return PutMethod(student.Id, student);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
