﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHalas.WSI.Lab1.Models
{
    public class StudentCourse : IId<int>
    {
        public int ID { get; set; }

        public int StudentID { get; set; }
        public int CourseID { get; set; }
    }
}
