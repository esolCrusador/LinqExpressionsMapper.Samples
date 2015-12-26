﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqExpressionsMapper.Samples.DAL.DataEntities;

namespace LinqExpressionsMapper.Samples.Models
{
    public class CourseFullModel: CourseModel, ISelectExpression<Course, CourseFullModel>
    {
        public virtual IEnumerable<CourseEnrollmentModel> Enrollments { get; set; }
        Expression<Func<Course, CourseFullModel>> ISelectExpression<Course, CourseFullModel>.GetSelectExpression()
        {
            Expression<Func<Course, CourseFullModel>> initExpression = course => new CourseFullModel { };
            
            Expression<Func<Course, CourseModel>> baseInit = Mapper.From<Course>().To<CourseModel>();
            initExpression = initExpression.InheritInit(baseInit);

            initExpression = initExpression.AddMemberInit(course => course.Enrollments, courseMode => courseMode.Enrollments, Mapper.From<Enrollment>().To<CourseEnrollmentModel>());

            return initExpression;
        }

        public override string ToString()
        {
            return base.ToString() + " Enrollments:\r\n" + String.Join("\r\n", Enrollments.Select(e => e.ToString()));
        }
    }
}
