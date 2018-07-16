
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace Wall
{
    public class messages{
        [Key]
        [Required]
        public int MessagesId {get;set;}
        [Required]
        public int UserId {get;set;}

        [Required]
        [MinLength(3)]
        public string Messages_Text {get;set;}


        [Required]
        public DateTime Created_At {get;set;}
        public DateTime Updated_At {get;set;}
    
        public List<comments> comments {get;set;}

        public messages(){
            comments = new List<comments>();
        }
    
    }




}