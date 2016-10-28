using System;
using System.Collections.Generic;

namespace TaskWebApi.Models.DTO
{
    public class Task_DTO
    {
        public int TaskId { get; set; }
        public string EncryptedId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class UserDetailsDTO
    {
        public UserDetailsDTO()
        {
            UserTasks = new List<Task_DTO>();
        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public List<Task_DTO> UserTasks { get; set; }
   }
}