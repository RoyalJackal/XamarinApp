using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Shared.Dto;

namespace Server.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public NotificationController(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpGet("GetNotifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Unauthorized();

            var notifiers = _dbContext.Notifications
                .Include(x => x.User)
                .Where(x => x.User.Id == user.Id)
                .Where(x => x.DateTime > DateTimeOffset.UtcNow && !x.IsSend)
                .Select(x => new NotificationDto
                {
                    Id = x.Id,
                    Text = x.Text,
                    DateTime = x.DateTime
                })
                .ToList();

            return Ok(notifiers);
        }

        [HttpPost("AddNotification")]
        public async Task<IActionResult> AddNotification([FromBody] NotificationDto notificationDto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Unauthorized();

            await _dbContext.Notifications
                .AddAsync(new Notification
                {
                    Text = notificationDto.Text,
                    User = user,
                    DateTime = notificationDto.DateTime.ToUniversalTime(),
                });
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("AddToken")]
        public async Task<IActionResult> AddToken([FromBody] FirebaseToken firebaseToken)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Unauthorized();

            var foundToken = await _dbContext.FirebaseTokens
                .FirstOrDefaultAsync(x => x.Token == firebaseToken.Token);
            if (foundToken != null) return Ok();

            await _dbContext.FirebaseTokens
                .AddAsync(new FirebaseToken
                {
                    Token = firebaseToken.Token,
                    User = user
                });
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("RemoveToken")]
        public async Task<IActionResult> RemoveToken([FromBody] FirebaseToken firebaseToken)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Unauthorized();

            var foundToken = await _dbContext.FirebaseTokens
                .FirstOrDefaultAsync(x => x.Token == firebaseToken.Token);
            if (foundToken == null) return BadRequest();

            _dbContext.FirebaseTokens.Remove(foundToken);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}