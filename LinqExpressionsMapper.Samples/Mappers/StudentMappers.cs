using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using LinqExpressionsMapper.Samples.DAL.DataEntities;
using LinqExpressionsMapper.Samples.Models;

namespace LinqExpressionsMapper.Samples.Mappers
{
    public class StudentMappers: 
        ISelectExpression<Student, StudentModel2>, 
        ISelectExpression<Course, CourseModel2>, 
        IPropertiesMapper<StudentModel2, Student>, 
        IPropertiesMapper<CourseModel2, Course>,
        ISelectExpression<Course, CourseModel2, Culture>,
        ISelectDynamicExpression<Enrollment, EnrollmentBaseModel>
    {
        public Expression<Func<Student, StudentModel2>> GetSelectExpression()
        {
            Expression<Func<Student, StudentModel2>> select = student => new StudentModel2
            {
                StudentId = student.ID,
                StudentName = student.FirstMidName + " " + student.LastName
            };

            select = select.AddMemberInit(s => s.Enrollments.Select(e => e.Course), s => s.Courses, Mapper.From<Course>().To<CourseModel2>().Using<StudentMappers>());

            return select;
        }

        Expression<Func<Course, CourseModel2>> ISelectExpression<Course, CourseModel2>.GetSelectExpression()
        {
            return course => new CourseModel2
            {
                CourseId = course.CourseID,
                CourseName = course.Title
            };
        }

        public void MapProperties(StudentModel2 source, Student dest)
        {
            dest.FirstMidName = String.Join(" ", source.StudentName.Split(' ').Reverse().Skip(1).Reverse());
            dest.LastName = source.StudentName.Split(' ').Reverse().First();
        }

        public void MapProperties(CourseModel2 source, Course dest)
        {
            dest.Title = source.CourseName;
        }

        public Expression<Func<Course, CourseModel2>> GetSelectExpression(Culture culture)
        {
            return course=>new CourseModel2
            {
                CourseId = course.CourseID,
                CourseName = course.CourseRes.FirstOrDefault(r=>r.Culture== culture).Title
            };
        }

        Expression<Func<Enrollment, EnrollmentBaseModel>> ISelectExpression<Enrollment, EnrollmentBaseModel>.GetSelectExpression()
        {
            return e => new EnrollmentBaseModel
            {
                EnrollmentId = e.EnrollmentID,
                Grade = e.Grade
            };
        }
    }
}
