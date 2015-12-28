using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using LinqExpressionsMapper.Samples.DAL;
using LinqExpressionsMapper.Samples.DAL.DataEntities;

namespace LinqExpressionsMapper.Samples.AllExamples
{
    public static class InitInheritanceExample
    {
        public static void ShowCourses(SchoolContext context)
        {
            //Initialization of course base models.
            var basceCourses = context.Courses.Project().To<CourseBaseModel>().ToList();

            //Initialization of course with inherited course model.
            var courses = context.Courses.Project().To<CourseModel>().ToList();

            //Initialization of cross entities inherited model.
            var localizedCourses = context.CourseRes.Project().To<LocalizedCourseModel>().ToList();
        }

        public class CourseBaseModel : ISelectExpression<Course, CourseBaseModel>
        {
            public int CourseId { get; set; }
            public string Name { get; set; }
            Expression<Func<Course, CourseBaseModel>> ISelectExpression<Course, CourseBaseModel>.GetSelectExpression()
            {
                return course => new CourseBaseModel
                {
                    CourseId = course.CourseID,
                    Name = course.Title + "!"
                };
            }
        }

        public class CourseModel : CourseBaseModel, ISelectExpression<Course, CourseModel>
        {
            public int EnrollmentsCount { get; set; }

            Expression<Func<Course, CourseModel>> ISelectExpression<Course, CourseModel>.GetSelectExpression()
            {
                Expression<Func<Course, CourseModel>> select = course => new CourseModel
                {
                    Name = course.Title,
                    EnrollmentsCount = course.Enrollments.Count
                };

                select = select.InheritInit(Mapper.From<Course>().To<CourseBaseModel>().GetExpression());

                return select;
            }
        }

        public class LocalizedCourseModel : CourseModel, ISelectExpression<CourseRes, LocalizedCourseModel>
        {
            public Culture Culture { get; set; }

            Expression<Func<CourseRes, LocalizedCourseModel>> ISelectExpression<CourseRes, LocalizedCourseModel>.GetSelectExpression()
            {
                Expression<Func<CourseRes, LocalizedCourseModel>> select = course => new LocalizedCourseModel
                {
                    Name = course.Title,
                    Culture = course.Culture
                };

                select = select.InheritInit(course => course.Course, Mapper.From<Course>().To<CourseModel>().GetExpression());

                return select;
            }
        }
    }
}
