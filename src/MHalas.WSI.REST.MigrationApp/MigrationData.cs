using MHalas.WSI.REST.Configuration;
using MHalas.WSI.REST.Models;
using MHalas.WSI.REST.Repository.Base;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MHalas.WSI.REST.MigrationApp
{
    public class MigrationData
    {
        private static List<Course> CourseList = new List<Course>()
        {
            new Course() { Id= ObjectId.GenerateNewId(), LeadTeacher = "Teacher A", Name = "C# Course", ECTS = "10"},
            new Course() { Id= ObjectId.GenerateNewId(), LeadTeacher = "Teacher A", Name = "JAVA Course", ECTS = "1"},
            new Course() { Id= ObjectId.GenerateNewId(), LeadTeacher = "Teacher B", Name = "Math", ECTS = "2"},
            new Course() { Id= ObjectId.GenerateNewId(), LeadTeacher = "Teacher B", Name = "Statistics", ECTS = "2"},
            new Course() { Id= ObjectId.GenerateNewId(), LeadTeacher = "Teacher B", Name = "Graphs", ECTS = "4"},
        };

        private static List<Student> StudentList = new List<Student>()
        {
            new Student() { Index="11111", FirstName="AAA", LastName="AAA", BirthDate = new DateTime(2000,01,01).ToString("yyyy-MM-dd")},
            new Student() { Index="11112", FirstName="BBB", LastName="XXX", BirthDate = new DateTime(2000,02,01).ToString("yyyy-MM-dd"),
                SignedUpCourses = new List<MongoDBRef>()
                {
                    new MongoDBRef("Course", CourseList.Where(x=>x.Name=="C# Course").Single().Id),
                    new MongoDBRef("Course", CourseList.Where(x=>x.Name=="Math").Single().Id),
                }},
            new Student() { Index="11113", FirstName="CCC", LastName="YYY", BirthDate = new DateTime(2000,03,01).ToString("yyyy-MM-dd"),
                SignedUpCourses = new List<MongoDBRef>()
                {
                    new MongoDBRef("Course", CourseList.Where(x=>x.Name=="JAVA Course").Single().Id),
                },
                Grades = new List<Grade>()
                {
                    new Grade() {GradeValue = 2, CourseID = new MongoDBRef("Course", CourseList.Where(x=>x.Name=="JAVA Course").Single().Id), AddedDate = DateTime.Now.ToString("yyyy-MM-dd")},
                    new Grade() {GradeValue = 2.5m, CourseID = new MongoDBRef("Course", CourseList.Where(x=>x.Name=="JAVA Course").Single().Id), AddedDate = DateTime.Now.ToString("yyyy-MM-dd")},
                    new Grade() {GradeValue = 3, CourseID = new MongoDBRef("Course", CourseList.Where(x=>x.Name=="JAVA Course").Single().Id), AddedDate = DateTime.Now.ToString("yyyy-MM-dd")},
                    new Grade() {GradeValue = 3.5m, CourseID = new MongoDBRef("Course", CourseList.Where(x=>x.Name=="JAVA Course").Single().Id), AddedDate = DateTime.Now.ToString("yyyy-MM-dd")},
                    new Grade() {GradeValue = 4, CourseID = new MongoDBRef("Course", CourseList.Where(x=>x.Name=="JAVA Course").Single().Id), AddedDate = DateTime.Now.ToString("yyyy-MM-dd")},
                    new Grade() {GradeValue = 4.5m, CourseID = new MongoDBRef("Course", CourseList.Where(x=>x.Name=="JAVA Course").Single().Id), AddedDate = DateTime.Now.ToString("yyyy-MM-dd")},
                    new Grade() {GradeValue = 5, CourseID = new MongoDBRef("Course", CourseList.Where(x=>x.Name=="JAVA Course").Single().Id), AddedDate = DateTime.Now.ToString("yyyy-MM-dd")},
                    new Grade() {GradeValue = 5.5m, CourseID = new MongoDBRef("Course", CourseList.Where(x=>x.Name=="JAVA Course").Single().Id), AddedDate = DateTime.Now.ToString("yyyy-MM-dd")},
                }
            },
            new Student() { Index="11114", FirstName="DDD", LastName="AAA", BirthDate = new DateTime(2000,04,01).ToString("yyyy-MM-dd")},
            new Student() { Index="11115", FirstName="AAA", LastName="AAA", BirthDate = new DateTime(2000,05,01).ToString("yyyy-MM-dd")},
            new Student() { Index="11116", FirstName="EEE", LastName="XXX", BirthDate = new DateTime(2000,06,01).ToString("yyyy-MM-dd")},
        };

        public void Migrate()
        {
            ResetDatabase(DBConfig.ConnectionString, DBConfig.DatabaseName);
            MigrateData(CourseList, "Course");
            MigrateData(StudentList, "Student");
        }

        private void MigrateData<TModel>(List<TModel> list, string collectionName)
            where TModel: IId<ObjectId>
        {
            IBaseRepository<TModel> baseRepo = new BaseMongoRepository<TModel>(collectionName);
            baseRepo.Create(list);
        }
        private void ResetDatabase(string connectionString, string databaseName)
        {
            MongoClient client = new MongoClient(connectionString);
            client.DropDatabase(databaseName);

            client.GetDatabase(databaseName);
        }
    }
}
