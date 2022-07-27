using System.Text.Json;
using IdentityServer.DbContext;
using MeetupAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    
    /*[HttpGet]
    [Route("getEvents")]
    public string GetEvents()
    {
        return JsonSerializer.Serialize(_db.Events.ToList());
    }*/
    
    [HttpGet]
    [Route("getEvents")]
    public string GetEvents(string org)
    {
        if(string.IsNullOrEmpty(org))
            return JsonSerializer.Serialize(_db.Events.ToList());
        else
            return JsonSerializer.Serialize(_db.Events.Where(x => x.Orgonizer == org).ToList());
    }
    
    [HttpGet]
    [Route("getEvent")]
    public string GetEvent()
    {
        var queryStrings = HttpContext.Request.Query;

        if (queryStrings.Any(x => x.Key == "id"))
        {
            var id = queryStrings.First(x => x.Key == "id").Value;

            if (_db.Events.Any(x => x.Id.ToString().Equals(id)))
            {
                return JsonSerializer.Serialize(
                    _db.Events.First(x => x.Id.ToString().Equals(id)));
            }
        }

        Event e = null;
        return JsonSerializer.Serialize(e);
    }

    [HttpPost]
    [Route("addEvent")]
    public void AddEvent([FromBody] Event eventObject)
    {
        _db.Events.AddAsync(eventObject);
        _db.SaveChanges();
    }
    
    [HttpPut]
    [Route("updateEvent")]
    public void UpdateEvent([FromBody] Event eventObject)
    {
        var prev = _db.Events.First(x => x.Id == eventObject.Id);
        
        prev.MeetupName = eventObject.MeetupName;
        prev.Theme = eventObject.Theme;
        prev.Description = eventObject.Description;
        prev.Orgonizer = eventObject.Orgonizer;
        prev.Speeker = eventObject.Speeker;
        prev.Time = eventObject.Time;
        prev.Location = eventObject.Location;
        
        _db.Events.Update(prev);
        _db.SaveChanges();
    }
    
    [HttpDelete]
    [Route("deleteEvent")]
    public void DeleteEvent(string eventId)
    {
        var events = _db.Events.ToList();

        if (events.Any(x => x.Id.ToString().Equals(eventId)))
        {
            Event rEvent = events.Where( x => x.Id.ToString().Equals(eventId) ).First();

            _db.Events.Remove(rEvent);
            _db.SaveChanges();
        }
        
    }
}