using System;
using LinqExpressionsMapper.Samples.CustomMappers;
using LinqExpressionsMapper.Samples.DAL.DataEntities;

namespace LinqExpressionsMapper.Samples.CustomModels
{
    public class StudentDomainModel: IMapProperties<Student, StudentDomainModel>
    {
        public int StudentId { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public void Map(Student source, StudentDomainModel dest)
        {
            dest.LastName = source.LastName;
            dest.FirstMidName = source.FirstMidName;
            dest.EnrollmentDate = source.EnrollmentDate;
            dest.StudentId = source.ID;
        }
    }
}
