using MHalas.WSI.Lab1.Models;
using MHalas.WSI.Lab1.Repository.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MHalas.WSI.Lab1.Test.Repository.Base
{
    [TestClass]
    public class StudentRepository
    {
        BaseRepository<Student> _studentRepository = new BaseRepository<Student>("Student");
        BaseRepository<Course> _courseRepository = new BaseRepository<Course>("Course");


        [TestMethod]
        public void StartSession_Success()
        {
            var isConnected = _studentRepository.StartSession();
            Assert.IsTrue(isConnected);
        }

        [TestMethod]
        public void GetSomeData_Success()
        {
            var items = _studentRepository.Retrieve();
        }

        [TestMethod]
        public void GetConcreteData_Success()
        {
            var guid = Guid.NewGuid();
            var dateTime = DateTime.Now;

            var newStudent = new Student()
            {
                Index = DateTime.Now.Ticks.ToString(),
                FirstName = $"Test_{guid}",
                LastName = "Test",
                BirthDate = dateTime,
            };

            _studentRepository.Create(newStudent);

            var retrievedObject = _studentRepository.Retrieve(x => x.FirstName == $"Test_{guid}").ToList().SingleOrDefault();

            Assert.AreEqual($"Test_{guid}", retrievedObject.FirstName);
            Assert.AreEqual($"Test", retrievedObject.LastName);
            Assert.AreEqual(dateTime.ToUniversalTime().ToString(), retrievedObject.BirthDate.ToString());
        }
        
        [TestMethod]
        public void InsertSomeData_Success()
        {
            var newStudent = new Student()
            {
                Index = DateTime.Now.Ticks.ToString(),
                FirstName = $"Test_{Guid.NewGuid()}",
                LastName = "Test",
                BirthDate = new DateTime(1992, 05, 19),
            };

            _studentRepository.Create(newStudent);
        }

        [TestMethod]
        public void UpdateObject_Success()
        {
            var guid = Guid.NewGuid();
            var dateTime = DateTime.Now;

            var newStudent = new Student()
            {
                Index = DateTime.Now.Ticks.ToString(),
                FirstName = $"Test_{guid}",
                LastName = "Test",
                BirthDate = dateTime,
            };

            _studentRepository.Create(newStudent);

            var addedObject = _studentRepository.Retrieve(x => x.FirstName == $"Test_{guid}").SingleOrDefault();
            addedObject.LastName = "Updated Surname";

            var result = _studentRepository.Update(x => x.FirstName == $"Test_{guid}", addedObject);

            Assert.AreEqual(1, result.ModifiedCount);

            var updatedObject = _studentRepository.Retrieve(x => x.FirstName == $"Test_{guid}").SingleOrDefault();

            Assert.AreEqual("Updated Surname", updatedObject.LastName);
        }

        [TestMethod]
        public void DeleteObject_Success()
        {
            var guid = Guid.NewGuid();
            var dateTime = DateTime.Now;

            var newStudent = new Student()
            {
                Index = DateTime.Now.Ticks.ToString(),
                FirstName = $"Test_{guid}",
                LastName = "Test",
                BirthDate = dateTime,
            };

            _studentRepository.Create(newStudent);

            var result = _studentRepository.Delete(x => x.FirstName == $"Test_{guid}");

            Assert.AreEqual(1, result.DeletedCount);
        }

        [TestMethod]
        public void DeleteObject_Fail()
        {
            var result = _studentRepository.Delete(x => x.FirstName == $"NotCreatedObject");
            Assert.AreEqual(0, result.DeletedCount);
        }

        [TestMethod]
        public void AddNewCourse_Success()
        {
            var guid = Guid.NewGuid();
            var dateTime = DateTime.Now;

            var newStudent = new Student()
            {
                Index = DateTime.Now.Ticks.ToString(),
                FirstName = $"Test_{guid}",
                LastName = "Test",
                BirthDate = dateTime,
            };
            _studentRepository.Create(newStudent);

            var newCourse = new Course()
            {
                LeadTeacher = "Test",
                Name = $"Test_{Guid.NewGuid()}",
                ECTS = "1"
            };
            _courseRepository.Create(newCourse);

            var courseRef = new MongoDBRef("course", newCourse.Id);

            var student = _studentRepository.Retrieve(x => x.Index == newStudent.Index).SingleOrDefault();
            Assert.IsNotNull(student);
            Assert.IsNull(student.SignedUpCourses);

            _studentRepository.FindOneAndUpdate(x => x.SignedUpCourses, newStudent.Id, courseRef);
            student = _studentRepository.Retrieve(x => x.Index == newStudent.Index).SingleOrDefault();
            Assert.IsNotNull(student.SignedUpCourses);
            Assert.AreEqual(1, student.SignedUpCourses.Count);
            Assert.AreEqual(courseRef, student.SignedUpCourses.First());

        }
    }
}
