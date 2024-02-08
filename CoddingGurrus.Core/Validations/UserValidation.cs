using CoddingGurrus.Core.Models.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Validations
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("*Required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("*Required").Length(6, 10);
            RuleFor(x => x.Email).EmailAddress().WithMessage("Not Valid").NotEmpty().WithMessage("*Required");
        }
    }
}
