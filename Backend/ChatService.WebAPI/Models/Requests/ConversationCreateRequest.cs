﻿using FluentValidation;

namespace ChatService.WebAPI.Models.Requests;


public record ConversationCreateRequest(string Name, ConversationAddMemberRequest[] Member);

public class ConversationCreateRequestValidator : AbstractValidator<ConversationCreateRequest>
{
    public ConversationCreateRequestValidator()
    {
        RuleFor(e => e.Name).NotNull().NotEmpty();
        RuleFor(e => e.Member).NotNull();
    }
}



