namespace RestServerCustomers.Validations
{
    public class ValidationErrors
    {
        public int CustomerId { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
