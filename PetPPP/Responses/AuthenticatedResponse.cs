using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPPP.Responses
{
    public class AuthenticatedResponse
    {
        public AuthenticatedResponse(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
