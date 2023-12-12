using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using RestServerCustomers.Model;
using RestServerCustomers.Services;
using Telerik.JustMock;

namespace RestServerCustomers.Tests.IntegrationTests
{
    [TestFixture]
    public class Tests
    {
        private static int _id = 0;
        private CustomWebApplicationFactory<Program> _applicationFactory;
        private CustomerService _customerService2;
        private IWebHostEnvironment _webHostEnvironment;
        public HttpClient GetClient() => _applicationFactory.CreateClient();

        [SetUp]
        public void Setup()
        {
            _applicationFactory = new CustomWebApplicationFactory<Program>();
            _webHostEnvironment = Mock.Create<IWebHostEnvironment>();
            _customerService2 = new CustomerService(_webHostEnvironment);
            _id = GetNextAvailableId();
        }

        [Test, Order(1)]
        public async Task CustomersController_GetAll_ShouldGetCustomersData()
        {
            //Arrange
            var client = GetClient();

            //Act
            var result = await client.GetAsync("/api/Customers");
            var responseString = await result.Content.ReadAsStringAsync();
            var actualResult = JsonConvert.DeserializeObject<List<Customer>>(responseString);

            //Assert
            Assert.IsTrue(result.StatusCode == System.Net.HttpStatusCode.OK && actualResult.Count > 0);
        }

        [Test, Order(2)]
        public async Task CustomersController_Post_ShouldCreateSuccessfully()
        {
            //Arrange
            var client = GetClient();
            
            var newCustomer = new Customer
            {
                Id = _id + 1,
                FirstName = GetRandomFirstName(),
                LastName = GetRandomLastName(),
                Age = GetRandonAge() + 8
            };

            //Act
            var result = await client.PostAsync("/api/Customers", ContentHelper.GetStringContent(newCustomer));
            var responseString = await result.Content.ReadAsStringAsync();
            var actualResult = JsonConvert.DeserializeObject<List<Customer>>(responseString);

            //Assert
            Assert.IsTrue(result.StatusCode == System.Net.HttpStatusCode.OK && actualResult.Count > 0);
            _id = _id + 1;
        }

        [Test, Order(3)]
        [TestCaseSource(typeof(CustomersWithTestFixtures), nameof(CustomersWithTestFixtures.CustomersTestData))]
        public async Task CustomersController_Post_ShouldValidateDataBeforeCreation(
            string firstName, string lastName, int age)
        {
            //Arrange
            var client = GetClient();

            var newCustomer = new Customer
            {
                Id = _id + 1,
                FirstName = firstName,
                LastName = lastName,
                Age = age
            };

            //Act
            var result = await client.PostAsync("/api/Customers", ContentHelper.GetStringContent(newCustomer));
            var responseString = await result.Content.ReadAsStringAsync();
            var actualResult = JsonConvert.DeserializeObject<List<Customer>>(responseString);

            //Assert
            Assert.IsTrue(result.StatusCode == System.Net.HttpStatusCode.OK && actualResult.Count > 0);
            _id = _id + 1;
        }

        public int GetNextAvailableId()
        {
            var list = _customerService2.GetCustomers();
            return list.OrderByDescending(x => x.Id).FirstOrDefault().Id;
        }

        public static int GetRandonAge()
        {
            Random r = new Random();
            return r.Next(10, 90);
        }

        public static string GetRandomLastName()
        {
            Random r = new Random();
            var lastNamesList = GetLastNames();

            int index = r.Next(lastNamesList.Count);
            string randomLastNameString = lastNamesList[index];

            return randomLastNameString;
        }

        public static string GetRandomFirstName()
        {
            Random r = new Random();
            var firstNamesList = GetFirstNames();

            int index = r.Next(firstNamesList.Count);
            string randomFirstNameString = firstNamesList[index];

            return randomFirstNameString;
        }

        private static List<string> GetFirstNames()
        {
            return new List<string>
            {
                "Leia",
                "Sadie",
                "Jose",
                "Sara",
                "Frank",
                "Dewey",
                "Tomas",
                "Joel",
                "Lukas",
                "Carlos"
            };
        }

        private static List<string> GetLastNames()
        {
            return new List<string>
            {
                "Liberty",
                "Ray",
                "Harrison",
                "Ronan",
                "Drew",
                "Powell",
                "Larsen",
                "Chan",
                "Anderson",
                "Lane"
            };
        }
    }

    public class CustomersWithTestFixtures
    {
        public static object[] CustomersTestData =
        {
            new object[] { Tests.GetRandomFirstName(), Tests.GetRandomLastName(), Tests.GetRandonAge() },
            new object[] { Tests.GetRandomFirstName(), Tests.GetRandomLastName(), Tests.GetRandonAge() },
            new object[] { Tests.GetRandomFirstName(), Tests.GetRandomLastName(), Tests.GetRandonAge() },
        };
    }

}
