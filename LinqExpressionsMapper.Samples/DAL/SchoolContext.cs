﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using LinqExpressionsMapper.Samples.DAL.DataEntities;

namespace LinqExpressionsMapper.Samples.DAL
{
    public class SchoolContext : DbContext
    {

        public SchoolContext()
            : base("SchoolContext")
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseRes> CourseRes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
