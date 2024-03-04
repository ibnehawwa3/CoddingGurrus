using CoddingGurrus.Core.Models.Topics;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Core.Validations
{
    public class TopicModelValidator : AbstractValidator<TopicsModel>
    {
        public TopicModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.CourseId).Must(id => id > 0).WithMessage("Select a valid course");
        }
    }
}
