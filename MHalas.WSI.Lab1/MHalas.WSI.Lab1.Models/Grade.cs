using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHalas.WSI.Lab1.Models
{
    public class Grade: IId<string>
    {
        public string Identity { get => $"{StudentId},{CourseName},{GradeValue}"; set => Identity = value; }

        public float GradeValue { get; set; }
        public DateTime AddedDate { get; set; }

        public string StudentId { get; set; }
        public string CourseName { get; set; }
    }
}
