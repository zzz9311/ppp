using Microsoft.EntityFrameworkCore;

namespace DAL.Entities
{
    [Index(nameof(Username), IsUnique = true, Name = "UK_Username")]
    public class AppUser
    {
        public AppUser()
        {
            Id = Guid.NewGuid();    
        }
        
        public Guid Id { get; init; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
