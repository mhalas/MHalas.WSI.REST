using System.Collections.Generic;
using MHalas.WSI.Lab1.Models;
using System.Web.Http;
using System;
using System.Linq;

namespace MHalas.WSI.Lab1.Rest.Controllers
{
    [RoutePrefix("students/{studentID}")]
    public class GradesController : ApiController, IBaseController<Grade>
    {
        private List<Grade> _items = new List<Grade>()
        {
            new Grade() { GradeValue = 4, StudentId = "1234", CourseName="aaaa", AddedDate = new DateTime(2018,01,23) },
        };
        public List<Grade> Items
            => _items;

        public Grade Delete(string keyValue)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Grade> Get()
        {
            throw new NotImplementedException();
        }

        [Route("grades")]
        [HttpGet]
        public IEnumerable<Grade> GetGradesByStudentId(string studentID)
        {
            return Items.Where(x => x.StudentId == studentID);
        }

        [Route("courses/{courseName}/grades")]
        [HttpGet]
        public IEnumerable<Grade> GetGradesByStudentIdAndCourseName(string studentID, string courseName)
        {
            return Items.Where(x => x.StudentId == studentID && x.CourseName == courseName);
        }

        public Grade Post([FromBody] Grade newModel)
        {
            throw new NotImplementedException();
        }

        public Grade Put([FromBody] Grade newModel)
        {
            throw new NotImplementedException();
        }
    }
}
