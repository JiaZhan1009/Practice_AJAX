using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeService.Models;
using System.Reflection;
using EmployeeService.DTO;
using Microsoft.AspNetCore.Cors;

namespace EmployeeService.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]  // REST界面, 每個功能都有 URI
    public class EmployeesController : ControllerBase
    {
        private readonly NorthwindContext _context;
            
        public EmployeesController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<IEnumerable<EmployeeDTO>> GetEmployees()
        {
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
                Title = employees.Title,
            };
            return EmpDTO;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutEmployees(int id, EmployeeDTO empDTO)
        {
            if (id != empDTO.EmployeeId)
            {
                return "ID不正確";
            }
            Employees emp = await _context.Employees.FindAsync(empDTO.EmployeeId);
            emp.FirstName = empDTO.FirstName;
            emp.LastName = empDTO.LastName;
            emp.Title = empDTO.Title;
            _context.Entry(emp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeesExists(id))
                {
                    return "找不到欲修改的記錄";
                }
                else
                {
                    throw;
                }
            }
            return "修改成功!!";
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<Employees> PostEmployees(EmployeeDTO employees)
        {
            Employees emp = new Employees
            {
                FirstName = employees.FirstName,
                LastName = employees.LastName,
                Title= employees.Title,
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
                return "找不到欲刪除的記錄!";
            }

            _context.Employees.Remove(employees);
            await _context.SaveChangesAsync();

            return "刪除成功!";
        }

        [HttpPost("Filter")] // ==> URI: api/Employees/Filter
        public async Task<IEnumerable<EmployeeDTO>> FilterEmployee([FromBody]EmployeeDTO employees)
        {
            return _context.Employees.Where(emp => emp.FirstName.Contains(employees.FirstName)).Select(emp => new EmployeeDTO
            {
                EmployeeId = emp.EmployeeId,
                FirstName = emp.FirstName,
                LastName = emp.LastName,  
                Title = emp.Title,
            });
        }
        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
