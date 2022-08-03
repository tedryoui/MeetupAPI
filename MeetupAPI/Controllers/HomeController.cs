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
        var queryString = HttpContext.Request.Query;

        if (!queryString.ContainsKey("amount"))
        {
            HttpContext.Response.StatusCode = 404;
            return new JsonResult(null);
        }

        int amount, page; 
        var pageString = (queryString.ContainsKey("page")) ? queryString["page"].ToString() : "0";

        if (Int32.TryParse(queryString["amount"].ToString(), out amount) &&
            Int32.TryParse(pageString, out page))
        {
            if (queryString.ContainsKey("org"))
                return new JsonResult(
                    _db.Events.Include(x => x.Theme).Where(x => x.OrgonizerEmail == queryString["org"].ToString())
                        .Skip((page - 1) * amount).Take(amount).ToList());
            else
                return new JsonResult(
                    _db.Events.Include(x => x.Theme).Skip((page - 1) * amount).Take(amount).ToList());
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
        } else if (queryStrings.ContainsKey("name"))
        {
            var name = queryStrings["name"].ToString();

            if (_db.Events.Any(x => x.MeetupName.Contains(name)))
            {
                HttpContext.Response.StatusCode = 200;
                return new JsonResult(
                    _db.Events
                        .Include(x => x.Theme)
                        .First(x => x.MeetupName.Contains(name)));
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