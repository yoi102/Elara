﻿namespace SocialLink.WebAPI.Events
{
    public record class ResetPasswordByEmailResetCodeEvent(string Email, string Subject, string HtmlMessage);


}
