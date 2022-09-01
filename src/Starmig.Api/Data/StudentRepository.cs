using Dapper;
using Starmig.Api.Models;

namespace Starmig.Api.Data
{
    public class StudentRepository: BaseRepository, IStudentRepository
    {
        public StudentRepository(DapperContext context): base(context)
        {
        }

        public async Task<bool> Add(Student student)
        {
            string _sql = @"insert into dbo.Student (FirstName, MiddleName, LastName, EnrollmentDate) 
                                                 values (@FirstName, @MiddleName, @LastName, @EnrollmentDate)";
            return await ExecuteSql(_sql, student);
        }

        public async Task<Student> Get(int id)
        {
            string _sql = "SELECT * FROM Student WHERE Id = @ID";
            return await QueryFirstOrDefault<Student>(_sql, new { id });
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            string _sql = @"Select * from Student";
            return await Query<Student>(_sql, new { });
        }

        public async Task<bool> Update(Student student)
        {
            string _sql = @"update Student
                                    set FirstName = @FirstName,
                                    LastName = @LastName,
                                    EnrollmentDate = @EnrollmentDate,
                                    MiddleName = @MiddleName
                                    where ID = @ID";

            return await ExecuteSql(_sql, new { student.FirstName, student.LastName, student.EnrollmentDate, student.MiddleName, student.ID });
        }

        public async Task<bool> Delete(int id)
        {
            string _sql = @"delete Student
                                    where ID = @id";

            return await ExecuteSql(_sql, new { id });
        }

    }
}
