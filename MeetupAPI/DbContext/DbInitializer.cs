using MeetupAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace MeetupAPI.DbContext;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider scope)
    {
        var set = scope.GetService<DbSet<Event>>();
    }
}