using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        //Geral
         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         Task<bool> SaveChangesAsync();

         //Evento
         Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes = false);
         Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false);
         Task<Evento> GetEventoAsyncById(int eventoId, bool includePalestrante);

         //Palestrante
         Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos);
         Task<Palestrante[]> GetAllPalestrantesAsyncByName(string nome, bool includeEventos);
         Task<Palestrante> GetPalestranteAsyncById(int palestranteId, bool includeEvento);
    }
}