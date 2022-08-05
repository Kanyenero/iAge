### Заметки по заданию

- программа генерирует JSON файл, если таковой не существует в указанной директории;
- задать путь к JSON файлу можно только через явное указание в Program;
- стандартное расположение генерируемого JSON файла:
    - для основной программы: ...\iAge.ConsoleApp\ConsoleApp\bin\Debug\net6.0\employees.json
    - для тестов: ...\iAge.ConsoleApp\DataAccess.Tests\bin\Debug\net6.0\employeesTest.json

С Newtonsoft.Json (да и вообще с Json в целом) работал впервые. Опирался на примеры из официальной документации:
https://www.newtonsoft.com/json/help/html/Samples.htm

### Примеры использования

1. Добавление сотрудника
Команда
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
Option 'Salary' is required.

Description:

Usage:
  iAge.ConsoleApp -add [options]

Options:
  FirstName <FirstName> (REQUIRED)  Employee first name
  LastName <LastName> (REQUIRED)    Employee last name
  Salary <Salary> (REQUIRED)        Employee salary
  -?, -h, --help                    Show help and usage information

```
