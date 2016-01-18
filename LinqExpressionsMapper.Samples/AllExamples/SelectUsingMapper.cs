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
            Student newStudent = Mapper.From(new StudentModel2 {StudentId = 1, StudentName = "Boris Alexandrovich Sotsky"}).To<Student>().Using<StudentMappers>();
            var students = context.Students.Project().To<StudentModel2>(c=>c.Using<StudentMappers>()).ToList();

            var courses = context.Courses.Project().To<CourseModel2>(c=>c.Using<StudentMappers, Culture>().WithParam(Culture.Default)).ToList();
        }
    }
}
