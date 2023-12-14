namespace RestServerCustomers.Model
{
    public class CustomerComparer : IComparer<Customer>
    {
        public int Compare(Customer x, Customer y)
        {
            int result = x.FirstName.ToLower().CompareTo(y.FirstName.ToLower());
            if (result == 0)
            {
                result = x.LastName.ToLower().CompareTo(y.LastName.ToLower());
            }
            return result;
        }
    }
}
