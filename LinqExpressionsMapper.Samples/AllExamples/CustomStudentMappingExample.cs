using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqExpressionsMapper.Samples.CustomModels;
using LinqExpressionsMapper.Samples.DAL;
using LinqExpressionsMapper.Samples.DAL.DataEntities;

namespace LinqExpressionsMapper.Samples.AllExamples
{
    public class CustomStudentMappingExample
    {
        public static void ShowStudents(SchoolContext ctx)
        {
            var studetns = ctx.Students.ToList().MapSelect<Student, StudentDomainModel>().ToList();

            var student2 = ctx.Students.ToList().MapSelect<Student, StudentDomainModel>().ToList();
        }
    }
}
