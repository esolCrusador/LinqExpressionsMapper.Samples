using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExpressionsMapper.Samples.Models
{
    public class StudentModel2
    {
        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public IEnumerable<CourseModel2> Courses { get; set; }
    }
}
