namespace MvcMeetupClient.ViewModels;

public class EventsViewModel
{
    public EventsViewModel()
    {
        events = new List<EventViewModel>();
    }
    
    public List<EventViewModel> events;
}