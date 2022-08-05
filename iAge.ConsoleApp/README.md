### Заметки по заданию:

- программа генерирует JSON файл, если таковой не существует в указанной директории;
- задать путь к JSON файлу можно только через явное указание в Program;
- стандартное расположение генерируемого JSON файла:
    - для основной программы: ...\iAge.ConsoleApp\ConsoleApp\bin\Debug\net6.0\employees.json
    - для тестов: ...\iAge.ConsoleApp\DataAccess.Tests\bin\Debug\net6.0\employeesTest.json

С Newtonsoft.Json (да и вообще с Json в целом) работал впервые. Опирался на примеры из официальной документации:
https://www.newtonsoft.com/json/help/html/Samples.htm
