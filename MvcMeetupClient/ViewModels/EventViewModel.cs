namespace MvcMeetupClient.ViewModels;

public class EventViewModel
{
#pragma warning disable CS8618
    public int Id { get; set; }
    public string MeetupName { get; set; }
    public string Theme { get; set; }
    public string Description { get; set; }
    public List<string> Schedule { get; set; }
    public string Orgonizer { get; set; }
    public string Speeker { get; set; }
    public DateTime Time { get; set; }
    public string Location { get; set; }
#pragma warning restore CS8618

}