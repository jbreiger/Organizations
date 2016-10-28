using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace beltReviewer.Models {
 public class UserGroup: BaseEntity
    {
        public UserGroup() {
            users = new List<User>();
            groups = new List<Group>();
        }

        [Key]
        public long id { get; set; }
        [Required]
         
        public ICollection<User> users {get; set;}
        public ICollection<Group> groups {get; set;}

       
    }
}       