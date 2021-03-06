﻿
using ArcadiaWebForm.Models;
using AutoMapper;
using Xunit;

namespace ArcadiaWebForm.Tests
{
    public class DescribeAutomapperProfile
    {
        [Fact]
        public void ItShouldMapBetweenInputAndOutput()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile<AutomapperProfile>();
            });

            var input = new Models.Opportunity.OpportunityInputModel { Id = "input_id", Description = "desc", SelectedClient = "1", Title = "title" };
            var output = Mapper.Map<Models.Opportunity.OpportunityInputModel, Models.Opportunity.OpportunityOutputModel>(input);
            Assert.Equal(input.Title, output.Title);            
            Assert.True(output.Client != null);
            Assert.Equal(input.SelectedClient, output.Client.Values[0]);
        }
    }
}
