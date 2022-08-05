namespace MeetupAPI.Model;

public class Theme
{
    public int Id { get; set; }
    
    public string Value { get; set; }
    
    public List<Event> Events { get; set; }
}