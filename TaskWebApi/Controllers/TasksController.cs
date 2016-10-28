using System;
using System.Linq;
using System.Web.Http;
using TaskWebApi.Models.DRO;
using TaskWebApi.Models.DTO;
using TaskWebApi.Models.Utilities;
using TaskWebApi.Services.Implementations;
using TaskWebApi.Services.Interfaces;
using TaskWebApi.Services.Models;

namespace TaskWebApi.Controllers
{
    [RoutePrefix("api/task")]
    public class TasksController : ApiController
    {
        readonly IUnitOfWork _work;
        private readonly IRepository<AspNetUser> _aspnetUserRepository;
        //private IRepository<AspNetRole> _aspnetRoleRepository;
        private readonly IRepository<Task> _taskRepository;
        private readonly IRepository<TaskUser> _taskUserRepository; 

        public TasksController()
        {
            _work = new UnitOfWork();
            _aspnetUserRepository = _work.Repository<AspNetUser>();
            //_aspnetRoleRepository = _work.Repository<AspNetRole>();
            _taskRepository = _work.Repository<Task>();
            _taskUserRepository = _work.Repository<TaskUser>();
        }

        [Route("")]
        [HttpGet]
        public TaskResponses GetTasks()
        {
            TaskResponses response = new TaskResponses();
            try
            {
                response.Data = _taskRepository.GetAll().Select(x => new Task_DTO
                {
                    TaskId = x.Id,
                    Name = x.Name,
                    EncryptedId = x.EncryptedId,
                    Description = x.Description,
                    DateCreated = x.DateCreated,
                    DueDate = x.DueDate
                }).OrderByDescending(x => x.DateCreated).ToList();
                response.status = response.status;
                return response;
            }
            catch (Exception ex)
            {
                response.status = ResponseErrors.internal_server_error.ToString();
                response.error_messages.Add(ex.Message);
                return response;
            }
        }

        //Get api/task/1
        [Route("{encId}")]
        [HttpGet]
        public TaskResponses GetTask(string encId)
        {
            TaskResponses response = new TaskResponses();
            try
            {
                var task = _taskRepository.Get(x => x.EncryptedId == encId);
                if (task == null)
                {
                    response.status = ResponseErrors.resource_not_found_at_specified_endpoint.ToString();
                }
                else
                {
                    response.Data.Add(new Task_DTO
                    {
                        TaskId = task.Id,
                        EncryptedId = task.EncryptedId,
                        Name = task.Name,
                        Description = task.Description,
                        DateCreated = task.DateCreated,
                        DueDate = task.DueDate
                    });
                }
                return response;
            }
            catch (Exception ex)
            {
                response.status = ResponseErrors.internal_server_error.ToString();
                response.error_messages.Add(ex.Message);
                return response;
            }
        }

        [Route("")]
        [HttpPost]
        public TaskResponses PostTask(Task_DTO taskDto)
        {
            TaskResponses response = new TaskResponses();
            try
            {
                if (_taskRepository.Get(x => x.Name == taskDto.Name) != null)
                {
                    response.status = ResponseErrors.resource_already_exist_at_specified_endpoint.ToString();
                }
                else
                {
                    var newTask = new Task
                    {
                        EncryptedId = Guid.NewGuid().ToString(),
                        Name = taskDto.Name,
                        Description = taskDto.Description,
                        DateCreated = DateTime.Now,
                        DueDate = taskDto.DueDate
                    };
                    _taskRepository.Add(newTask);
                    response.returned_object = new { id = newTask.Id, encryptedId = newTask.EncryptedId };
                    response.status = ResponseStatus.success.ToString();
                }
                return response;
            }
            catch (Exception ex)
            {
                response.status = ResponseErrors.internal_server_error.ToString();
                response.error_messages.Add(ex.Message);
                return response;
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public TaskResponses PutTask(int id, Task_DTO taskDto)
        {
            TaskResponses response = new TaskResponses();
            try
            {
                var task = _taskRepository.Get(x => x.Id == id);
                if (id != taskDto.TaskId || task == null)
                {
                    response.status = ResponseErrors.resource_not_found_at_specified_endpoint.ToString();
                }
                else
                {
                    task.Name = taskDto.Name;
                    task.Description = taskDto.Description;
                    task.DueDate = taskDto.DueDate;
                    _taskRepository.Attach(task);
                    response.status = ResponseStatus.success.ToString();
                }
                return response;
            }
            catch (Exception ex)
            {
                response.status = ResponseErrors.internal_server_error.ToString();
                response.error_messages.Add(ex.Message);
                return response;
            }
        }

        [HttpDelete]
        [Route("{Id:int}")]
        public TaskResponses DeleteTask(int id)
        {
            TaskResponses response = new TaskResponses();
            try
            {
                var task = _taskRepository.Get(x => x.Id == id);
                if (task == null)
                {
                    response.status = ResponseErrors.resource_not_found_at_specified_endpoint.ToString();
                }
                else
                {
                    _taskRepository.Remove(task);
                    response.status = ResponseStatus.success.ToString();
                }
                return response;
            }
            catch (Exception ex)
            {
                response.status = ResponseErrors.internal_server_error.ToString();
                response.error_messages.Add(ex.Message);
                return response;
            }
        }

        //GET LIST OF TASKS not expired (Due Task)
        [Route("Due")]
        [HttpGet]
        public TaskResponses DueTask()
        {
            TaskResponses response = new TaskResponses();
            try
            {
                response.Data = _taskRepository.GetAll().Where(x => x.DueDate > DateTime.Now).Select(x => new Task_DTO
                {
                    TaskId = x.Id,
                    Name = x.Name,
                    EncryptedId = x.EncryptedId,
                    Description = x.Description,
                    DateCreated = x.DateCreated,
                    DueDate = x.DueDate
                }).OrderByDescending(x => x.DueDate).ToList();
                response.status = response.status;
                return response;
            }
            catch (Exception ex)
            {
                response.status = ResponseErrors.internal_server_error.ToString();
                response.error_messages.Add(ex.Message);
                return response;
            }
        }

        //GET LIST OF Expired TASKS 
        [Route("Expired")]
        [HttpGet]
        public TaskResponses ExpireTask()
        {
            TaskResponses response = new TaskResponses();
            try
            {
                response.Data = _taskRepository.GetAll().Where(x => x.DueDate < DateTime.Now).Select(x => new Task_DTO
                {
                    TaskId = x.Id,
                    Name = x.Name,
                    EncryptedId = x.EncryptedId,
                    Description = x.Description,
                    DateCreated = x.DateCreated,
                    DueDate = x.DueDate
                }).OrderByDescending(x => x.DueDate).ToList();
                response.status = response.status;
                return response;
            }
            catch (Exception ex)
            {
                response.status = ResponseErrors.internal_server_error.ToString();
                response.error_messages.Add(ex.Message);
                return response;
            }
        }

        //GET LIST OF USER with their tasks
        [Route("users")]
        [HttpGet]
        public UserDetailsResponses UserDetails()
        {
            UserDetailsResponses response = new UserDetailsResponses();
            try
            {
                response.Data = _aspnetUserRepository.GetAll().Select(ts => new UserDetailsDTO
                {
                    Id = ts.Id,
                    Email = ts.Email,
                    FirstName = ts.FirstName,
                    LastName = ts.LastName,
                    MobilePhone = ts.PhoneNumber,
                    UserTasks = ts.TaskUsers.Select(w => new Task_DTO
                    {
                        TaskId = w.TaskId,
                        EncryptedId = w.Task.EncryptedId,
                        Description = w.Task.Description,
                        Name = w.Task.Name,
                        DateCreated = w.Task.DateCreated,
                        DueDate = w.Task.DueDate
                    }).ToList(),
                }).ToList();
                response.status = response.status;
                return response;
            }
            catch (Exception ex)
            {
                response.status = ResponseErrors.internal_server_error.ToString();
                response.error_messages.Add(ex.Message);
                return response;
            }
        }

        //GET LIST OF USER ASSIGN TO A Particular TASK
        [Route("taskUsers/{id:int}")]
        [HttpGet]
        public UserDetailsResponses UsersAssignToATask(int id)
        {
            UserDetailsResponses response = new UserDetailsResponses();
            try
            {
                response.Data = _taskRepository.Get(x => x.Id == id).TaskUsers.Select(u => new UserDetailsDTO
                {
                    Id = u.AspNetUser.Id,
                    FirstName = u.AspNetUser.FirstName,
                    LastName = u.AspNetUser.LastName,
                    Email = u.AspNetUser.Email,
                    MobilePhone = u.AspNetUser.PhoneNumber
                }).ToList();

                response.status = response.status;
                return response;
                
            }
            catch (Exception ex)
            {
                response.status = ResponseErrors.internal_server_error.ToString();
                response.error_messages.Add(ex.Message);
                return response;
            }
        }

        //GET LIST OF TASK ASSGN TO A USER
        [Route("user_tasks/{userId}")]
        [HttpGet]
        public TaskResponses UserTasks(string userId)
        {
            TaskResponses response = new TaskResponses();
            try
            {
                response.Data = _aspnetUserRepository.Get(x => x.Id == userId).TaskUsers.Select(s => new Task_DTO
                {
                    TaskId = s.Task.Id,
                    EncryptedId = s.Task.EncryptedId,
                    Name = s.Task.Name,
                    Description = s.Task.Description,
                    DateCreated = s.Task.DateCreated,
                    DueDate = s.Task.DueDate
                }).ToList();
                
                response.status = response.status;
                return response;
            }
            catch (Exception ex)
            {
                response.status = ResponseErrors.internal_server_error.ToString();
                response.error_messages.Add(ex.Message);
                return response;
            }
        }

        //ADD TASK TO A USER
        [Route("{userId}/task/{taskId:int}")]
        [HttpPut]
        public ResponseMessage AddUserToTask(int taskId, string userId)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                var task = _taskRepository.Get(x => x.Id == taskId);
                var user = _aspnetUserRepository.Get(x => x.Id == userId);
                if (task == null || user == null)
                {
                    response.status = "failed";
                    response.error_messages.Add(ResponseErrors.resource_not_found_at_specified_endpoint.ToString());
                    return response;
                }

                TaskUser userTask = _work.Repository<TaskUser>().Get(x => x.TaskId == taskId && x.UserId == userId);
                if (userTask != null)
                {
                    response.status = "failed";
                    response.error_messages.Add(ResponseErrors.resource_already_exist_at_specified_endpoint.ToString());
                    return response;
                }
                
                //first option using 2NF
                var newTaskUser = new TaskUser() { UserId = userId, TaskId = taskId };
                _taskUserRepository.Add(newTaskUser);
                //_work.Repository<TaskUser>().Add(newTaskUser);

                //second option using 2NF
                //var newTaskUser2 = new TaskUser() { UserId = userId };
                //_task.TaskUsers.Add(newTaskUser2);
                //_taskRepository.Attach(_task);

                response.status = ResponseStatus.success.ToString();
                //response.returned_object = new { id = newTaskUser.Id };
                return response;
            }
            catch (Exception ex)
            {
                response.status = ResponseErrors.internal_server_error.ToString();
                response.error_messages.Add(ex.Message);
                return response;
            }
        }

        //REMOVE TASK ASSIGN TO A USER
        [Route("{userId}/task/{taskId:int}")]
        [HttpDelete]
        public ResponseMessage RemoveUserFromTask(int taskId, string userId)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                var task = _taskRepository.Get(x => x.Id == taskId);
                var user = _aspnetUserRepository.Get(x => x.Id == userId);
                if (task == null || user == null)
                {
                    response.status = "failed";
                    response.error_messages.Add(ResponseErrors.resource_not_found_at_specified_endpoint.ToString());
                    return response;
                }

                TaskUser userTask = _work.Repository<TaskUser>().Get(x => x.TaskId == taskId && x.UserId == userId);
                if (userTask == null)
                {
                    response.status = "failed";
                    response.error_messages.Add(ResponseErrors.resource_not_found_at_specified_endpoint.ToString());
                    return response;
                }

                _taskUserRepository.Remove(userTask);
                //_work.Repository<TaskUser>().Remove(userTask);                
                response.status = ResponseStatus.success.ToString();
                return response;
            }
            catch (Exception ex)
            {
                response.status = ResponseErrors.internal_server_error.ToString();
                response.error_messages.Add(ex.Message);
                return response;
            }
        }


        //Create User Account


    }
}
