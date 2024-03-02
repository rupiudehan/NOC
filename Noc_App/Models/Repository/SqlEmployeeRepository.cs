using Noc_App.Context;
using Noc_App.Models.interfaces;
using Noc_App.Models.ViewModel;

namespace Noc_App.Models.Repository
{
    public class SqlEmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public SqlEmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Employee Add(Employee employee)
        {
            _context.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _context.Employee.Find(id);
            if (employee != null)
            {
                _context.Remove(employee);
                _context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employee==null ? null:_context.Employee.ToList();
        }

        public Employee GetEmployee(int id)
        {
            return _context.Employee.Find(id);
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = _context.Employee.Attach(employeeChanges);
            employee.State=Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return employeeChanges;
        }
    }
}
