### Заметки по заданию

- программа генерирует JSON файл, если таковой не существует в указанной директории;
- задать путь к JSON файлу можно только через явное указание в Program;
- стандартное расположение генерируемого JSON файла:
    - для основной программы: ...\iAge.ConsoleApp\ConsoleApp\bin\Debug\net6.0\employees.json
    - для тестов: ...\iAge.ConsoleApp\DataAccess.Tests\bin\Debug\net6.0\employeesTest.json

С Newtonsoft.Json (да и вообще с Json в целом) работал впервые. Опирался на примеры из официальной документации:
https://www.newtonsoft.com/json/help/html/Samples.htm

### Примеры использования

#### Добавление сотрудника
```powershell
> .\iAge.ConsoleApp.exe -add FirstName:Salor LastName:Moon Salary:10000000
```
Вывод при успешном выполнении команды
```powershell
Successfully added a new record.
```
Вывод при неудачном выполнении команды
```powershell
Cannot add a new record.
```
Вывод при неполной команде
```powershell
> .\iAge.ConsoleApp.exe -add FirstName:Salor LastName:Moon
Option 'Salary' is required.

Description: 
  Adds new employee to storage

Usage:
  iAge.ConsoleApp -add [options]

Options:
  FirstName <FirstName> (REQUIRED)  Employee first name
  LastName <LastName> (REQUIRED)    Employee last name
  Salary <Salary> (REQUIRED)        Employee salary
  -?, -h, --help                    Show help and usage information
```

#### Обновление сотрудника
```powershell
> .\iAge.ConsoleApp.exe -update Id:0 Salary:1000
```
Вывод при успешном выполнении команды
```powershell
Successfully updated record on id [0].
```
Вывод при неудачном выполнении команды
```powershell
Cannot update record on id [0].
```
Вывод при неполной команде
```powershell
> .\iAge.ConsoleApp.exe -update
Option 'Id' is required.

Description:
  Updates employee on [id] with specified data

Usage:
  iAge.ConsoleApp -update [options]

Options:
  Id <Id> (REQUIRED)     Employee Id
  FirstName <FirstName>  Employee first name
  LastName <LastName>    Employee last name
  Salary <Salary>        Employee salary
  -?, -h, --help         Show help and usage information
```

#### Получение сотрудника
```powershell
> .\iAge.ConsoleApp.exe -get Id:0
```
Вывод при успешном выполнении команды
```powershell
Id = 0, FirstName = Salor, LastName = Moon, Salary = 1000,0
```
Вывод при неудачном выполнении команды
```powershell
Cannot get record on id [0].
```
Вывод при неполной команде
```powershell
> .\iAge.ConsoleApp.exe -get
Option 'Id' is required.

Description:
  Gets employee with the specified id

Usage:
  iAge.ConsoleApp -get [options]

Options:
  Id <Id> (REQUIRED)  Employee Id
  -?, -h, --help      Show help and usage information
```

#### Удаление сотрудника
```powershell
> .\iAge.ConsoleApp.exe -delete Id:0
```
Вывод при успешном выполнении команды
```powershell
Successfully deleted record on id [0].
```
Вывод при неудачном выполнении команды
```powershell
Cannot delete record on id [0].
```
Вывод при неполной команде
```powershell
> .\iAge.ConsoleApp.exe -delete
Option 'Id' is required.

Description:
  Deletes employee with the specified id

Usage:
  iAge.ConsoleApp -delete [options]

Options:
  Id <Id> (REQUIRED)  Employee Id
  -?, -h, --help      Show help and usage information
```

#### Получение всех сотрудников
```powershell
> .\iAge.ConsoleApp.exe -getall
```
Вывод при успешном выполнении команды
```powershell
Id = 0, FirstName = Salor, LastName = Moon, Salary = 10000000,0
Id = 1, FirstName = Elon, LastName = Musk, Salary = 10000000000,0
```
Вывод при неудачном выполнении команды
```powershell
Cannot get records.
```
