namespace RestServerCustomers.Model
{
    public class CustomerComparer : IComparer<Customer>
    {
        public int Compare(Customer x, Customer y)
        {
            int result = x.FirstName.CompareTo(y.FirstName);
            if (result == 0)
            {
                result = x.LastName.CompareTo(y.LastName);
            }
            return result;
        }
    }
}
