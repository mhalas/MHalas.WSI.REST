﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHalas.WSI.Lab1.Models
{
    public class Course: IId<string>
    {
        public string Identity { get; set; }
        public string LeadTeacher { get; set; }

        public string StudentId { get; set; }
    }
}
