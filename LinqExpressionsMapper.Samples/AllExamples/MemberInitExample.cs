using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqExpressionsMapper.Samples.DAL;
using LinqExpressionsMapper.Samples.DAL.DataEntities;

namespace LinqExpressionsMapper.Samples.AllExamples
{
    public static class MemberInitExample
    {
        public static void ShowStudents(SchoolContext context)
        {
            // Reuse member init of single member.
            var enrollments = context.Enrollments.Project().To<EnrollmentModel>().ToList();

            // Reurse member init of IEnumerable of members.
            var studentsWithCourses = context.Students.Project().To<StudentWithCoursesModel>().ToList();
        }

        public class EnrollmentModel: ISelectExpression<Enrollment, EnrollmentModel>
        {
            public int EnrollmentId { get; set; }
            public Grade? Grade { get; set; }

            public StudentBaseModel Student { get; set; }

            public CourseBaseModel Course { get; set; }

            public Expression<Func<Enrollment, EnrollmentModel>> GetSelectExpression()
            {
                Expression<Func<Enrollment, EnrollmentModel>> select = enrollment =>
                    new EnrollmentModel
                    {
                        EnrollmentId = enrollment.EnrollmentID,
                        Grade = enrollment.Grade
                    };

                //select = select.AddMemberInit(entollment => entollment.Student, enrollmentModel => enrollmentModel.Student, Mapper.GetExternalExpression<Student, StudentBaseModel>());
                // If StudentBaseModel implements ISelectExpression<Student, StudentBaseModel> than it can be resolved without explicit getting of select expression.
                select = select.ResolveMemberInit(entollment => entollment.Student, enrollmentModel => enrollmentModel.Student);

                //select = select.AddMemberInit(enrollment => enrollment.Course, enrollmentModel => enrollmentModel.Course, Mapper.GetExternalExpression<Course, CourseBaseModel>());
                select = select.ResolveMemberInit(enrollment => enrollment.Course, enrollmentModel => enrollmentModel.Course);

                return select;
            }
        }

        public class StudentBaseModel: ISelectExpression<Student, StudentBaseModel>
        {
            public int StudentId { get; set; }

            public string FullName { get; set; }
            
            Expression<Func<Student, StudentBaseModel>> ISelectExpression<Student, StudentBaseModel>.GetSelectExpression()
            {
                return student => new StudentBaseModel
                {
                    StudentId = student.ID,
                    FullName = student.FirstMidName + " " + student.LastName
                };
            }
        }

        public class CourseBaseModel: ISelectExpression<Course, CourseBaseModel>
        {
            public int CourseId { get; set; }

            public string CourseName { get; set; }

            public int Credits { get; set; }
            public Expression<Func<Course, CourseBaseModel>> GetSelectExpression()
            {
                return course=>new CourseBaseModel
                {
                    CourseId = course.CourseID,
                    CourseName = course.Title,
                    Credits = course.Credits
                };
            }
        }

        public class StudentWithCoursesModel : StudentBaseModel, ISelectExpression<Student, StudentWithCoursesModel>
        {
            public IEnumerable<CourseBaseModel> Courses { get; set; }
            public IEnumerable<CourseBaseModel> PositiveGradedCourses { get; set; }

            public Grade? MaxGade { get; set; }

            Expression<Func<Student, StudentWithCoursesModel>> ISelectExpression<Student, StudentWithCoursesModel>.GetSelectExpression()
            {

    Expression<Func<Student, StudentWithCoursesModel>> select = student =>
        new StudentWithCoursesModel
        {
            MaxGade = student.Enrollments.Max(e => e.Grade),
            Courses = Mapper.From<Course>().To<CourseBaseModel>().GetExpression()
                .InvokeEnumerable(student.Enrollments.Select(er => er.Course)),
        };
    select = select.ApplyExpressions();

    select = select.AddMemberInit(
        s => s.Enrollments.Where(e => e.Grade <= Grade.C).Select(e => e.Course),
        s => s.PositiveGradedCourses,
        Mapper.From<Course>().To<CourseBaseModel>().GetExpression());

    select = select.InheritInit(Mapper.From<Student>().To<StudentBaseModel>().GetExpression());

                // RESULT:
                //select = student => new StudentWithCoursesModel
                //{
                //    StudentId = student.ID,
                //    FullName = student.FirstMidName + " " + student.LastName,
                //    MaxGade = student.Enrollments.Max(e => e.Grade),

                //    Courses = student.Enrollments.Select(er => er.Course)
                //        .Select(course => new CourseBaseModel
                //        {
                //            CourseId = course.CourseID,
                //            CourseName = course.Title,
                //            Credits = course.Credits
                //        }),
                //    PositiveGradedCourses = student.Enrollments.Where(e => e.Grade <= Grade.C)
                //        .Select(e => e.Course)
                //        .Select(course => new CourseBaseModel
                //        {
                //            CourseId = course.CourseID,
                //            CourseName = course.Title,
                //            Credits = course.Credits
                //        })
                //};

                var memberInit = Mapper.From<Course>().To<CourseBaseModel>().GetExpression();
    select = student => new StudentWithCoursesModel
    {
        Courses = student.Enrollments.Select(er => memberInit.Invoke(er.Course))
    };
    select = select.ApplyExpressions();

                return select;
            }
        }
    }
}
