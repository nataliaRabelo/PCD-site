using System.ComponentModel;

namespace Cadastro.Domain.Entities
{
    public class Product: BaseModel
    {
        public string UserId { get; set; }
        [DisplayName("Nome da classe")]
        public string Name { get; set; }
        [DisplayName("Código")]
        public string Value { get; set; }
        
        [DisplayName("Ativo")]
        public bool Active { get; set; }

        public int IdCategory { get; set; }
        public int? IdRelatorio { get; set; }

        [DisplayName("Classe")]
        public virtual Category Category { get; set; }

        public virtual Report Report { get; set; }
        public virtual User User { get; set; }
    }
}