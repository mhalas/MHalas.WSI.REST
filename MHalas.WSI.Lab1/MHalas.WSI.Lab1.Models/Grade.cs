using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHalas.WSI.Lab1.Models
{
    public class Grade
    {
        [Key]
        public float GradeValue { get; set; }
        public DateTime AddedDate { get; set; }
        public Student Student { get; set; }
    }
}
