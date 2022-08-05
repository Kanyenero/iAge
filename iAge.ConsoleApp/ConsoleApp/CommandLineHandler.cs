using System.CommandLine;
using System.CommandLine.Parsing;
using iAge.ConsoleApp.Models;

namespace iAge.ConsoleApp
{
    internal class CommandLineHandler
    {
        private readonly RootCommand rootCommand;

        public CommandLineHandler()
        {
            rootCommand = new RootCommand("Provides basic model storage manipulation procedures.");
        }

        public void RunCommand(string[] args)
        {
            rootCommand.Invoke(args);
        }

        public event ModelEventHandler<Employee> EmployeeAddEvent = default!;
        public event ModelEventHandler<Employee> EmployeeUpdateEvent = default!;
        public event ModelEventHandler<Employee> EmployeeGetEvent = default!;
        public event ModelEventHandler<Employee> EmployeeDeleteEvent = default!;
        public event ModelEventHandler<Employee> EmployeeGetAllEvent = default!;

        public RootCommand CreateCommandTree()
        {
            var addEmployeeCommand = CreateAddEmployeeCommandBranch();
            var updateEmployeeCommand = CreateUpdateEmployeeCommandBranch();
            var getEmployeeCommand = CreateGetEmployeeCommandBranch();
            var deleteEmployeeCommand = CreateDeleteEmployeeCommandBranch();
            var getAllEmployeeCommand = CreateGetAllEmployeeCommandBranch();

            rootCommand.AddCommand(addEmployeeCommand);
            rootCommand.AddCommand(updateEmployeeCommand);
            rootCommand.AddCommand(getEmployeeCommand);
            rootCommand.AddCommand(deleteEmployeeCommand);
            rootCommand.AddCommand(getAllEmployeeCommand);

            return rootCommand;
        }

        private Command CreateAddEmployeeCommandBranch()
        {
            var employeeFirstNameOption = CreateSingleTokenOption(name: "FirstName", isRequired: true, description: "Employee first name");
            var employeeLastNameOption = CreateSingleTokenOption(name: "LastName", isRequired: true, description: "Employee last name");
            var employeeSalaryOption = CreateSingleTokenOption<decimal>(name: "Salary", isRequired: true, description: "Employee salary");

            var addEmployeeCommand = new Command("-add", "Adds new employee to storage");

            addEmployeeCommand.AddOption(employeeFirstNameOption);
            addEmployeeCommand.AddOption(employeeLastNameOption);
            addEmployeeCommand.AddOption(employeeSalaryOption);

            addEmployeeCommand.SetHandler((firstName, lastName, salary) =>
            {
                if (firstName == null || lastName == null || salary == null)
                    return;

                var employee = new Employee(firstName, lastName, salary);

                EmployeeAddEvent.Invoke(this, new ModelEventArgs<Employee>(employee));
            },
            employeeFirstNameOption,
            employeeLastNameOption,
            employeeSalaryOption);

            return addEmployeeCommand;
        }

        private Command CreateUpdateEmployeeCommandBranch()
        {
            var employeeIdOption = CreateSingleTokenOption<int>(name: "Id", isRequired: true, description: "Employee Id");
            var employeeFirstNameOption = CreateSingleTokenOption(name: "FirstName", isRequired: false, description: "Employee first name");
            var employeeLastNameOption = CreateSingleTokenOption(name: "LastName", isRequired: false, description: "Employee last name");
            var employeeSalaryOption = CreateSingleTokenOption<decimal>(name: "Salary", isRequired: false, description: "Employee salary");

            var updateEmployeeCommand = new Command("-update", "Updates employee on [id] with specified data");

            updateEmployeeCommand.AddOption(employeeIdOption);
            updateEmployeeCommand.AddOption(employeeFirstNameOption);
            updateEmployeeCommand.AddOption(employeeLastNameOption);
            updateEmployeeCommand.AddOption(employeeSalaryOption);

            updateEmployeeCommand.SetHandler((id, firstName, lastName, salary) =>
            {
                if (id == null)
                    return;

                var employee = new Employee(id ?? -1, firstName, lastName, salary);

                EmployeeUpdateEvent.Invoke(this, new ModelEventArgs<Employee>(employee));
            },
            employeeIdOption,
            employeeFirstNameOption,
            employeeLastNameOption,
            employeeSalaryOption);

            return updateEmployeeCommand;
        }

        private Command CreateGetEmployeeCommandBranch()
        {
            var employeeIdOption = CreateSingleTokenOption<int>(name: "Id", isRequired: true, description: "Employee Id");

            var getEmployeeCommand = new Command("-get", "Gets employee with the specified id");

            getEmployeeCommand.AddOption(employeeIdOption);

            getEmployeeCommand.SetHandler((id) =>
            {
                if (id == null)
                    return;

                var employee = new Employee(id ?? -1);

                EmployeeGetEvent.Invoke(this, new ModelEventArgs<Employee>(employee));
            },
            employeeIdOption);

            return getEmployeeCommand;
        }

        private Command CreateDeleteEmployeeCommandBranch()
        {
            var employeeIdOption = CreateSingleTokenOption<int>(name: "Id", isRequired: true, description: "Employee Id");

            var deleteEmployeeCommand = new Command("-delete", "Deletes employee with the specified id");

            deleteEmployeeCommand.AddOption(employeeIdOption);

            deleteEmployeeCommand.SetHandler((id) =>
            {
                if (id == null)
                    return;

                var employee = new Employee(id ?? -1);

                EmployeeDeleteEvent.Invoke(this, new ModelEventArgs<Employee>(employee));
            },
            employeeIdOption);

            return deleteEmployeeCommand;
        }

        private Command CreateGetAllEmployeeCommandBranch()
        {
            var getAllEmployeeCommand = new Command("-getall", "Provides all employees records from storage");

            getAllEmployeeCommand.SetHandler(() =>
            {
                EmployeeGetAllEvent.Invoke(this, new ModelEventArgs<Employee>(new Employee()));
            });

            return getAllEmployeeCommand;
        }

        private static Option<T?> CreateSingleTokenOption<T>(string name, bool isRequired = false, string? description = null)
            where T : struct
        {
            var valueToReturnType = typeof(T);
            var valueToReturn = new T();

            var parseArgument = new ParseArgument<T?>(parseArgumentResult => 
            {
                string? tokenValue = GetStringFromSingleTokenOption(parseArgumentResult, "FirstName");

                var parseMethod = valueToReturnType.GetMethod("Parse", new[] { typeof(string) });

                if (parseMethod == null)
                    throw new Exception($"No provided method [Parse] on type '{valueToReturnType}'.");

                var parseMethodInvokeResult = parseMethod.Invoke(valueToReturn, new[] { tokenValue });

                if (parseMethodInvokeResult == null)
                {
                    parseArgumentResult.ErrorMessage = $"[{name}] Cannot parse argument. Format was invalid.";
                    return null;
                }

                if (parseMethodInvokeResult is T)
                {
                    return (T)parseMethodInvokeResult;
                }

                try
                {
                    return (T)Convert.ChangeType(parseMethodInvokeResult, typeof(T));
                }
                catch (InvalidCastException)
                {
                    throw;
                }
            });

            return new Option<T?>(name, parseArgument, isDefault: false, description)
            {
                AllowMultipleArgumentsPerToken = false,
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = isRequired
            };
        }

        private static Option<string?> CreateSingleTokenOption(string name, bool isRequired = false, string? description = null)
        {
            var parseArgument = new ParseArgument<string?>(parseArgumentResult =>
            {
                return GetStringFromSingleTokenOption(parseArgumentResult, "FirstName");
            });

            return new Option<string?>(name, parseArgument, isDefault: false, description)
            {
                AllowMultipleArgumentsPerToken = false,
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = isRequired
            };
        }

        private static string? GetStringFromSingleTokenOption(ArgumentResult result, string optionName)
        {
            if (result.Tokens.Count == 0)
                return null;

            var token = result.Tokens[0];

            if (string.IsNullOrWhiteSpace(token.Value))
            {
                result.ErrorMessage = $"[{optionName}] Option argument was null or whitespace.";

                return null;
            }

            return token.Value;
        }
    }
}
