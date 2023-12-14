using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestServerCustomers.Model;
using RestServerCustomers.Validations;
using System;

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

        /// <inheritdoc/>
        public (List<Customer>, ValidationErrors) AddCustomer2(List<Customer> newCustomers)
        {
            var validationError = new ValidationErrors();
            var validator = new CustomerValidator();
            var customers = GetCustomers();

            //validate and insert records into the list
            foreach (var customer in newCustomers)
            {
                var result = validator.Validate(customer);

                if (!result.IsValid)
                {
                    validationError.ErrorMessages.AddRange(result.Errors.Select(x => x.ErrorMessage).ToList());
                    return (null, validationError);
                }

                if (customers.Any(x => x.Id == customer.Id))
                {
                    validationError.ErrorMessages.Add("Id is already in use");
                    return (null, validationError);
                }

                var customerAdd = new Customer { Id = customer.Id, FirstName = customer.FirstName, LastName = customer.LastName, Age = customer.Age };

                int index = customers.BinarySearch(customerAdd, new CustomerComparer());
                if (index < 0)
                {
                    index = ~index;
                }
                customers.Insert(index, customerAdd);
            }            

            try
            {
                //save json with customers
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

                jsonObj["customers"] = customersArray;
                string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(jsonFile, newJsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }            

            return (customers, null);
        }

    }
}
