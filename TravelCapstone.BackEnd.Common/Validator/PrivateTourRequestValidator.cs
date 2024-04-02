using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO;

namespace TravelCapstone.BackEnd.Common.Validator
{
    public class PrivateTourRequestValidator: AbstractValidator<PrivateTourRequestDTO>
    {
        public PrivateTourRequestValidator() {
            RuleFor(x => x.NumOfAdult).GreaterThan(0).WithMessage("There must be at least one adult in a tour");
            RuleFor(x => new { x.StartDate, x.EndDate })
                .Must(x => (x.EndDate > x.StartDate))
                .WithMessage("End date must be greater than start date");
        }
    }
}
