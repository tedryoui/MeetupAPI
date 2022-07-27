using System.Diagnostics.CodeAnalysis;

namespace MeetupAPI.Model;

public class Event
{
    public int Id { get; set; }
    
    [NotNull]
    public string MeetupName { get; set; }
    
    [NotNull]
    public string Theme { get; set; }
    
    [AllowNull]
    public string Description { get; set; }
    
    [AllowNull]
    public string Schedule { get; set; }
    
    [NotNull]
    public string Orgonizer { get; set; }
    
    [AllowNull]
    public string Speeker { get; set; }
    
    [NotNull]
    public DateTime Time { get; set; }
    
    [NotNull]
    public string Location { get; set; }
}