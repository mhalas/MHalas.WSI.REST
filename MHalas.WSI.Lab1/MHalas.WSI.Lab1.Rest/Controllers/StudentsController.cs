using System.Collections.Generic;
using MHalas.WSI.Lab1.Models;
using System;
using System.Web.Http;
using System.Linq;

namespace MHalas.WSI.Lab1.Rest.Controllers
{
    [RoutePrefix("students")]
    public class StudentsController : BaseApiController<Student, int>, IBaseController<Student, int>
    {
        private List<Student> _list = new List<Student>()
        {
            new Student() {ID=1, Name="Maciej", Surname="Halas", BirthDate=new DateTime(1992,05,19) },
            new Student() {ID=2, Name="Test", Surname="1", BirthDate=new DateTime(1992,05,19) },
            new Student() {ID=3, Name="Test", Surname="2", BirthDate=new DateTime(1992,05,19) },
            new Student() {ID=4, Name="Test", Surname="3", BirthDate=new DateTime(1992,05,19) },
        };

        public override List<Student> Items
            => _list;

        [Route()]
        [HttpGet]
        public IEnumerable<Student> Get()
            => GetMethod();

        [Route("{studentID}")]
        [HttpGet]
        public IHttpActionResult Get(int studentID)
            => GetMethod(studentID);

        [Route()]
        [HttpPost]
        public IHttpActionResult Post([FromBody]Student student)
            => PostMethod(student);

        [Route("{studentID}")]
        [HttpPut]
        public IHttpActionResult Put(int studentID, [FromBody] Student student)
            => PutMethod(studentID, student);

        [Route("{studentID}")]
        [HttpDelete]
        public Student Delete(int studentID)
            => DeleteMethod(studentID);
    }
}
