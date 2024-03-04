using CoddingGurrus.Core.Models.Menu;
using CoddingGurrus.Core.Models.Topics;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Validations
{
    public class MenuModelValidator : AbstractValidator<MenuModel>
    {
        public MenuModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        }
    }
}
