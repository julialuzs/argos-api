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
            return await context.Projetos.FindAsync(id, cancellationToken);
        }

        /// <summary>
        /// Busca todos os projeto pelo id do usuário
        /// </summary>
        public async Task<List<Projeto>> ListarProjetosPorUsuario(long idUsuario, CancellationToken cancellationToken)
        {
            return context.Projetos.Where((projeto) => 
                projeto.Usuarios
                    .Select(u => u.Id)
                    .Contains(idUsuario)).ToList() ?? [];
        }

        /// <summary>
        /// Busca todos os projeto pelo usuário logado
        /// </summary>
        public async Task<List<Projeto>> ListarProjetosPorUsuarioLogado( CancellationToken cancellationToken)
        { 
            return context.Projetos.Where((projeto) =>
                projeto.Usuarios
                    .Select(u => u.Id)
                    .Contains(currentUser.Id)).ToList() ?? [];
        }

        /// <summary>
        /// Cria projeto na base de dados
        /// </summary>
        public async Task CriarProjeto(CriacaoProjetoRequest projeto, CancellationToken cancellationToken)
        {
            var usuario = await context.Usuarios.FindAsync(currentUser.Id, cancellationToken);

            var novoProjeto = new Projeto
            {
                Nome = projeto.Nome,
                Descricao = projeto.Descricao,
            };

            novoProjeto.Usuarios.Add(usuario);

            await context.Projetos.AddAsync(novoProjeto, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Busca projeto pelo id e o altera na base de dados
        /// </summary>
        public async Task<Projeto?> EditarProjeto(Projeto projeto, CancellationToken cancellationToken)
        {
            var entity = await context.Projetos.FindAsync(projeto.Id);
            if (entity == null)
            {
                return null;
            }
            entity = projeto;
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        /// <summary>
        /// Busca projeto pelo id e o altera na base de dados
        /// </summary>
        /// TODO: implementar tratamento de erros
        public async Task<Projeto?> VincularUsuarioNoProjeto(long idProjeto, long idUsuario, CancellationToken cancellationToken)
        {
            var entityProjeto = await context.Projetos.FindAsync(idProjeto, cancellationToken);
            if (entityProjeto == null)
            {
                // projeto nao encontrado
                return null;
            }

            var entityUsuario = await context.Usuarios.FindAsync(idUsuario, cancellationToken);
            if (entityUsuario == null)
            {
                // usuario nao encontrado
                return null;
            }

            entityProjeto.Usuarios.Add(entityUsuario);
            await context.SaveChangesAsync();

            return entityProjeto;
        }

    }
}