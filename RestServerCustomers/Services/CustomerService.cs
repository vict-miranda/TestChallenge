using Newtonsoft.Json.Linq;
using RestServerCustomers.Model;
using RestServerCustomers.Validations;

namespace RestServerCustomers.Services
{
    public class CustomerService : ICustomerService
    {
        private string jsonFile = string.Empty;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomerService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            jsonFile = Path.Combine(_webHostEnvironment.ContentRootPath, "Persistence\\customers.json");
        }

        /// <inheritdoc/>
        public List<Customer> GetCustomers()
        {
            var json = File.ReadAllText(jsonFile);
            List<Customer> customers = new List<Customer>();
            
            try
            {
                var jObject = JObject.Parse(json);

                if (jObject != null)
                {
                    JArray customersArray = (JArray)jObject["customers"];
                    if (customersArray != null)
                    {
                        foreach (var item in customersArray)
                        {
                            customers.Add(new Customer
                            {
                                Id = Convert.ToInt32(item["id"]),
                                FirstName = item["firstName"].ToString(),
                                LastName = item["lastName"].ToString(),
                                Age = Convert.ToInt32(item["age"]),
                            });
                        }

                    }
                }

                return customers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public (List<Customer>, ValidationErrors) AddCustomer(List<Customer> newCustomers)
        {
            var validationError = new ValidationErrors();
            var validator = new CustomerValidator();
            var customers = GetCustomers();

            foreach (var customer in newCustomers)
            {
                var result = validator.Validate(customer);

                if (!result.IsValid)
                {
                    validationError.ErrorMessages.AddRange(result.Errors.Select(x => x.ErrorMessage).ToList());
                    return (null, validationError);
                }

                try
                {                   

                    if (customers.Any(x => x.Id == customer.Id))
                    {
                        validationError.ErrorMessages.Add("Id is already in use");
                        return (null, validationError);
                    }

                    var newCustomer = "{ 'id': " + customer.Id + ", 'firstName': '" + customer.FirstName + "', 'lastName': '" + customer.LastName + "', 'age': '" + customer.Age + "'}";
                    var customerAdd = new Customer { Id = customer.Id, FirstName = customer.FirstName, LastName = customer.LastName, Age = customer.Age };

                    int index = customers.BinarySearch(customerAdd, new CustomerComparer());
                    if (index < 0)
                    {
                        index = ~index;
                    }
                    customers.Insert(index, customerAdd);

                    //InsertOrderedBinarySearch(customers, customerAdd);

                    var json = File.ReadAllText(jsonFile);
                    var jsonObj = JObject.Parse(json);
                    var customersArray = jsonObj.GetValue("customers") as JArray;
                    var newCompany = JObject.Parse(newCustomer);
                    customersArray.Add(newCompany);

                    jsonObj["customers"] = customersArray;
                    string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj,
                                           Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(jsonFile, newJsonResult);

                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return (customers, null);
        }

        public static void InsertOrderedBinarySearch<T>(List<T> list, T item) where T : IComparable<T>
        {
            int left = 0;
            int right = list.Count - 1;

            while (left <= right)
            {
                int mid = (left + right) / 2;
                int comparison = item.CompareTo(list[mid]);

                if (comparison == 0)
                {
                    list.Insert(mid, item);
                    return;
                }
                else if (comparison < 0)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            list.Insert(left, item);
        }

    }
}
