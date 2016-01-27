using System;
using System.Linq.Expressions;
using LinqExpressionsMapper.Samples.DAL.DataEntities;

namespace LinqExpressionsMapper.Samples.Models
{
    public class CourseEnrollmentModel : EnrollmentBaseModel, ISelectExpression<Enrollment, CourseEnrollmentModel>
    {
        public StudentModel Student { get; set; }

        Expression<Func<Enrollment, CourseEnrollmentModel>> ISelectExpression<Enrollment, CourseEnrollmentModel>.GetSelectExpression()
        {
            Expression<Func<Enrollment, CourseEnrollmentModel>> select = enrollment => new CourseEnrollmentModel
            {

            };

            select = select.InheritInit(Mapper.From<Enrollment>().To<EnrollmentBaseModel>().GetExpression());
            select = select.AddMemberInit(enrollment => enrollment.Student, enrollment => enrollment.Student, Mapper.From<Student>().To<StudentModel>());

            return select;
        }

        public override string ToString()
        {
            return base.ToString() + " for student>>>\r\n" + Student.ToString();
        }
    }
}
