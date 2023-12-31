﻿using Microsoft.AspNetCore.Mvc;
using RestServerCustomers.Model;
using RestServerCustomers.Services;

namespace RestServerCustomers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ICustomerService _customerService;        

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Gets a list of customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await _customerService.GetCustomers();          
            return Ok(customers);
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="customers">Customers information</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]List<Customer> customers)
        {
            var (response, errors) = await _customerService.AddCustomer(customers);
            if (errors != null && errors.Any())
            {
                return BadRequest(errors);
            }
            return Ok(response);
        }
    }
}
