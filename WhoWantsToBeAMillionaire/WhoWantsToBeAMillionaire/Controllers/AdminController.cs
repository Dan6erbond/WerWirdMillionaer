using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Admin;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Users;

namespace WhoWantsToBeAMillionaire.Controllers
{
    
    [Authorize]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DataManager _dataManager;
        private readonly UserManager _userManager;

        public AdminController(IHttpContextAccessor httpContextAccessor, DataManager dataManager, UserManager userManager)
        {
            _httpContextAccessor = httpContextAccessor;

            _dataManager = dataManager;
            _userManager = userManager;
        }

        [HttpPost("questions/edit")]
        public IActionResult EditQuestion([FromBody] QuizQuestion question)
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.GetUser(username);
            
            //TODO: Return error if user isn't administrator
            
            _dataManager.EditQuestion(question);
            return Ok();
        }
        
        [HttpPost("questions/add")]
        public IActionResult AddQuestion([FromBody] QuizQuestion question)
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.GetUser(username);
            
            //TODO: Return error if user isn't administrator
            
            var newQuestion = _dataManager.AddQuestion(question);
            return Ok(newQuestion);
        }
        
        [HttpGet("questions/delete/{id}")]
        public IActionResult DeleteQuestion(int id)
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.GetUser(username);
            
            //TODO: Return error if user isn't administrator
            
            _dataManager.DeleteQuestion(id);
            return Ok();
        }
        
        [HttpPost("categories/add")]
        public IActionResult AddCategory(Category category)
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.GetUser(username);
            
            //TODO: Return error if user isn't administrator
            
            var newCategory = _dataManager.AddCategory(category);
            return Ok(newCategory);
        }
        
        [HttpGet("categories/delete/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.GetUser(username);
            
            //TODO: Return error if user isn't administrator
            
            _dataManager.DeleteCategory(id);
            return Ok();
        }
    }
}