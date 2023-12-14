using Newtonsoft.Json;
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
        public async Task<List<Customer>> GetCustomers()
        {
            var json = await File.ReadAllTextAsync(jsonFile);
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
        public async Task<(List<Customer>, List<ValidationErrors>)> AddCustomer(List<Customer> newCustomers)
        {
            var validationsErrors = new List<ValidationErrors>();
            
            var validator = new CustomerValidator();
            var customers = await GetCustomers();

            //validate and insert records into the list
            foreach (var customer in newCustomers)
            {
                var result = validator.Validate(customer);
                var validationError = new ValidationErrors();

                if (customers.Any(x => x.Id == customer.Id))
                {
                    validationError.CustomerId = customer.Id;
                    validationError.ErrorMessages.Add("Id is already in use");
                    validationsErrors.Add(validationError);
                }

                if (!result.IsValid)
                {
                    validationError.CustomerId = customer.Id;
                    validationError.ErrorMessages.AddRange(result.Errors.Select(x => x.ErrorMessage).ToList());
                    validationsErrors.Add(validationError);
                }                

                if (!validationError.ErrorMessages.Any())
                {
                    var customerAdd = new Customer { Id = customer.Id, FirstName = customer.FirstName, LastName = customer.LastName, Age = customer.Age };

                    int index = customers.BinarySearch(customerAdd, new CustomerComparer());
                    if (index < 0)
                    {
                        index = ~index;
                    }
                    customers.Insert(index, customerAdd);
                }                
            }
            
            //if exists error return an object with errors
            if (validationsErrors.Any())
            {
                return (null, validationsErrors);
            }

            try
            {
                //read json
                var json = File.ReadAllText(jsonFile);
                var jsonObj = JObject.Parse(json);
                var customersArray = jsonObj.GetValue("customers") as JArray;
                customersArray = [];

                //generate json string to save customers
                string newJsonCustomers = string.Empty;

                foreach (var customer in customers)
                {
                    newJsonCustomers = "{ 'id': " + customer.Id + ", 'firstName': '" + customer.FirstName + "', 'lastName': '" + customer.LastName + "', 'age': " + customer.Age + "}";
                    var newCustObj = JObject.Parse(newJsonCustomers);
                    customersArray.Add(newCustObj);
                }

                //save json with customers
                jsonObj["customers"] = customersArray;
                string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                await File.WriteAllTextAsync(jsonFile, newJsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }            

            return (customers, null);
        }

    }
}
