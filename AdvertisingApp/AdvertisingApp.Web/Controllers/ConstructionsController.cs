using AdvertisingApp.Core.Contracts;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Web.Mappings;
using AdvertisingApp.Web.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingApp.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConstructionsController : ControllerBase
{
    private readonly IConstructionService _constructionService;

    public ConstructionsController(IConstructionService constructionService)
    {
        _constructionService = constructionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConstructionResponseModel>>> GetAll()
    {
        var constructions = await _constructionService.GetAllAsync();
        var response = constructions.Select(c => c.ToResponse());
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ConstructionResponseModel>> GetById(int id)
    {
        var construction = await _constructionService.GetByIdAsync(id);
        if (construction == null)
            return NotFound();

        return Ok(construction.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<ConstructionResponseModel>> Create([FromBody] CreateConstructionData request)
    {
        var created = await _constructionService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToResponse());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ConstructionResponseModel>> Update(int id, [FromBody] UpdateConstructionData request)
    {
        try
        {
            var updated = await _constructionService.UpdateAsync(id, request);
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
        var result = await _constructionService.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}