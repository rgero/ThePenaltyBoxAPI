using Microsoft.AspNetCore.Mvc;
using PenaltyBox.API.Controllers;
using PenaltyBox.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PenaltyBox.API.Tests
{
    public class APIFilterTests : IClassFixture<PenaltySeedDataFixture>
    {
        struct InputParams
        {
            public InputParams()
            {
            }

            public string? playerName = null;
            public string? teamName = null;
            public string? startDate = null;
            public string? endDate = null;
            public string? opponentName = null;
            public string? penaltyName = null;
            public string? home = null;
            public string? referees = null;
        };

        readonly PenaltiesController controller;
        InputParams inputParams;

        public APIFilterTests(PenaltySeedDataFixture fixture)
        {
            controller = new PenaltiesController(fixture.Context);
            this.inputParams = new InputParams();
        }

        ActionResult<IEnumerable<Penalty>> GetActionResult(InputParams inputParams)
        {
            return controller.GetFilteredPenalty(inputParams.playerName,
                                                 inputParams.teamName, 
                                                 inputParams.startDate, 
                                                 inputParams.endDate, 
                                                 inputParams.opponentName, 
                                                 inputParams.penaltyName,
                                                 inputParams.home,
                                                 inputParams.referees).Result;
        }

        [Fact]
        public void NoFilterGetsAllTest()
        {
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(5, penaltyList.Count);
        }

        [Fact]
        public void Filter_PlayerName_ValidTest()
        {
            inputParams.playerName = "Shamed";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(2, penaltyList.Count);
        }

        [Fact]
        public void Filter_PlayerName_Multiple()
        {
            inputParams.playerName = "Shamed,Patrice";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(3, penaltyList.Count);
        }

        [Fact]
        public void Filter_PlayerName_InvalidTest()
        {
            inputParams.playerName = "abc123";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Empty(penaltyList);
        }

        [Fact]
        public void Filter_TeamName_ValidTest()
        {
            inputParams.teamName = "Bruins";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Single(penaltyList);
        }

        [Fact]
        public void Filter_TeamName_MultipleTest()
        {
            inputParams.teamName = "Bruins,Leafs";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(2,penaltyList.Count);
        }

        [Fact]
        public void Filter_TeamName_InvalidTest()
        {
            inputParams.playerName = "abc123";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Empty(penaltyList);
        }

        [Fact]
        public void Filter_Penalty_Valid()
        {
            inputParams.penaltyName = "Hooking";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(3, penaltyList.Count);
        }

        [Fact]
        public void Filter_Penalty_Multiple()
        {
            inputParams.penaltyName = "Tripping,Fighting";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(2, penaltyList.Count);
        }

        [Fact]
        public void Filter_Opponent_Multiple()
        {
            inputParams.opponentName = "Leafs,Bruins";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(2, penaltyList.Count);
        }

        [Fact]
        public void Filter_Opponent_Valid()
        {
            inputParams.opponentName = "Islanders";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(3, penaltyList.Count);
        }

        [Fact]
        public void Filter_Multiple_PlayerTeam()
        {
            inputParams.penaltyName = "Hooking";
            inputParams.teamName = "Avalanche";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(2, penaltyList.Count);
        }

        [Fact]
        public void Filter_Multiple_PenaltyTeamPlayer()
        {
            inputParams.penaltyName = "Hooking";
            inputParams.teamName = "Avalanche";
            inputParams.playerName = "Roger";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Single(penaltyList);
        }

        [Fact]
        public void Filter_Home_IsHome()
        {
            inputParams.home = "true";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(2, penaltyList.Count);
        }

        [Fact]
        public void Filter_Home_IsAway()
        {
            inputParams.home = "false";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(3, penaltyList.Count);
        }

        [Fact]
        public void Filter_Home_IsInvalid()
        {
            // This will get all the penalties since it's invalid
            inputParams.home = "Shiny";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(5, penaltyList.Count);
        }

        [Fact]
        public void Filter_Date_Start()
        {
            inputParams.startDate = "2022-06-21";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(3, penaltyList.Count);
        }

        [Fact]
        public void Filter_Date_End()
        {
            inputParams.endDate = "2022-06-21";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(2, penaltyList.Count);
        }

        [Fact]
        public void Filter_Date_Both()
        {
            inputParams.startDate = "2022-06-21";
            inputParams.endDate = "2022-07-03";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(2, penaltyList.Count);
        }

        [Fact]
        public void Filter_Referee_Single()
        {
            inputParams.referees = "Ralph";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Single(penaltyList);
        }

        [Fact]
        public void Filter_Referee_Multiple()
        {
            inputParams.referees = "Steve,Bob";
            var result = GetActionResult(inputParams);
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(4, penaltyList.Count);
        }



    }
}
