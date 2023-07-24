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
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        }
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public string RefreshToken { get; init; }
        public DateTime RefreshTokenExpiryTime { get; init; }
        public string DeviceInfo { get; init; }
    }
}
