using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PenaltyBox.API.Controllers;
using PenaltyBox.API.Data;
using PenaltyBox.API.Models;
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
        public async void GetNoPenaltiesTest ()
        {
            var result = await controller.GetPenalties();
            var actionResult= Assert.IsType<ActionResult<IEnumerable<Penalty>>>(result);

            List<Penalty> penaltyList = Assert.IsType<List<Penalty>>(actionResult.Value);
            Assert.Equal(0, penaltyList.Count());
        }
    }
}