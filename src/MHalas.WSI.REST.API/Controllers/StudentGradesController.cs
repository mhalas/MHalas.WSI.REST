﻿using System.Collections.Generic;
using MHalas.WSI.REST.Models;
using System.Web.Http;
using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MHalas.WSI.REST.Repository.Base;

namespace MHalas.WSI.Web.Controllers.API
{
    [RoutePrefix("api/students/{studentIndex}")]
    public class StudentGradesController : BaseApiController<Student>
    {
        private IBaseRepository<Course> _courseRepo;

        public StudentGradesController()
            :base(nameof(Student))
        {
            _courseRepo = new BaseMongoRepository<Course>("course");

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
            ObjectId courseObjectId = ObjectId.Parse(courseID);

            var student = GetMethod(x => x.Index == studentIndex).SingleOrDefault();
            var course = _courseRepo.Retrieve(x => x.Id == courseObjectId).SingleOrDefault();

            if (student == null || course == null)
                return NotFound();

            grade.Id = ObjectId.GenerateNewId();
            grade.AddedDate = DateTime.Now;

            grade.CourseID = new MongoDBRef(nameof(Course), ObjectId.Parse(courseID));
            grade.CourseName = course.Name;

            student.Grades.Add(grade);

            var created = PostMethod(student);
            return Created(string.Format("{0}/{1}", Request.RequestUri, created.Id), created);
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
