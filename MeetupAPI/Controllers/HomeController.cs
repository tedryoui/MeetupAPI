using System.Text.Json;
using IdentityServer.DbContext;
using MeetupAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppDbContext = MeetupAPI.DbContext.AppDbContext;

namespace MeetupAPI;

[Authorize(Policy = "ApiAuthenticated")]
[Route("api")]
public class HomeController : ControllerBase
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db)
    {
        _db = db;
    }
    
    [HttpGet("getEvents")]
    public JsonResult GetEvents()
    {
        HttpContext.Response.Headers.ContentType = "application/json";
        HttpContext.Response.StatusCode = 200;
        
        var queryString = HttpContext.Request.Query;

        if (queryString.ContainsKey("org"))
        {
            var org = queryString["org"].ToString();

            return new JsonResult(_db.Events
                .Include(x => x.Theme)
                .Where(x => x.OrgonizerEmail == org)
                .ToList());
        }

        return new JsonResult(_db.Events.Include(x => x.Theme).ToList());
    }
    
    [HttpGet("getEvent")]
    public JsonResult GetEvent()
    {
        var queryStrings = HttpContext.Request.Query;

        if (queryStrings.ContainsKey("id"))
        {
            var id = queryStrings["id"].ToString();

            if (_db.Events.Any(x => x.Id.ToString().Equals(id)))
            {
                HttpContext.Response.StatusCode = 200;
                return new JsonResult(
                    _db.Events
                        .Include(x => x.Theme)
                        .First(x => x.Id.ToString().Equals(id)));
            }
        }

        HttpContext.Response.StatusCode = 404;
        return new JsonResult(null);
    }

    [HttpPost("addEvent")]
    public void AddEvent([FromBody] Event eventObject)
    {
        CheckForThemes(ref eventObject);

        _db.Events.AddAsync(eventObject);
        _db.SaveChanges();
    }
    
    [HttpPut("updateEvent")]
    public void UpdateEvent([FromBody] Event eventObject)
    {
        CheckForThemes(ref eventObject);
        
        _db.Events.Update(eventObject);
        _db.SaveChanges();
    }
    
    [HttpDelete("deleteEvent")]
    public void DeleteEvent()
    {
        var queryString = HttpContext.Request.Query;

        if (queryString.ContainsKey("id"))
        {
            var id = queryString["id"].ToString();
            
            var events = _db.Events.ToList();

            if (events.Any(x => x.Id.ToString().Equals(id)))
            {
                Event rEvent = events.Where( x => x.Id.ToString().Equals(id) ).First();

                _db.Events.Remove(rEvent);
                _db.SaveChanges();
            }
        }
    }

    public void CheckForThemes(ref Event old)
    {
        Theme completeTheme = old.Theme;

        if (_db.Themes.Any(x => x.Value == completeTheme.Value))
        {
            completeTheme = _db.Themes.First(x => x.Value == completeTheme.Value);
        }
        else
        {
            _db.Themes.Add(completeTheme);
            _db.SaveChanges();
            completeTheme = _db.Themes.Where(x => x.Value == completeTheme.Value).First();
        }

        old.Theme = completeTheme;
    }
}