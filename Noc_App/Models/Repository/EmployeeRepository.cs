using Noc_App.Models.interfaces;

namespace Noc_App.Models.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public EmployeeRepository()
        {
            _employeeList=new List<Employee>() { 
                new Employee(){Id=1,Name="Mary",Department=Dept.HR,Email="mary@gmail.com"},
                new Employee(){Id=2,Name="Jane",Department=Dept.IT,Email="jane@gmail.com"},
                new Employee(){Id=3,Name="Sam",Department=Dept.Payroll,Email="sam@gmail.com"}
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id=_employeeList.Max(x => x.Id)+1;
            _employeeList.Add(employee);
            return employee;

        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email= employeeChanges.Email;
                employee.Department = employeeChanges.Department;
                _employeeList.Add(employee);
            }
            return employee;
        }
    }
}
