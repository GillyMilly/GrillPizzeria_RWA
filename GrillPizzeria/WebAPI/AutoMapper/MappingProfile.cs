using AutoMapper;
using ClassLibrary.Models;
using WebAPI.DTO;

namespace WebAPI.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Hrana, HranaDto>()
            .ForMember(dest => dest.KategorijaNaziv, opt => opt.MapFrom(src => src.KategorijaHrane != null ? src.KategorijaHrane.Naziv : string.Empty))
            .ForMember(dest => dest.AlergenNazivi, opt => opt.MapFrom(src => src.HranaAlergens.Where(ha => ha.Alergen != null).Select(ha => ha.Alergen!.Naziv)))
            .ReverseMap();

        CreateMap<Korisnik, KorisnikDto>()
            .ReverseMap();

        CreateMap<Narudzba, NarudzbaDto>()
            .ForMember(dest => dest.KorisnikId, opt => opt.MapFrom(src => src.KorisnikId))
            .ReverseMap();

        CreateMap<KategorijaHraneDto, KategorijaHrane>()
            .ReverseMap();

        CreateMap<AlergenDto, Alergen>()
            .ReverseMap();
    }
}
