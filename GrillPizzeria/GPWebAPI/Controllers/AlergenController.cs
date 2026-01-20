using AutoMapper;
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTO;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AlergenController : ControllerBase
{
    private readonly IAlergenRepository _repository;
    private readonly IMapper _mapper;

    public AlergenController(IAlergenRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AlergenDto>> GetAll()
    {
        var alergens = _repository.GetAll();
        var alergenDtos = _mapper.Map<IEnumerable<AlergenDto>>(alergens);
        return Ok(alergenDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<AlergenDto> GetById(int id)
    {
        var alergen = _repository.GetById(id);
        if (alergen == null)
            return NotFound($"Alergen s ID={id} nije pronađen.");

        return Ok(_mapper.Map<AlergenDto>(alergen));
    }

    [HttpPost]
    public ActionResult<AlergenDto> Create([FromBody] AlergenDto alergenDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var alergen = _mapper.Map<Alergen>(alergenDto);
        _repository.Add(alergen);

        return CreatedAtAction(nameof(GetById), new { id = alergen.Idalergen }, _mapper.Map<AlergenDto>(alergen));
    }

    [HttpPut("{id}")]
    public ActionResult<AlergenDto> Update(int id, [FromBody] AlergenDto alergenDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingAlergen = _repository.GetById(id);
        if (existingAlergen == null)
            return NotFound($"Alergen s ID={id} nije pronađen.");

        _mapper.Map(alergenDto, existingAlergen);
        _repository.Update(existingAlergen);

        return Ok(_mapper.Map<AlergenDto>(existingAlergen));
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var alergen = _repository.GetById(id);
        if (alergen == null)
            return NotFound($"Alergen s ID={id} nije pronađen.");

        _repository.Delete(id);
        return Ok(new { message = "Alergen je uspješno obrisan.", id });
    }
}
