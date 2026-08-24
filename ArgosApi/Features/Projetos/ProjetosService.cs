using ArgosApi.Data;
using ArgosApi.Domain.Entities;
using ArgosApi.Features.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace ArgosApi.Features.Projetos
{
    /// <summary>
    /// Service responsável por gerenciar os projetos
    /// </summary>
    public class ProjetosService(AppDbContext context, CurrentUser currentUser)
    {
        /// <summary>
        /// Busca projeto pelo id
        /// </summary>
        public async Task<Projeto?> GetProjetoPorId(long id, CancellationToken cancellationToken)
        {
            return await context.Projetos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.Usuarios.Any(u => u.Id == currentUser.Id), cancellationToken);
        }

        /// <summary>
        /// Busca todos os projeto pelo id do usuário
        /// </summary>
        public async Task<List<Projeto>> ListarProjetosPorUsuario(long idUsuario, CancellationToken cancellationToken)
        {
            return await context.Projetos.AsNoTracking().Where((projeto) =>
                projeto.Usuarios
                    .Select(u => u.Id)
                    .Contains(idUsuario)).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Busca todos os projeto pelo usuário logado
        /// </summary>
        public async Task<List<Projeto>> ListarProjetosPorUsuarioLogado(CancellationToken cancellationToken)
        {
            return await context.Projetos.AsNoTracking().Where((projeto) =>
                projeto.Usuarios
                    .Select(u => u.Id)
                    .Contains(currentUser.Id)).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Cria projeto na base de dados
        /// </summary>
        public async Task CriarProjeto(CriacaoProjetoRequest projeto, CancellationToken cancellationToken)
        {
            var usuario = await context.Usuarios.FindAsync([currentUser.Id], cancellationToken);

            var novoProjeto = new Projeto
            {
                Nome = projeto.Nome,
                Descricao = projeto.Descricao,
                UrlBase = projeto.UrlBase?.Trim() ?? "",
                Rotas = NormalizarRotas(projeto.Rotas),
                IncluirW3c = projeto.IncluirW3c,
            };

            if (usuario is not null)
            {
                novoProjeto.Usuarios.Add(usuario);
            }

            await context.Projetos.AddAsync(novoProjeto, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Busca projeto pelo id e o altera na base de dados
        /// </summary>
        public async Task<Projeto?> EditarProjeto(long id, CriacaoProjetoRequest request, CancellationToken cancellationToken)
        {
            var entity = await context.Projetos
                .Include(p => p.Usuarios)
                .FirstOrDefaultAsync(p => p.Id == id && p.Usuarios.Any(u => u.Id == currentUser.Id), cancellationToken);
            if (entity == null)
            {
                return null;
            }

            entity.Nome = request.Nome;
            entity.Descricao = request.Descricao;
            entity.UrlBase = request.UrlBase?.Trim() ?? "";
            entity.Rotas = NormalizarRotas(request.Rotas);
            entity.IncluirW3c = request.IncluirW3c;

            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        /// <summary>
        /// Busca projeto pelo id e o altera na base de dados
        /// </summary>
        /// TODO: implementar tratamento de erros
        public async Task<Projeto?> VincularUsuarioNoProjeto(long idProjeto, long idUsuario, CancellationToken cancellationToken)
        {
            var entityProjeto = await context.Projetos.FindAsync([idProjeto], cancellationToken);
            if (entityProjeto == null)
            {
                return null;
            }

            var entityUsuario = await context.Usuarios.FindAsync([idUsuario], cancellationToken);
            if (entityUsuario == null)
            {
                return null;
            }

            entityProjeto.Usuarios.Add(entityUsuario);
            await context.SaveChangesAsync(cancellationToken);

            return entityProjeto;
        }

        private static string[] NormalizarRotas(string[]? rotas)
        {
            var normalizadas = (rotas ?? [])
                .Select(r => r.Trim())
                .Where(r => r.Length > 0)
                .ToArray();

            return normalizadas.Length > 0 ? normalizadas : ["/"];
        }
    }
}
