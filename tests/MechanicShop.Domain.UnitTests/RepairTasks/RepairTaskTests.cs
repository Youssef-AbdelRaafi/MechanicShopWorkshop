using MechanicShop.Domain.RepairTasks;
using MechanicShop.Domain.RepairTasks.Enums;
using MechanicShop.Domain.RepairTasks.Parts;
using MechanicShop.Tests.Common.RepaireTasks;

using Xunit;

namespace MechanicShop.Domain.UnitTests.RepairTasks;

public class RepairTaskTests
{
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        var id = Guid.NewGuid();
        const string name = "SomeTask";
        const decimal laborCost = 100m;
        const decimal partCost = 50m;
        const int partQuantity = 1;
        const RepairDurationInMinutes estimatedDurationInMin = RepairDurationInMinutes.Min30;
        List<Part> parts = [PartFactory.CreatePart(cost: partCost, quantity: partQuantity).Value];

        const decimal totalCost = (partCost * partQuantity) + laborCost;

        var result = RepairTaskFactory.CreateRepairTask(id: id, name: name, laborCost: laborCost, repairDurationInMinutes: estimatedDurationInMin, parts: parts);

        Assert.True(result.IsSuccess);

        var task = result.Value;

        Assert.Equal(id, task.Id);
        Assert.Equal(name, task.Name);
        Assert.Equal(laborCost, task.LaborCost);
        Assert.Equal(estimatedDurationInMin, task.EstimatedDurationInMins);
        Assert.Single(task.Parts);
        Assert.Equal(totalCost, task.TotalCost);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldFail()
    {
        const string name = " ";

        var result = RepairTaskFactory.CreateRepairTask(name: name);

        Assert.True(result.IsError);

        Assert.Equal(RepairTaskErrors.NameRequired.Code, result.TopError.Code);
    }

    [Fact]
    public void Create_WithInvalidLaborCost_ShouldFail()
    {
        const int laborCost = 0;

        var result = RepairTaskFactory.CreateRepairTask(laborCost: laborCost);

        Assert.True(result.IsError);

        Assert.Equal(RepairTaskErrors.LaborCostInvalid.Code, result.TopError.Code);
    }

    [Fact]
    public void Create_WithInvalidDuration_ShouldFail()
    {
        const int invalidDurationValue = 999;

        var result = RepairTaskFactory.CreateRepairTask(repairDurationInMinutes: (RepairDurationInMinutes)invalidDurationValue);

        Assert.True(result.IsError);

        Assert.Equal(RepairTaskErrors.DurationInvalid.Code, result.TopError.Code);
    }
}