using ClassLibrary.Models;

namespace ClassLibrary.Interfaces;

public interface IAlergenRepository
{
    IEnumerable<Alergen> GetAll();
    Alergen? GetById(int id);
    void Add(Alergen alergen);
    void Update(Alergen alergen);
    void Delete(int id);
    void Save();
}
