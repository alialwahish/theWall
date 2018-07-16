using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

using System;

namespace Wall
{
    public class comments
    {
        [Key]
        [Required]
        public int CommentId { get; set; }
        [Required]
        public int MessagesId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(3)]
        public string Comment_Text {get;set;}

        [Required]
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }




    }





}