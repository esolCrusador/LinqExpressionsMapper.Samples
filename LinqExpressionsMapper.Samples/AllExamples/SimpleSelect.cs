using System;
using System.Linq;
using System.Linq.Expressions;
using LinqExpressionsMapper.Samples.DAL;
using LinqExpressionsMapper.Samples.DAL.DataEntities;

namespace LinqExpressionsMapper.Samples.AllExamples
{
    public static class SimpleSelect
    {
        public static void ShowStudents(SchoolContext context)
        {
            var students = context.Students.Map().To<StudentModel>().ToList();

            Mapper.Register(new CourseModelMapper());
            var courses = context.Courses.Map().To<CourseModel>().ToList();
        }

        public class StudentModel:ISelectExpression<Student, StudentModel>
        {
            public int StudentId { get; set; }

            public string StudentName { get; set; }
            public Expression<Func<Student, StudentModel>> GetSelectExpression()
            {
                return student => new StudentModel
                {
                    StudentId = student.ID,
                    StudentName = student.FirstMidName + " " + student.LastName
                };
            }
        }

        public class CourseModel
        {
            public int CourseId { get; set; }

            public string Title { get; set; }
        }

        public class CourseModelMapper:ISelectExpression<Course, CourseModel>
        {
            public Expression<Func<Course, CourseModel>> GetSelectExpression()
            {
                return course=>new CourseModel
                {
                    CourseId = course.CourseID,
                    Title = course.Title
                };
            }
        }
    }
}
