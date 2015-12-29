using System;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using LinqExpressionsMapper.Samples.DAL;
using LinqExpressionsMapper.Samples.DAL.DataEntities;

namespace LinqExpressionsMapper.Samples.AllExamples
{
    public static class SelectNonCache
    {
        public static void ShowStudents(SchoolContext context)
        {
            Mapper.Register(new StudentModelMapper());
            var students = context.Students.Project().To<StudentModel>().Querable.ToList();
        }

        public class StudentModel
        {
            public int StudentId { get; set; }
            public string StudentName { get; set; }
            public int? MinutesAfterEnrollment { get; set; }
        }

        public class StudentModelMapper : ISelectDynamicExpression<Student, StudentModel>
        {

            /// <summary>
            /// We need to use ISelectDynamicExpression because DateTime.Now is changed and expression will give different results, so it requires to be rebuild every time.
            /// </summary>
            public Expression<Func<Student, StudentModel>> GetSelectExpression()
            {
                DateTime now = DateTime.Now;

                return student => new StudentModel
                {
                    StudentId = student.ID,
                    StudentName = student.FirstMidName + " " + student.LastName,
                    MinutesAfterEnrollment = SqlFunctions.DateDiff("minute", student.EnrollmentDate, now)
                };
            }
        }
    }
}
