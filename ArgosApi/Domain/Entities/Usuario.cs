namespace ArgosApi.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public ICollection<Projeto> Projetos { get; set; } = [];
    }
}