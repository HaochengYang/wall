using System;
using System.ComponentModel.DataAnnotations;
namespace wall.Models {
    public class RegUser : BaseEntity
    {
       [Required]
       [MinLength(2)]
       [Display(Name ="First Name:")]
       public string firstname {get;set;}

       [Required]
       [MinLength(2)]
       [Display(Name ="Last Name:")]
       public string lastname {get;set;}
       [Required]
       [EmailAddress]
       [Display(Name ="Email:")]
       public string email {get;set;}

       [Required]
       [MinLength(2)]
       [Display(Name ="Username:")]
       public string username {get;set;}

       [Required]
       [DataType(DataType.Password)]
       [Display(Name ="Password:")]
       public string password {get;set;}

       [Required]
       [CompareAttribute("password", ErrorMessage = "Password confirmation must match Password")]
       [Display(Name ="Confirm Your Password:")]
       public string cpassword {get;set;}

       public DateTime CreatedAt { get; set; }

       public DateTime UpdatedAt { get; set; }
    }
}