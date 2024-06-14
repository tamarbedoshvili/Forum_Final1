using Final.database;
using Final.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class PostStatusChecker : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private Timer _timer;

    public PostStatusChecker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(CheckPostStatus, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        return Task.CompletedTask;
    }

    private void CheckPostStatus(object state)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            var xDays = 30;
            var now = DateTime.Now;

            var postsToUpdate = context.Posts
              .Where(p => p.Status != EStatus.Inactive)
                .Include(p => p.Comments)
                .ToList().Where(x=>x.Comments.Count>0)
                .Where(p => p.Comments.All(x => (DateTime.Now - x.CreateDate).TotalDays > xDays))
                .ToList();

            foreach (var post in postsToUpdate)
            {
                post.Status = EStatus.Inactive;
            }

            context.SaveChanges();
        }
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