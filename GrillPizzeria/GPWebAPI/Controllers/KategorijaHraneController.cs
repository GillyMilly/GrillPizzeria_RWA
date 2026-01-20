using AutoMapper;
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTO;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KategorijaHraneController : ControllerBase
{
    private readonly IKategorijaHraneRepository _repository;
    private readonly IMapper _mapper;

    public KategorijaHraneController(IKategorijaHraneRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<KategorijaHraneDto>> GetAll()
    {
        var kategorije = _repository.GetAll();
        var kategorijaDtos = _mapper.Map<IEnumerable<KategorijaHraneDto>>(kategorije);
        return Ok(kategorijaDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<KategorijaHraneDto> GetById(int id)
    {
        var kategorija = _repository.GetById(id);
        if (kategorija == null)
            return NotFound($"Kategorija s ID={id} nije pronađena.");

        return Ok(_mapper.Map<KategorijaHraneDto>(kategorija));
    }

    [HttpPost]
    public ActionResult<KategorijaHraneDto> Create([FromBody] KategorijaHraneDto kategorijaDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var kategorija = _mapper.Map<KategorijaHrane>(kategorijaDto);
        _repository.Add(kategorija);

        return CreatedAtAction(nameof(GetById), new { id = kategorija.IdkategorijaHrane }, _mapper.Map<KategorijaHraneDto>(kategorija));
    }

    [HttpPut("{id}")]
    public ActionResult<KategorijaHraneDto> Update(int id, [FromBody] KategorijaHraneDto kategorijaDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingKategorija = _repository.GetById(id);
        if (existingKategorija == null)
            return NotFound($"Kategorija s ID={id} nije pronađena.");

        _mapper.Map(kategorijaDto, existingKategorija);
        _repository.Update(existingKategorija);

        return Ok(_mapper.Map<KategorijaHraneDto>(existingKategorija));
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var kategorija = _repository.GetById(id);
        if (kategorija == null)
            return NotFound($"Kategorija s ID={id} nije pronađena.");

        _repository.Delete(id);
        return Ok(new { message = "Kategorija je uspješno obrisana.", id });
    }
}
