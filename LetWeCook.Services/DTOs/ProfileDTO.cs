using LetWeCook.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetWeCook.Services.DTOs
{
    public class ProfileDTO
    {
        public Guid Id { get; set; }
        public Guid UserId {  get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime DateJoined { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber {  get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
