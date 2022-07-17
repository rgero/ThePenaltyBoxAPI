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
    public class PenaltyBoxAPITests : IClassFixture<PenaltySeedDataFixture>
    {
        readonly PenaltySeedDataFixture fixture;
        readonly PenaltiesController controller;

        public PenaltyBoxAPITests(PenaltySeedDataFixture fixture)
        {
            this.fixture = fixture;
            controller = new PenaltiesController(fixture.Context);
        }

        [Fact]
        public void GetsNumberOfPenaltiesTest()
        {
            var result = controller.GetPenalties().Result;
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(5, penaltyList.Count);
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
        public void DeletePenaltyTest()
        {
            int targetID = 1;
            var result = controller.DeletePenalty(targetID).Result;

            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);

            List<Penalty> penaltyList = this.fixture.Context.Penalties.ToList<Penalty>();
            Assert.Equal(4, penaltyList.Count);
        }

        [Fact]
        public async void NoDeleteInvalidPenaltyTest()
        {
            int targetID = 22;
            var result = await controller.DeletePenalty(targetID);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);

        }
    }
}