namespace ArgosApi.Domain.Entities
{
    public class Projeto : BaseEntity
    {
        public string Nome { get; set; } = "";

        public ICollection<Usuario> Usuarios { get; set; } = [];

        public ICollection<Relatorio> Relatorios { get; set; } = [];
    }
}
