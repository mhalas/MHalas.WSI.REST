using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHalas.WSI.Lab1.Models
{
    public interface IId<T>
    {
        [Key]
        T Id { get; set; }
    }
}
