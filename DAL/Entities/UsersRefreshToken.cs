using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class UsersRefreshToken
    {
        public UsersRefreshToken()
        {
            RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        }
        public int Id { get; init; }
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
