using Microsoft.EntityFrameworkCore;
using PenaltyBox.API.Data;
using PenaltyBox.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenaltyBox.API.Tests
{
    public class PenaltySeedDataFixture : IDisposable
    {
        public PenaltyContext Context { get; private set; }

        public PenaltySeedDataFixture()
        {
            DbContextOptions<PenaltyContext> options = new DbContextOptionsBuilder<PenaltyContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            Context = new PenaltyContext(options);

            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();

            List<Penalty> testPenalties = new()
            {
                new Penalty()
                {
                    Player = "Patrice",
                    Team = "Bruins",
                    Opponent = "Islanders",
                    GameDate = DateTime.Parse("2022-07-01"),
                    Home = false,
                    PenaltyName = "Fighting",
                    Referees = new string[] { "Steve", "Bob" },
                    SeasonType = SeasonType.Playoffs
                },
                new Penalty()
                {
                    Player = "Roger",
                    Team = "Avalanche",
                    Opponent = "Islanders",
                    GameDate = DateTime.Parse("2022-07-05"),
                    Home = true,
                    PenaltyName = "Hooking",
                    Referees = new string[] { "Steve", "Bob" },
                    SeasonType = SeasonType.Regular
                },
                new Penalty()
                {
                    Player = "George",
                    Team = "Leafs",
                    Opponent = "Islanders",
                    GameDate = DateTime.Parse("2022-06-22"),
                    Home = false,
                    PenaltyName = "Hooking",
                    Referees = new string[] { "Ralph", "George" }
                },
                new Penalty()
                {
                    Player = "Shamed",
                    Team = "Avalanche",
                    Opponent = "Leafs",
                    GameDate = DateTime.Parse("2022-06-15"),
                    Home = true,
                    PenaltyName = "Hooking",
                    Referees = new string[] { "Steve", "Bob" }
                },
                new Penalty()
                {
                    Player = "Shamed",
                    Team = "Avalanche",
                    Opponent = "Bruins",
                    GameDate = DateTime.Parse("2022-02-22"),
                    Home = false,
                    PenaltyName = "Tripping",
                    Referees = new string[] { "Steve", "Bob" }
                }
            };

            Context.AddRange(testPenalties);
            Context.SaveChanges();
        }


        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
