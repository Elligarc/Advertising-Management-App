using AdvertisingApp.Core.Contracts;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Web.Mappings;
using AdvertisingApp.Web.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingApp.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContractResponseModel>>> GetAll()
    {
        var contracts = await _contractService.GetAllAsync();
        var response = contracts.Select(c => c.ToResponse());
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContractResponseModel>> GetById(int id)
    {
        var contract = await _contractService.GetByIdAsync(id);
        if (contract == null)
            return NotFound();

        return Ok(contract.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<ContractResponseModel>> Create([FromBody] CreateContractData request)
    {
        try
        {
            var created = await _contractService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToResponse());
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/items")]
    public async Task<ActionResult<ContractItemResponseModel>> AddItem(int id,
        [FromBody] CreateContractItemData request)
    {
        try
        {
            var item = await _contractService.AddItemAsync(id, request);
            return CreatedAtAction(nameof(GetById), new { id }, item.ToResponse());
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
    public async Task<ActionResult<ContractResponseModel>> Update(int id, [FromBody] UpdateContractData request)
    {
        try
        {
            var updated = await _contractService.UpdateAsync(id, request);
            return Ok(updated.ToResponse());
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}