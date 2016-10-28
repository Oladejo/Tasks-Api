using System.Collections.Generic;
using TaskWebApi.Models.DTO;
using TaskWebApi.Models.Utilities;

namespace TaskWebApi.Models.DRO
{
    public class TaskResponses : ResponseMessage
    {
        public TaskResponses()
        {
            Data = new List<Task_DTO>();
        }
        public List<Task_DTO> Data { get; set; }
    }

    public class UserDetailsResponses : ResponseMessage
    {
        public UserDetailsResponses()
        {
            Data = new List<UserDetailsDTO>();
        }

        public List<UserDetailsDTO> Data { get; set; }
    }
}