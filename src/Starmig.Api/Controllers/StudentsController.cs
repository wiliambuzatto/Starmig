using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starmig.Api.Data;
using Starmig.Api.Models;

namespace Starmig.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly IStudentRepository _studentRepository;
        public StudentsController(SchoolContext context,
                                 IStudentRepository studentRepository)
        {
            _context = context;
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _studentRepository.GetAll());
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _studentRepository.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            return Ok(await _studentRepository.Add(student));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Student student)
        {
            return Ok(await _studentRepository.Update(student));
        }

        [HttpDelete("/{id}")]
        public async Task<IActionResult> Exclude(int id)
        {
            return Ok(await _studentRepository.Delete(id));
        }
    }
}
