using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServices.Services
{
    public class UserService: IUserService
    {
        public UserService(HttpClient httpClient)
        {

            var url = "SocialLink/api/user";

        }

       

    }
}
