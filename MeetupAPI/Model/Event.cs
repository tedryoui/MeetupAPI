using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using IdentityServer.DbContext;

namespace MeetupAPI.Model;

public class Event
{
    public int Id { get; set; }
    public string MeetupName { get; set; }
    public Theme Theme { get; set; }
    public string Description { get; set; }
    public string Schedule { get; set; }
    public string OrgonizerEmail { get; set; }
    public string SpeekerName { get; set; }
    
    public DateTime Time { get; set; }
    public string Location { get; set; }
}