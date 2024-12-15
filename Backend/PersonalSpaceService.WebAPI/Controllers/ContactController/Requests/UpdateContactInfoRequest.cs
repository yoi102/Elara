﻿using FluentValidation;

namespace PersonalSpaceService.WebAPI.Controllers.ContactController.Requests
{

    public record class UpdateContactInfoRequest(string Remark);

    public class UpdateContactInfoRequestValidator : AbstractValidator<UpdateContactInfoRequest>
    {
        public UpdateContactInfoRequestValidator()
        {
            RuleFor(e => e.Remark).NotNull().NotEmpty();
        }
    }

}
