using System.Collections.Generic;
using MHalas.WSI.Lab1.Models;
using System;
using System.Web.Http;
using System.Linq;

namespace MHalas.WSI.Lab1.Rest.Controllers
{
    [RoutePrefix("students")]
    public class StudentsController : ApiController, IBaseController<Student>
    {
        private List<Student> _list = new List<Student>()
        {
            new Student() {IndexNumber="1234", Name="Maciej", Surname="Halas", BirthDate=new DateTime(1992,05,19) },
            new Student() {IndexNumber="2345", Name="Maciej", Surname="Halas", BirthDate=new DateTime(1992,05,19) },
            new Student() {IndexNumber="3456", Name="Maciej", Surname="Halas", BirthDate=new DateTime(1992,05,19) },
            new Student() {IndexNumber="4567", Name="Maciej", Surname="Halas", BirthDate=new DateTime(1992,05,19) },
        };

        public List<Student> Items
            => _list;

        [Route()]
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return Items;
        }

        [Route("{studentID}")]
        [HttpGet]
        public Student Get(string studentID)
        {
            return Items.SingleOrDefault(x => x.IndexNumber == studentID);
        }

        [Route()]
        [HttpPost]
        public Student Post([FromBody]Student student)
        {
            Items.Add(student);
            return Items.Where(x => x.IndexNumber == student.IndexNumber).SingleOrDefault();
        }

        [Route()]
        [HttpPut]
        public Student Put([FromBody] Student student)
        {
            var studentToChange = Items.SingleOrDefault(x => x.IndexNumber == student.IndexNumber);
            if(studentToChange != null)
            {
                Items.Remove(studentToChange);
                Items.Add(student);
            }
            return null;
        }

        [Route("{studentID}")]
        [HttpDelete]
        public Student Delete(string studentID)
        {
            var studentToRemove = Items.SingleOrDefault(x => x.IndexNumber == studentID);
            if(studentToRemove != null)
                Items.Remove(studentToRemove);

            return Items.SingleOrDefault(x => x.IndexNumber == studentID);
        }
    }
}
