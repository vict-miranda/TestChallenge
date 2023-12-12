using Microsoft.AspNetCore.Mvc;
using RestServerCustomers.Controllers;
using RestServerCustomers.Model;
using RestServerCustomers.Services;
using RestServerCustomers.Validations;
using System.Net;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace RestServerCustomers.Tests.Controllers
{
    [TestFixture]
    public class CustomersControllerTests
    {
        private ICustomerService _customerService;
        private CustomersController _controller;

        [SetUp]
        public void Setup()
        {
            _customerService = Mock.Create<ICustomerService>(Behavior.Strict);
            //_controller = Mock.Create<CustomersController>(new CustomersController(_customerService), Behavior.Strict);
            _controller = new CustomersController(_customerService);
        }

        [Test]
        public void GetCustomers_OK_ReturnList()
        {
            //Arrange            
            _customerService.Arrange(x => x.GetCustomers()).Returns(this.GetCustomerList());

            //Act
            var result = _controller.Get();
            var contentResult = result as OkObjectResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(contentResult, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.AreEqual((int)HttpStatusCode.OK, contentResult.StatusCode);
        }

        [Test]
        public void AddCustomers_ReturnOk()
        {
            //Arrange
            //var newCustomer = new Customer { Id = 5, FirstName = "Test", LastName = "test", Age = 32 };
            //var resultTuple = new ValueTuple<List<Customer>, ValidationErrors>(new List<Customer>(), new ValidationErrors());

            //_customerService.Arrange(x => x.AddCustomer(newCustomer)).Returns(resultTuple);

            ////Act
            //var result = _controller.Post(newCustomer);
            //var contentResult = result as OkObjectResult;

            ////Assert
            //Assert.That(result, Is.Not.Null);
            //Assert.That(contentResult, Is.Not.Null);
            //Assert.That(result, Is.TypeOf<OkObjectResult>());
            //Assert.AreEqual((int)HttpStatusCode.OK, contentResult.StatusCode);
        }

        [Test]
        public void AddCustomers_ReturnError()
        {
            //Arrange
            //var newCustomer = new Customer { Id = 1, FirstName = "", LastName = "", Age = 2 };
            //var validationError = new ValidationErrors { ErrorMessages = new List<string> { "Error" } };
            //var resultTuple = new ValueTuple<List<Customer>, ValidationErrors>(new List<Customer>(), validationError);

            //_customerService.Arrange(x => x.AddCustomer(newCustomer)).Returns(resultTuple);

            ////Act
            //var result = _controller.Post(newCustomer);
            //var contentResult = result as BadRequestObjectResult;

            ////Assert
            //Assert.That(result, Is.Not.Null);
            //Assert.That(contentResult, Is.Not.Null);
            //Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            //Assert.AreEqual((int)HttpStatusCode.BadRequest, contentResult.StatusCode);
        }

        private List<Customer> GetCustomerList()
        {
            return new List<Customer>
            {
                new Customer { Id = 1, FirstName = "Test 1", LastName = "test 1", Age = 18 },
                new Customer { Id = 2, FirstName = "Test 2", LastName = "test 2", Age = 20 },
                new Customer { Id = 3, FirstName = "Test 3", LastName = "test 3", Age = 22 }
            };
        }
    }
}
