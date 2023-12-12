namespace RestServerCustomers.Model
{
    public class Customer : IComparable<Customer>
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }

        public int CompareTo(Customer? other)
        {
            return string.Compare(FirstName, other.FirstName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
