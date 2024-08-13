using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SPAproj.Server.Models;

public class UserPassword
{
    [Key]
    public int UserId { get; set; }
    public string Password { get; set; }
}
