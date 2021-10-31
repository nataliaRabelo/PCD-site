using System.ComponentModel;

namespace Cadastro.Domain.Entities
{
    public class Category : BaseModel
    {
        [DisplayName("Nome")]
        public string Name { get; set; }
    }
}
