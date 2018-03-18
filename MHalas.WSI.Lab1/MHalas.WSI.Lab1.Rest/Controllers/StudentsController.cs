using System.Collections.Generic;
using MHalas.WSI.Lab1.Models;
using System;
using System.Web.Http;
using System.Linq;

namespace MHalas.WSI.Lab1.Rest.Controllers
{
    [RoutePrefix("students")]
    public class StudentsController : BaseApiController<Student, string>, IBaseController<Student, string>
    {
        private List<Student> _list = new List<Student>()
        {
            new Student() {Identity="1234", Name="Maciej", Surname="Halas", BirthDate=new DateTime(1992,05,19) },
            new Student() {Identity="2345", Name="Maciej", Surname="Halas", BirthDate=new DateTime(1992,05,19) },
            new Student() {Identity="3456", Name="Maciej", Surname="Halas", BirthDate=new DateTime(1992,05,19) },
            new Student() {Identity="4567", Name="Maciej", Surname="Halas", BirthDate=new DateTime(1992,05,19) },
        };

        public override List<Student> Items
            => _list;

        [Route()]
        [HttpGet]
        public IEnumerable<Student> Get()
            => GetMethod();

        [Route("{studentID}")]
        [HttpGet]
        public Student Get(string studentID)
            => GetMethod(studentID);

        [Route()]
        [HttpPost]
        public Student Post([FromBody]Student student)
            => PostMethod(student);

        [Route()]
        [HttpPut]
        public Student Put([FromBody] Student student)
            => PutMethod(student);

        [Route("{studentID}")]
        [HttpDelete]
        public Student Delete(string studentID)
            => DeleteMethod(studentID);
    }
}
