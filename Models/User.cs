using System.Collections.Generic;

namespace Lancelittle.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int AuthLevel { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public List<Character> Characters { get; set; }
    }
}