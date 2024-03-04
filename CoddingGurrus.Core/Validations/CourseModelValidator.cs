using CoddingGurrus.Core.Models.Course;
using CoddingGurrus.Core.Models.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Validations
{
    public class CourseModelValidator : AbstractValidator<CourseModel>
    {
        public CourseModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is Required");
        }
    }
}
