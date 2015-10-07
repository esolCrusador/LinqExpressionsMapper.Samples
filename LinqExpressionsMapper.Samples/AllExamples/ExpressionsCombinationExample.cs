﻿using System;
using System.Linq;
using System.Linq.Expressions;
using LinqExpressionsMapper.Samples.DAL;
using LinqExpressionsMapper.Samples.DAL.DataEntities;
using LinqExpressionsMapper.Samples.Models;

namespace LinqExpressionsMapper.Samples.AllExamples
{
    public class ExpressionsCombinationExample
    {
        public static void CombineSelectExpression(SchoolContext ctx)
        {
            Expression<Func<Course, CourseModel>> courseSelect =
                course => new CourseModel
                {
                    CourseId = course.CourseID,
                    Title = course.Title,
                    Credits = course.Credits
                };
            Expression<Func<Enrollment, EnrollmentBaseModel>> enrollmentSelect =
                enrollment => new EnrollmentBaseModel
                {
                    EnrollmentId = enrollment.EnrollmentID,
                    Grade = enrollment.Grade
                };
            Expression<Func<Student, StudentModel>> studentSelect =
                student => new StudentModel
                {
                    StudentId = student.ID,
                    FirstMidName = student.FirstMidName,
                    LastName = student.LastName
                };


            Expression<Func<Enrollment, CourseEnrollmentModel>> enrollmentWithStudentsSelect =
                enrollment => new CourseEnrollmentModel
                {
                };

            enrollmentWithStudentsSelect = enrollmentWithStudentsSelect.InheritInit(enrollmentSelect);

            enrollmentWithStudentsSelect = enrollmentWithStudentsSelect.AddMemberInit(
                enrollmentEntity => enrollmentEntity.Student,
                enrollmentModel => enrollmentModel.Student,
                studentSelect
                );

            enrollmentWithStudentsSelect = enrollmentWithStudentsSelect.AddMemberInit(
                enrollmentEntity => enrollmentEntity.Grade,
                enrollmentModel => enrollmentModel.GradeString,
                EnumExpressionExtensions.GetEnumToStringExpression<Grade?>(grade => grade.ToString())
                );

            Expression<Func<Course, CourseFullModel>> courseFullSelect =
                course => new CourseFullModel
                {
                };
            courseFullSelect = courseFullSelect.InheritInit(courseSelect);

            courseFullSelect = courseFullSelect.AddMemberInit(
                courseEntity => courseEntity.Enrollments,
                courseModel => courseModel.Enrollments,
                enrollmentWithStudentsSelect
                );

            var courses = ctx.Courses.Select(courseFullSelect).ToList();
        }
    }
}
