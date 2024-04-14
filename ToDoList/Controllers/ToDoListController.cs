using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;
using ToDoList.Repositories;

namespace ToDoList.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly ToDoListRepo taskRepo = new ToDoListRepo();
        
        public ActionResult Index()
        {
            string userID = HttpContext.Session["UserID"]?.ToString();
            ViewBag.PageMode = string.IsNullOrEmpty(userID) ? 0 : 1;
            List<Task> taskList = taskRepo.GetAllTasks();
            if (!string.IsNullOrEmpty(userID))
                taskList=taskList.Where(x => x.UserID == Convert.ToInt32(userID)).ToList();
            return View(taskList);
        }

        [HttpPost]
        public ActionResult AddorUpdateTask(Task task)
        {
            string message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    task.UserID = Convert.ToInt32(HttpContext.Session["UserID"]);
                    taskRepo.AddorUpdateTask(task, ref message);
                    return Json(new { success = true, taskID = task.TaskID, message });
                }
                else
                {
                    message = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return Json(new { success = false, message });
                }
            }
            catch(Exception ex)
            {
                message = "Exception :: " + ex.Message;
                return Json(new { success = false, message });
            }
        }
        
        public ActionResult Delete(int taskID)
        {
            taskRepo.DeleteTask(taskID);
            return RedirectToAction("Index");
        }
        
        public ActionResult updateStatus(int taskID, bool status)
        {
            taskRepo.ToggleTaskStatus(taskID, status);
            return Json(new { success = true });
        }

    }
}