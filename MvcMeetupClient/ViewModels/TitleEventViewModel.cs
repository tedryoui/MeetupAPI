namespace MvcMeetupClient.ViewModels;

public class TitleEventViewModel
{
    public string MeetupName { get; set; }
    public string Theme { get; set; }
    public UserViewModel Orgonizer { get; set; }
    public string Speeker { get; set; }
    public DateTime Time { get; set; }
    public string Location { get; set; }
}