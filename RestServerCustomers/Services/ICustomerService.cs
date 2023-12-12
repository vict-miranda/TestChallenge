using RestServerCustomers.Model;
using RestServerCustomers.Validations;

namespace RestServerCustomers.Services
{
    public interface ICustomerService
    {
        /// <summary>
        /// Gets a list of customers
        /// </summary>
        /// <returns>A list of <see cref="Customer"/></returns>
        List<Customer> GetCustomers();

        /// <summary>
        /// Creates a new Customer
        /// </summary>
        /// <param name="newCustomers">Customers information</param>
        /// <returns>A tuple with the new customer list, and an object with possible errors</returns>
        (List<Customer>, ValidationErrors) AddCustomer(List<Customer> newCustomers);
    }
}
