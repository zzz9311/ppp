using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPPP.BLL.Interfaces.Filters
{
    public class UserFilter
    {
        public UserFilter()
        {
        }

        public UserFilter(Guid userId)
        {
            Id = userId;
        }

        public UserFilter(string username)
        {
            Username = username;
        }
        public Guid? Id { get; init; }
        public string Username { get; init; }
    }
}
