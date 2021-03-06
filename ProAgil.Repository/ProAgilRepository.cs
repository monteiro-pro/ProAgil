using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;
        public ProAgilRepository(ProAgilContext context)
        {
            this._context = context;
            this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        //GERAIS
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        //EVENTOS
        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);

                if(includePalestrantes){
                    query = query
                        .Include(pe => pe.PalestrantesEventos)
                        .ThenInclude(p => p.Palestrante);
                }

                query = query.AsNoTracking()
                            .OrderByDescending(c => c.DataEvento);

                return await query.ToArrayAsync();
        }
        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);

                if(includePalestrantes){
                    query = query
                        .Include(pe => pe.PalestrantesEventos)
                        .ThenInclude(p => p.Palestrante);
                }

                query = query.OrderByDescending(c => c.DataEvento)
                    .Where(c => c.Tema.ToLower().Contains(tema.ToLower()));;

                return await query.ToArrayAsync();
        }
        public async Task<Evento> GetEventoAsyncById(int eventoId, bool includePalestrante)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);

                if(includePalestrante){
                    query = query
                        .Include(pe => pe.PalestrantesEventos)
                        .ThenInclude(p => p.Palestrante);
                }

                query = query.OrderByDescending(c => c.DataEvento)
                    .Where(c => c.Id == eventoId);;

                return await query.FirstOrDefaultAsync();
        }

        //PALESTRANTES
        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(c => c.RedesSociais);

                if(includeEventos){
                    query = query
                        .Include(pe => pe.PalestrantesEventos)
                        .ThenInclude(e => e.Evento);
                }

                query = query.OrderByDescending(c => c.Nome);

                return await query.ToArrayAsync();
        }
        public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(c => c.RedesSociais);

                if(includeEventos){
                    query = query
                        .Include(pe => pe.PalestrantesEventos)
                        .ThenInclude(e => e.Evento);
                }

                query = query.OrderByDescending(c => c.Nome)
                    .Where(p => p.Nome.ToLower().Contains(nome.ToLower()));

                return await query.ToArrayAsync();
        }
        public async Task<Palestrante> GetPalestranteAsyncById(int palestranteId, bool includeEvento)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(c => c.RedesSociais);

                if(includeEvento){
                    query = query
                        .Include(pe => pe.PalestrantesEventos)
                        .ThenInclude(e => e.Evento);
                }

                query = query.OrderBy(p => p.Nome)
                    .Where(p => p.Id == palestranteId);

                return await query.FirstOrDefaultAsync();
        }
    }
}