using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingApp.Tests.Extensions;

public static class ControllerAssertionsExtensions
{
    public static OkObjectResult ShouldBeOk(this IActionResult result)
    {
        return result.Should().BeOfType<OkObjectResult>().Subject;
    }

    public static CreatedAtActionResult ShouldBeCreated(this IActionResult result)
    {
        return result.Should().BeOfType<CreatedAtActionResult>().Subject;
    }

    public static NotFoundResult ShouldBeNotFound(this IActionResult result)
    {
        return result.Should().BeOfType<NotFoundResult>().Subject;
    }

    public static BadRequestObjectResult ShouldBeBadRequest(this IActionResult result)
    {
        return result.Should().BeOfType<BadRequestObjectResult>().Subject;
    }

    public static NoContentResult ShouldBeNoContent(this IActionResult result)
    {
        return result.Should().BeOfType<NoContentResult>().Subject;
    }

    public static T GetValue<T>(this OkObjectResult result) where T : class
    {
        return result.Value.Should().BeOfType<T>().Subject;
    }

    public static T GetValue<T>(this CreatedAtActionResult result) where T : class
    {
        return result.Value.Should().BeOfType<T>().Subject;
    }

    public static OkObjectResult GetOkResult(this IActionResult result)
    {
        return result.Should().BeOfType<OkObjectResult>().Subject;
    }

    public static CreatedAtActionResult GetCreatedResult(this IActionResult result)
    {
        return result.Should().BeOfType<CreatedAtActionResult>().Subject;
    }
}