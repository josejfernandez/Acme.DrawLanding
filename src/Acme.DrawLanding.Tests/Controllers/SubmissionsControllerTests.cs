using System.Text;
using System.Text.Json;
using Acme.DrawLanding.Library.Domain.SerialNumbers;
using Acme.DrawLanding.Library.Domain.Submissions;
using Acme.DrawLanding.Tests.Mocking;
using Acme.DrawLanding.Tests.Mocking.Submissions;
using Acme.DrawLanding.Website.Controllers;
using Acme.DrawLanding.Website.Domain.Submissions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Acme.DrawLanding.Tests.Controllers;

public sealed class SubmissionsControllerTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IntegrationTestFixture _integrationTestFixture;

    public SubmissionsControllerTests(IntegrationTestFixture integrationTestFixture)
    {
        _integrationTestFixture = integrationTestFixture ?? throw new ArgumentNullException(nameof(integrationTestFixture));
    }

    private static SubmissionsController CreateSut()
    {
        var submissionsRepository = new Mock<ISubmissionRepository>();
        submissionsRepository
            .Setup(x => x.InsertIfNotUsedMoreThan(It.IsAny<Submission>(), It.IsAny<int>()))
            .Returns(Task.FromResult(SubmissionInsertionResult.Ok(int.MaxValue)));

        return new SubmissionsController(submissionsRepository.Object);
    }

    [Fact]
    public async Task Get_Index__returns_success()
    {
        // Arrange
        var request = SubmissionMocking.CreateValidSubmissionRequest();
        var sut = CreateSut();

        // Act
        var result = await sut.Index(request);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Post_Index__happy_path()
    {
        // Arrange
        var request = SubmissionMocking.CreateValidSubmissionRequest();

        await _integrationTestFixture.SeedDatabase(async (context) =>
        {
            await context.SerialNumbers.AddAsync(new SerialNumberRecord()
            {
                Content = request.SerialNumber!.Value,
                Uses = 0,
            });

            await context.SaveChangesAsync();
        });

        var client = _integrationTestFixture.AppClient;

        // Act
        var httpContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/submit", httpContent);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Post_Index__requires_firstname()
    {
        // Arrange
        var request = SubmissionMocking.CreateValidSubmissionRequest();
        var client = _integrationTestFixture.AppClient;

        request.FirstName = null;

        // Act
        var httpContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/submit", httpContent);

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var problem = JsonSerializer.Deserialize<ValidationProblemDetails>(content);

        Assert.NotNull(problem);
        Assert.Single(problem.Errors);
        Assert.Contains(problem.Errors, x => x.Key == nameof(FormSubmissionRequest.FirstName));
    }

    [Fact]
    public async Task Post_Index__requires_lastname()
    {
        // Arrange
        var request = SubmissionMocking.CreateValidSubmissionRequest();
        var client = _integrationTestFixture.AppClient;

        request.LastName = null;

        // Act
        var httpContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/submit", httpContent);

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var problem = JsonSerializer.Deserialize<ValidationProblemDetails>(content);

        Assert.NotNull(problem);
        Assert.Single(problem.Errors);
        Assert.Contains(problem.Errors, x => x.Key == nameof(FormSubmissionRequest.LastName));
    }

    [Fact]
    public async Task Post_Index__requires_email()
    {
        // Arrange
        var request = SubmissionMocking.CreateValidSubmissionRequest();
        var client = _integrationTestFixture.AppClient;

        request.Email = null;

        // Act
        var httpContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/submit", httpContent);

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var problem = JsonSerializer.Deserialize<ValidationProblemDetails>(content);

        Assert.NotNull(problem);
        Assert.Single(problem.Errors);
        Assert.Contains(problem.Errors, x => x.Key == nameof(FormSubmissionRequest.Email));
    }

    [Fact]
    public async Task Post_Index__validates_is_adult()
    {
        // Arrange
        var request = SubmissionMocking.CreateValidSubmissionRequest();
        var sut = CreateSut();

        request.IsAdult = false;

        // Act
        var result = await sut.Index(request);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        var validationProblems = Assert.IsType<ValidationProblemDetails>(objectResult.Value);

        Assert.Contains(validationProblems.Errors, x => x.Key == nameof(FormSubmissionRequest.IsAdult));
    }

    [Fact]
    public async Task Post_Index__validates_serialnumber_exists()
    {
        // Mock
        var submissionsRepository = new Mock<ISubmissionRepository>();
        submissionsRepository
            .Setup(x => x.InsertIfNotUsedMoreThan(It.IsAny<Submission>(), It.IsAny<int>()))
            .Returns(Task.FromResult(SubmissionInsertionResult.DoesNotExist));

        // Arrange
        var request = SubmissionMocking.CreateValidSubmissionRequest();
        var sut = new SubmissionsController(submissionsRepository.Object);

        // Act
        var result = await sut.Index(request);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        var validationProblems = Assert.IsType<ValidationProblemDetails>(objectResult.Value);

        Assert.Contains(validationProblems.Errors, x => x.Key == nameof(FormSubmissionRequest.SerialNumber));
    }

    [Fact]
    public async Task Post_Index__validates_serialnumber_not_overused()
    {
        // Mock
        var submissionsRepository = new Mock<ISubmissionRepository>();
        submissionsRepository
            .Setup(x => x.InsertIfNotUsedMoreThan(It.IsAny<Submission>(), It.IsAny<int>()))
            .Returns(Task.FromResult(SubmissionInsertionResult.AlreadyUsed));

        // Arrange
        var request = SubmissionMocking.CreateValidSubmissionRequest();
        var sut = new SubmissionsController(submissionsRepository.Object);

        // Act
        var result = await sut.Index(request);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        var validationProblems = Assert.IsType<ValidationProblemDetails>(objectResult.Value);

        Assert.Contains(validationProblems.Errors, x => x.Key == nameof(FormSubmissionRequest.SerialNumber));
    }
}
