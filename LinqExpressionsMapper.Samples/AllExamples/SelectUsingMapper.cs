using System.Globalization;
using System.Linq;
using LinqExpressionsMapper.Samples.DAL;
using LinqExpressionsMapper.Samples.DAL.DataEntities;
using LinqExpressionsMapper.Samples.Mappers;
using LinqExpressionsMapper.Samples.Models;

namespace LinqExpressionsMapper.Samples.AllExamples
{
    public static class SelectUsingMapper
    {
        public static void ShowStudents(SchoolContext context)
        {
            Mapper.RegisterAll(new StudentMappers());

            Student newStudent = Mapper.From(new StudentModel2 {StudentId = 1, StudentName = "Boris Alexandrovich Sotsky"}).To<Student>();
            var students = context.Students.Map().To<StudentModel2>().ToList();

            var courses = context.Courses.Map().To<CourseModel2>().SelectWith(Culture.Default).ToList();
        }
    }
}
