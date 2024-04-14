using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToDoList.Models;

namespace ToDoList.Repositories
{
    public class ToDoListRepo
    {
        private ToDoListEntities _entities = new ToDoListEntities();
        public List<Task> GetAllTasks()
        {
            return _entities.Tasks.ToList();
        }

        public bool AddorUpdateTask(Task task, ref string message)
        {
            if (task.TaskID == 0)
            {
                _entities.Tasks.Add(task);
                message = "Task Added Successfully";
            }
            else
            {
                Task prevTask = _entities.Tasks.FirstOrDefault(x => x.TaskID == task.TaskID);
                if (prevTask == null)
                {
                    message = "Unable to update task. Task not found";
                    return false;
                }
                else
                {
                    prevTask.TaskDescription = task.TaskDescription;
                    prevTask.ToDoDate = task.ToDoDate;
                    message = "Task updated successfully";
                }
            }
            _entities.SaveChanges();
            return true;
        }

        public bool DeleteTask(int taskID)
        {
            Task prevTask = _entities.Tasks.FirstOrDefault(x => x.TaskID == taskID);
            if (prevTask == null)
            {
                return false;
            }
            else
            {
                _entities.Tasks.Remove(prevTask);
                _entities.SaveChanges();
                return true;
            }
        }

        public void ToggleTaskStatus(int taskID, bool status)
        {
            Task prevTask = _entities.Tasks.FirstOrDefault(x => x.TaskID == taskID);
            prevTask.Status=status;
            _entities.SaveChanges();
        }
    }
}