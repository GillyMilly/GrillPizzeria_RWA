using AutoMapper;
using ClassLibrary.Models;
using WebApp.ViewModels;

namespace WebApp.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Hrana, HranaVM>()
            .ForMember(dest => dest.KategorijaNaziv, opt => opt.MapFrom(src => src.KategorijaHrane.Naziv));

        CreateMap<HranaVM, Hrana>()
            .ForMember(dest => dest.KategorijaHraneId, opt => opt.MapFrom(src => src.KategorijaHraneId))
            .ForMember(dest => dest.Cijena, opt => opt.MapFrom(src => src.Cijena))
            .ForMember(dest => dest.Naslov, opt => opt.MapFrom(src => src.Naslov))
            .ForMember(dest => dest.Opis, opt => opt.MapFrom(src => src.Opis))
            .ForMember(dest => dest.SlikaUrl, opt => opt.MapFrom(src => src.SlikaUrl));

        CreateMap<Korisnik, KorisnikVM>()
            .ReverseMap();

        CreateMap<Narudzba, NarudzbaVM>()
            .ForMember(dest => dest.KorisnikId, opt => opt.MapFrom(src => src.Korisnik.Idkorisnik))
            .ReverseMap();

        CreateMap<KategorijaHrane, KategorijaHraneVM>()
            .ReverseMap();

        CreateMap<Alergen, AlergenVM>()
            .ReverseMap();

        CreateMap<Log, LogVM>()
            .ReverseMap();
    }
}
