using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions.UserResponses
{
    public record GetUserInfoResponse() : ResponseBase
    {
        public UserInfo? UserInfo { get; set; }
    };
}