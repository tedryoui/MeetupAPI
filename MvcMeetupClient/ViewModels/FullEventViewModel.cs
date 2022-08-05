namespace MvcMeetupClient.ViewModels;

public class FullEventViewModel
{
    public int Id { get; set; }
    public string MeetupName { get; set; }
    public string Theme { get; set; }
    public string Description { get; set; }
    public string Schedule { get; set; }
    public string Orgonizer { get; set; }
    public string SpeekerName { get; set; }
    public DateTime Time { get; set; }
    public string Location { get; set; }
}