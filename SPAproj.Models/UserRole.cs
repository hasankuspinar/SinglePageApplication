using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SPAproj.Models;

public class UserRole
{
    [Key]
    public int UserId { get; set; }
    public string Role {  get; set; }
}
