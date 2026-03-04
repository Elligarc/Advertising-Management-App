using AdvertisingApp.Core.Contracts;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Web.Mappings;
using AdvertisingApp.Web.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingApp.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurfacesController : ControllerBase
{
    private readonly ISurfaceService _surfaceService;

    public SurfacesController(ISurfaceService surfaceService)
    {
        _surfaceService = surfaceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SurfaceResponseModel>>> GetByConstruction([FromQuery] int constructionId)
    {
        var surfaces = await _surfaceService.GetByConstructionIdAsync(constructionId);
        var response = surfaces.Select(s => s.ToResponse());
        return Ok(response);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<SurfaceResponseModel>>> Filter([FromQuery] FilterSurfacesData filter)
    {
        var surfaces = await _surfaceService.GetFilteredAsync(filter);
        var response = surfaces.Select(s => s.ToResponse());
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SurfaceResponseModel>> GetById(int id)
    {
        var surface = await _surfaceService.GetByIdAsync(id);
        if (surface == null)
            return NotFound();

        return Ok(surface.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<SurfaceResponseModel>> Create([FromBody] CreateSurfaceData request)
    {
        try
        {
            var created = await _surfaceService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToResponse());
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SurfaceResponseModel>> Update(int id, [FromBody] UpdateSurfaceData request)
    {
        try
        {
            var updated = await _surfaceService.UpdateAsync(id, request);
            return Ok(updated.ToResponse());
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{id}/price")]
    public async Task<ActionResult<SurfaceResponseModel>> UpdatePrice(int id, [FromBody] UpdateSurfacePriceData request)
    {
        try
        {
            var updated = await _surfaceService.UpdatePriceAsync(id, request);
            return Ok(updated.ToResponse());
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<SurfaceResponseModel>> UpdateStatus(int id,
        [FromBody] UpdateSurfaceStatusData request)
    {
        try
        {
            var updated = await _surfaceService.UpdateStatusAsync(id, request);
            return Ok(updated.ToResponse());
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _surfaceService.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}