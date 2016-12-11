using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zadatak1.Interfaces;
using Zadatak1.Models;
using Zadatak2.Models;
using Zadatak2.Models.TodoViewModels;

namespace Zadatak2.Controllers
{

    [Authorize]
    public class TodoController : Controller
    {


        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        // Inject user manager into constructor
        public TodoController(ITodoRepository repository,
        UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await
            _userManager.GetUserAsync(HttpContext.User);
            return View(_repository.GetActive(new Guid(currentUser.Id)));
        }


        
        public ActionResult Add()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel m)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await
           _userManager.GetUserAsync(HttpContext.User);
                _repository.Add(new Zadatak1.Models.TodoItem(m.Text, new Guid(currentUser.Id)));
                return RedirectToAction("Index");
            }
            return View(m);
        }

        
        public async Task<IActionResult> Completed()
        {
            ApplicationUser currentUser = await
            _userManager.GetUserAsync(HttpContext.User);
            return View(_repository.GetCompleted(new Guid(currentUser.Id)));
        }

        
        [HttpGet("{guid}")]
        public async Task<IActionResult> MarkCompleted(string guid)
        {
            if (guid != null)
            {
                ApplicationUser currentUser = await
                _userManager.GetUserAsync(HttpContext.User);
                _repository.MarkAsCompleted(new Guid(guid), new Guid(currentUser.Id));
            }
            return RedirectToAction("Index");
        }
    }
}
