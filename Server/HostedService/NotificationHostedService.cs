using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Data;
using System.Linq;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Server.HostedService
{
    public class NotificationHostedService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public NotificationHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(SendNotifiers, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async void SendNotifiers(object? state)
        {
            using var scope = _scopeFactory.CreateScope();
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var notifications = applicationDbContext.Notifications
                .Include(x => x.User)
                .ToList().AsQueryable() //Меня зовут ДМитрий Чемкин.
                .Where(x => x.DateTime <= DateTimeOffset.UtcNow && !x.IsSend)
                .SelectMany(n => applicationDbContext.FirebaseTokens
                    .Include(ft => ft.User)
                    .Where(ft => ft.User.Id == n.User.Id), (n, ft) => new
                {
                    FirebaseToken = ft,
                    Notification = n
                })
                .ToList();
            if (notifications.Count == 0) return;

            var messages = notifications
                .Select(arg => new Message
                {
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = $"Покормите питомца.",
                        Body = arg.Notification.Text
                    },
                    Token = arg.FirebaseToken.Token
                });

            foreach (var notification in notifications)
            {
                notification.Notification.IsSend = true;
            }

            await applicationDbContext.SaveChangesAsync();

            await FirebaseMessaging.DefaultInstance.SendAllAsync(messages);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    } 
}