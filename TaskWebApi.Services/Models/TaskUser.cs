using System;
using System.Collections.Generic;

namespace TaskWebApi.Services.Models
{
    public partial class TaskUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TaskId { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Task Task { get; set; }
    }
}
