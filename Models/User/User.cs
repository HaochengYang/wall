using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wall.Models{
    public class User: BaseEntity
    {
        // public User(){
        //     quotes = new List<Quote>();
        // }
        [Key]
       public int Userid {get;set;}
       public string firstname {get;set;}

       public string lastname {get;set;}

       public string email {get;set;}

       public string username {get;set;}

       public string password {get;set;}
       
       public string cpassword {get;set;}

       public DateTime CreateAt { get; set; }

       public DateTime UpdateAt { get; set; }

       //public List<Quote>quotes {get;set;}

   }

}
