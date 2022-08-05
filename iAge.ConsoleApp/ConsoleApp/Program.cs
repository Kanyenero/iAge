using iAge.ConsoleApp.DataAccess;
using iAge.ConsoleApp.Models;

namespace iAge.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string jsonPath = "employees.json";

            IInvocableProvider<Employee> employeeProvider = new JsonProvider<Employee>(jsonPath);

            var commandLineHandler = new CommandLineHandler();

            if (employeeProvider != null)
            {
                commandLineHandler.EmployeeAddEvent += employeeProvider.OnAdd;
                commandLineHandler.EmployeeUpdateEvent += employeeProvider.OnUpdate;
                commandLineHandler.EmployeeGetEvent += employeeProvider.OnGet;
                commandLineHandler.EmployeeDeleteEvent += employeeProvider.OnDelete;
                commandLineHandler.EmployeeGetAllEvent += employeeProvider.OnGetAll;
            }

            commandLineHandler.CreateCommandTree();
            commandLineHandler.RunCommand(args);
        }
    }
}
