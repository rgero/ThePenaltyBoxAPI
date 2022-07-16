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
        PenaltiesController controller;
        PenaltyContext context;
        DbContextOptions<PenaltyContext> options;

        public PenaltyBoxAPITests()
        {
            options = new DbContextOptionsBuilder<PenaltyContext>().UseInMemoryDatabase("TestingDatabase").Options;
            context = new PenaltyContext(options);
            controller = new PenaltiesController(context);
        }

        [Fact]
        public void GetNoPenaltiesTest ()
        {
            context.Database.EnsureDeletedAsync();
            var result = controller.GetPenalties();
            var task = Assert.IsType<Task<ActionResult<IEnumerable<Penalty>>>>(result);
            var actionResult= Assert.IsType<ActionResult<IEnumerable<Penalty>>>(task.Result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(0, penaltyList.Count());
        }

        [Fact]
        public void PostValidPenaltyTest ()
        {
            Penalty testPenalty = new Penalty()
            {
                Player = "Patrice",
                Team = "Bruins",
                Opponent = "Islanders",
                GameDate = new System.DateTime(),
                Home = false,
                PenaltyName = "Hooking",
                Referees = new string[] { "Steve", "Bob" }
            };

            var result = controller.PostPenalty(testPenalty);
            var task = Assert.IsType<Task<ActionResult<Penalty>>>(result);
            var actionResult = Assert.IsType<ActionResult<Penalty>>(task.Result);
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
            Penalty testPenalty = new Penalty()
            {
                Player = "Patrice",
                Team = "Bruins",
                Opponent = "Islanders",
                GameDate = new System.DateTime(),
                Home = false,
                Referees = new string[] { "Steve", "Bob" }
            };

            controller.ModelState.AddModelError("PenaltyNameMissing", "No Penalty name defined.");
            var result = controller.PostPenalty(testPenalty);
            var task = Assert.IsType<Task<ActionResult<Penalty>>>(result);

            var exception = Record.Exception(() => task.Result);

            Assert.NotNull(exception);
            Assert.IsType<AggregateException>(exception);
        }

    }
}