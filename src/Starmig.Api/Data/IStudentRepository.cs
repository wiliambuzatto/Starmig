using Starmig.Api.Models;

namespace Starmig.Api.Data
{
    public interface IStudentRepository
    {
        Task<bool> Add(Student student);
        Task<Student> Get(int id);
        Task<IEnumerable<Student>> GetAll();
        Task<bool> Update(Student student);
        Task<bool> Delete(int id);
    }
}
