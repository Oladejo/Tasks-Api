using System;
using System.Collections.Generic;

namespace TaskWebApi.Services.Models
{
    public partial class Task
    {
        public Task()
        {
            this.TaskUsers = new List<TaskUser>();
        }

        public int Id { get; set; }
        public string EncryptedId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime DueDate { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual ICollection<TaskUser> TaskUsers { get; set; }
    }
}
