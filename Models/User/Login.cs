using System;
using System.ComponentModel.DataAnnotations;
namespace wall.Models {
    public class LogUser : BaseEntity
    {
       [Required]
       [MinLength(2)]
       [Display(Name ="Username:")]
       public string username {get;set;}

       [Required]
       [DataType(DataType.Password)]
       [Display(Name ="Password:")]
       public string password {get;set;}
    }
}