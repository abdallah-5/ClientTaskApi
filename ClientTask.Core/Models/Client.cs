using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientTask.Core.Models
{
    public class Client : BaseEntity
    {
      
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
       
    }
}