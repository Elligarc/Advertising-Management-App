using AdvertisingApp.Core.Contracts;
using AdvertisingApp.Core.Entities;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Web.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingApp.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientResponseModel>>> GetAll()
    {
        var clients = await _clientService.GetAllAsync();
        var response = clients.Select(MapToResponse);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClientResponseModel>> GetById(int id)
    {
        var client = await _clientService.GetByIdAsync(id);
        if (client == null)
            return NotFound();

        return Ok(MapToResponse(client));
    }

    [HttpPost]
    public async Task<ActionResult<ClientResponseModel>> Create([FromBody] CreateClientData request)
    {
        var created = await _clientService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToResponse(created));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClientResponseModel>> Update(int id, [FromBody] UpdateClientData request)
    {
        try
        {
            var updated = await _clientService.UpdateAsync(id, request);
            return Ok(MapToResponse(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _clientService.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    private static ClientResponseModel MapToResponse(Client client)
    {
        return new ClientResponseModel
        {
            Id = client.Id,
            Name = client.Name,
            Phone = client.Phone
        };
    }
}