using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wall.Models{
    public class Message : BaseEntity{
        public Message(){
            Comments = new List<Comment>();
        }
        
        [Key]
        public int Messageid {get;set;}

        [Required]
        [Display(Name = "Leave Your Message: ")]
        public string message {get;set;}
        
        public int postid {get;set;}


        public User poster {get; set;}

        public List<Comment> Comment {get;set;}

        public DateTime CreateAt {get;set;}
        
        public DateTime UpdateAt {get;set;}
        
        public bool Deletable {
            get {
                var diff = DateTime.Now - CreateAt;
                return (diff.TotalMinutes <= 30);
            }
        }
        }
    }
