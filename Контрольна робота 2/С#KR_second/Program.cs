using System;
using System.Collections.Generic;
using System.Linq;

public class FootballTeam
{
    public string Title { get; set; }
    public string City { get; set; }
    public int FoundationYear { get; set; }

    public FootballTeam(string title, string city, int foundationYear)
    {
        Title = title;
        City = city;
        FoundationYear = foundationYear;
    }
}

public class Tournament
{
    public string Title { get; set; }
    public int Year { get; set; }
    public FootballTeam Champion { get; set; }
    public List<FootballTeam> Participants { get; set; }

    public Tournament(string title, int year, FootballTeam champion, List<FootballTeam> participants)
    {
        Title = title;
        Year = year;
        Champion = champion;
        Participants = participants;
    }
}

class Program
{
    static void Main()
    {
        var team1 = new FootballTeam("Team A", "Kyiv", 1923);
        var team2 = new FootballTeam("Team B", "Lviv", 1945);
        var team3 = new FootballTeam("Team C", "Odesa", 1950);
        var team4 = new FootballTeam("Team D", "Dnipro", 1960);

        var tournaments = new List<Tournament>
        {
            new Tournament("Cup A", 2020, team1, new List<FootballTeam> { team1, team2, team3 }),
            new Tournament("Cup B", 2022, team2, new List<FootballTeam> { team2, team3 }),
            new Tournament("Cup C", 2021, team3, new List<FootballTeam> { team1, team2, team3, team4 })
        };

        // 1. Турнір з найбільшою кількістю учасників
        var maxParticipantsTournament = tournaments
            .OrderByDescending(t => t.Participants.Count)
            .FirstOrDefault();

        Console.WriteLine("Турнір з найбільшою кількістю учасників:");
        Console.WriteLine($"{maxParticipantsTournament?.Title} ({maxParticipantsTournament?.Year}) - {maxParticipantsTournament?.Participants.Count} учасників");

        // 2. Переможці турнірів, посортовані за назвою турніру
        Console.WriteLine("\nПереможці турнірів:");
        var winners = tournaments
            .OrderBy(t => t.Title)
            .Select(t => new { t.Title, t.Year, Champion = t.Champion.Title });

        foreach (var w in winners)
        {
            Console.WriteLine($"{w.Title} ({w.Year}): {w.Champion}");
        }

        // 3. Турніри в заданому періоді [year1, year2]
        int year1 = 2020, year2 = 2021;

        Console.WriteLine($"\nТурніри у період [{year1}, {year2}]:");
        var filteredTournaments = tournaments
            .Where(t => t.Year >= year1 && t.Year <= year2)
            .Select(t => new { t.Title, t.Year });

        foreach (var t in filteredTournaments)
        {
            Console.WriteLine($"{t.Title} ({t.Year})");
        }
    }
}