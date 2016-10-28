using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace beltReviewer.Models {
 public class Group: BaseEntity
    {
        // public User() {
        //     messages = new List<Message>();
        // }

        [Key]
        public long id { get; set; }
        [Required]
        public string name { get; set; }
        public string description { get; set; }
        public int user_id {get; set;}
        
        public User user {get; set;}

       
    }
}       