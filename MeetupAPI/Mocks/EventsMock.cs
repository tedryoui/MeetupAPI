using System.Text.Json;
using MeetupAPI.Model;

namespace MvcMeetupClient.Mocks;

public static class EventsMock
{
    public static List<Event> Get() =>
        new List<Event>
        {
            new Event()
            {
                Id = 1,
                MeetupName = "Inovations of 2017 y.",
                Theme = "Science",
                Description = "some description \n in few lines",
                Schedule = JsonSerializer.Serialize(new List<string>()
                {
                    "fees",
                    "greetings",
                    "microsoft stand",
                    "google stand",
                    "amazon stand",
                    "the ending"
                }),
                Orgonizer = "admin",
                Speeker = "Boris Yakovlev",
                Time = new DateTime(
                    year: 2017,
                    month: 12,
                    day: 12,
                    hour: 12,
                    minute: 12,
                    second: 0),
                Location = "England, London"
            },
            new Event()
            {
                Id = 2,
                MeetupName = "Birthday party",
                Theme = "Entertainment",
                Description = "Polina Sozontzova`s pary for her 18th birthday",
                Schedule = JsonSerializer.Serialize(new List<string>()),
                Orgonizer = "admin",
                Speeker = "Polina Sozontzova",
                Time = new DateTime(
                    year: 2020,
                    month: 07,
                    day: 24,
                    hour: 18,
                    minute: 00,
                    second: 00),
                Location = "pros. Mira, 24"
            },
            new Event()
            {
                Id = 3,
                MeetupName = "Brazil Carnival",
                Theme = "Entertainment",
                Description =
                    "The Carnival of Brazil is an annual Brazilian festival held the Friday afternoon before Ash Wednesday at noon",
                Schedule = JsonSerializer.Serialize(new List<string>()
                {
                    "Imperio Serrano",
                    "Grande Rio",
                    "Mosidade",
                    "Unidos Da Tijuca"
                }),
                Orgonizer = "admin",
                Speeker = "Porto Seguro",
                Time = new DateTime(
                    year: 2020,
                    month: 02,
                    day: 12,
                    hour: 00,
                    minute: 00,
                    second: 00),
                Location = "Brazil"
            }
        };
}