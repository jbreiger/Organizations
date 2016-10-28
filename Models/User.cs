using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace beltReviewer.Models {
 public class User: BaseEntity
    {
       

        [Key]
        public long id { get; set; }
        [Required]
        public string first_name { get; set; }
        [Required]
        public string last_name { get; set; }
        [Required]
        // [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        public string email { get; set; }

        public Group groups {get; set;}
    
          public ICollection<UserGroup> user_groups {get; set;}

       
    }
}       