using NUnit.Framework;
using iAge.ConsoleApp.Models;

namespace iAge.ConsoleApp.DataAccess.Tests
{
    [TestFixture("employeesTest.json")]
    public class JsonProviderTests
    {
        public string JsonPath { get; set; }
        public JsonProvider<Employee> JsonProvider { get; set; }

        public JsonProviderTests(string jsonPath)
        {
            JsonPath = jsonPath;
            JsonProvider = new JsonProvider<Employee>(JsonPath);

            JsonProvider.Add(new Employee("Ivan", "Ivanov", 50000));
            JsonProvider.Add(new Employee("Grigory", "Grigoryev", 100000));
        }

        [TestCase(0, ExpectedResult = 1)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(2, ExpectedResult = 0)]
        public int TestUpdate_NonExistentModel_ReturnsZero(int id)
        {
            return JsonProvider.Update(id, new Employee(null, null, 20000));
        }

        [TestCaseSource("UpdateExistingModelWithNewOne")]
        public void TestUpdate_ProvidedItem_IgnoreNull(int id, Employee newModel, Employee expectedResult)
        {
            JsonProvider.Update(id, newModel);

            var actualResult = JsonProvider.Get(id);

            if (actualResult != null)
                Assert.Equals(expectedResult, actualResult);
        }

        public static IEnumerable<TestCaseData> UpdateExistingModelWithNewOne()
        {
            yield return new TestCaseData(0, new Employee(null, null, 20000), new Employee(0, "Ivan", "Ivanov", 20000));
            yield return new TestCaseData(1, new Employee(null, "Vasnetsov", null), new Employee(1, "Grigory", "Vasnetsov", 100000));
        }
    }
}
