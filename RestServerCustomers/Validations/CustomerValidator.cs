using FluentValidation;
using RestServerCustomers.Model;

namespace RestServerCustomers.Validations
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {

            RuleFor(x => x.FirstName).NotEmpty().NotNull();
            RuleFor(x => x.LastName).NotEmpty().NotNull();
            RuleFor(x => x.Age).NotEmpty().NotNull().GreaterThan(18);
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
