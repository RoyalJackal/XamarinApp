using Microsoft.AspNetCore.Identity;

namespace Server.Models
{
    public class FirebaseToken
    {
        public int Id { get; set; }
        public IdentityUser User { get; set; }
        public string Token { get; set; }
    }
}