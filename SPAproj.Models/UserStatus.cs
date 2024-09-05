using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SPAproj.Models;

public class UserStatus
{
    [Key]
    public int UserId { get; set; }
    public int Status { get; set; }
}
