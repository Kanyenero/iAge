namespace iAge.ConsoleApp.Models
{
    public class Employee : IIndexableModel, ICopyableModel<Employee>, IEquatable<Employee>
    {
        public int Id { get; set; }
        public string? FirstName { get; set; } = default!;
        public string? LastName { get; set; } = default!;
        public decimal? Salary { get; set; } = default!;

        public Employee()
        {
        }

        public Employee(int id)
            : this(id, null, null, null)
        {
            Id = id;
        }

        public Employee(string? firstName, string? lastName, decimal? salary) 
            : this(-1, firstName, lastName, salary)
        {
        }

        public Employee(int id, string? firstName, string? lastName, decimal? salary)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Salary = salary;
        }

        public override string ToString()
        {
            return 
                $"{nameof(Id)} = {Id}, " +
                $"{nameof(FirstName)} = {FirstName}, " +
                $"{nameof(LastName)} = {LastName}, " +
                $"{nameof(Salary)} = {Salary}";
        }

        public void ShallowCopyFrom(Employee other, bool ignoreNull)
        {
            throw new NotImplementedException();
        }

        public void DeepCopyFrom(Employee other, bool ignoreNull)
        {
            Id = other.Id;

            if (ignoreNull)
            {
                FirstName = other.FirstName;
                LastName = other.LastName;
                Salary = other.Salary;

                return;
            }

            if (other.FirstName != null)
                FirstName = other.FirstName;

            if (other.LastName != null)
                LastName = other.LastName;

            if (other.Salary != null)
                Salary = other.Salary;
        }

        public bool Equals(Employee? other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return 
                Id == other.Id &&
                FirstName == other.FirstName &&
                LastName == other.LastName &&
                Salary == other.Salary;
        }
    }
}