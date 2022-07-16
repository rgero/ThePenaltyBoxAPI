using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PenaltyBox.API.Controllers;
using PenaltyBox.API.Data;
using PenaltyBox.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PenaltyBox.API.Tests
{
    public class PenaltyBoxAPITests
    {
        readonly PenaltiesController controller;
        readonly PenaltyContext context;
        readonly DbContextOptions<PenaltyContext> options;

        public PenaltyBoxAPITests()
        {
            options = new DbContextOptionsBuilder<PenaltyContext>().UseInMemoryDatabase("TestingDatabase").Options;
            context = new PenaltyContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            controller = new PenaltiesController(context);
        }

        [Fact]
        public async void GetNoPenaltiesTest ()
        {
            var result = await controller.GetPenalties();
            var actionResult= Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(0, penaltyList.Count);
        }

        [Fact]
        public async void GetsNumberOfPenaltiesTest()
        {
            // Set Up - Add 2 valid penalties
            Penalty testPenalty = new()
            {
                Player = "Patrice",
                Team = "Bruins",
                Opponent = "Islanders",
                GameDate = new System.DateTime(),
                Home = false,
                PenaltyName = "Hooking",
                Referees = new string[] { "Steve", "Bob" }
            };

            Penalty testPenalty2 = new()
            {
                Player = "Shamed",
                Team = "Avalanche",
                Opponent = "Islanders",
                GameDate = new System.DateTime(),
                Home = false,
                PenaltyName = "Hooking",
                Referees = new string[] { "Steve", "Bob" }
            };

            List<Penalty> testPenalties = new() { testPenalty, testPenalty2 };

            context.AddRange(testPenalties);
            await context.SaveChangesAsync();

            var result = await controller.GetPenalties();
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(2, penaltyList.Count);
        }

        [Fact]
        public async void PostValidPenaltyTest ()
        {
            Penalty testPenalty = new()
            {
                Player = "Patrice",
                Team = "Bruins",
                Opponent = "Islanders",
                GameDate = new System.DateTime(),
                Home = false,
                PenaltyName = "Hooking",
                Referees = new string[] { "Steve", "Bob" }
            };

            var result = await controller.PostPenalty(testPenalty);
            var actionResult = Assert.IsType<ActionResult<Penalty>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Penalty returnedPenalty = Assert.IsType<Penalty>(createdResult.Value);

            Assert.Equal(testPenalty.Player, returnedPenalty.Player);
            Assert.Equal(testPenalty.Team, returnedPenalty.Team);
            Assert.Equal(testPenalty.GameDate, returnedPenalty.GameDate);
            Assert.Equal(testPenalty.Home, returnedPenalty.Home);
            Assert.Equal(testPenalty.PenaltyName, returnedPenalty.PenaltyName);
            Assert.Equal(testPenalty.Referees, returnedPenalty.Referees);
            Assert.Equal(testPenalty.Opponent, returnedPenalty.Opponent);
        }

        [Fact]
        public void PostPenalty_Should_Throw_When_Not_Valid_Penalty()
        {
            Penalty testPenalty = new()
            {
                Player = "Patrice",
                Team = "Bruins",
                Opponent = "Islanders",
                GameDate = new System.DateTime(),
                Home = false,
                Referees = new string[] { "Steve", "Bob" }
            };

            var exception = Record.Exception(() => controller.PostPenalty(testPenalty).Result);

            Assert.NotNull(exception);
            Assert.IsType<AggregateException>(exception);
        }

        [Fact]
        public async void DeletePenaltyTest()
        {
            Penalty testPenalty = new()
            {
                Player = "Patrice",
                Team = "Bruins",
                Opponent = "Islanders",
                GameDate = new System.DateTime(),
                Home = false,
                PenaltyName = "Hooking",
                Referees = new string[] { "Steve", "Bob" }
            };

            context.Add(testPenalty);
            await context.SaveChangesAsync();

            List<Penalty> penaltyList = await context.Penalties.ToListAsync<Penalty>();
            Assert.Equal(1, penaltyList.Count);

            int targetID = 1;
            var result = await controller.DeletePenalty(targetID);

            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);

            penaltyList = await context.Penalties.ToListAsync<Penalty>();
            Assert.Equal(0, penaltyList.Count);
        }

        [Fact]
        public async void NoDeleteInvalidPenaltyTest()
        {
            int targetID = 2;
            var result = await controller.DeletePenalty(targetID);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);

        }
    }
}