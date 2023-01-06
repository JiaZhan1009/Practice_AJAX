using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeService.Models;
using EmployeeService.DTO;
using Microsoft.AspNetCore.Cors;

namespace EmployeeService.Controllers
{
    [EnableCors("MyAllowOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public EmployeesController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Employees>>> GetEmployees()
        //{
        //    // 一般傳到前端能不要 ToList 或 ToArray 就不要做, 資料量龐大處理效能不好
        //    return await _context.Employees.ToListAsync();
        //}

        public async Task<IEnumerable<EmployeeDTO>> GetEmployees()
        {
            // 一般傳到前端能不要 ToList 或 ToArray 就不要做, 資料量龐大處理效能不好
            return _context.Employees.Select(emp => new EmployeeDTO
            {
                EmployeeId = emp.EmployeeId,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Title = emp.Title
            });

        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployees(int id)
        {
            var employees = await _context.Employees.FindAsync(id);

            if (employees == null)
            {
                return NotFound();
            }

            EmployeeDTO EmpDTO = new EmployeeDTO
            {
                EmployeeId = employees.EmployeeId,
                FirstName = employees.FirstName,
                LastName = employees.LastName,
                Title = employees.Title
            };
            return EmpDTO;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutEmployees(int id, EmployeeDTO EmpDTO)
        {
            if (id != EmpDTO.EmployeeId)
            {
                return "ID不正確!";
            }
            Employees emp = await _context.Employees.FindAsync(EmpDTO.EmployeeId);
            emp.FirstName = EmpDTO.FirstName;
            emp.LastName = EmpDTO.LastName;
            emp.Title = EmpDTO.Title;

            _context.Entry(emp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeesExists(id))
                {
                    return "找不到欲修改的記錄!";
                }
                else
                {
                    throw;
                }
            }

            return "修改成功!";
        }

        [HttpPost("Filter")]  //uri: api/employees/Filter
        public async Task<IEnumerable<EmployeeDTO>> FilterEmployee([FromBody]EmployeeDTO EmpDTO)
        {
            return _context.Employees.Where(emp => emp.FirstName.Contains(EmpDTO.FirstName)).Select(
                    emp => new EmployeeDTO
                    {
                        EmployeeId = emp.EmployeeId,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName,
                        Title = emp.Title
                    }
                );
        }



        // POST: api/Employees // 新增
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<Employees> PostEmployees(EmployeeDTO EmpDTO)
        {
            Employees emp = new Employees
            {
                FirstName = EmpDTO.FirstName,
                LastName = EmpDTO.LastName,
                Title = EmpDTO.Title
            };
            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();

            return emp;
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteEmployees(int id)
        {
            var employees = await _context.Employees.FindAsync(id);
            if (employees == null)
            {
                return "找不到ID " + id + " 的記錄!";
            }

            // 移除找到的記錄
            _context.Employees.Remove(employees);

            // 寫回資料庫
            await _context.SaveChangesAsync();

            return "ID: " + employees.EmployeeId + " 刪除成功!";
        }

        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
