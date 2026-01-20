using ClassLibrary.Models;

namespace ClassLibrary.Interfaces;

public interface IKategorijaHraneRepository
{
    IEnumerable<KategorijaHrane> GetAll();
    KategorijaHrane? GetById(int id);
    void Add(KategorijaHrane kategorijaHrane);
    void Update(KategorijaHrane kategorijaHrane);
    void Delete(int id);
    void Save();
}
