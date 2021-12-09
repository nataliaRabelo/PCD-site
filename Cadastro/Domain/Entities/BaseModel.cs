using System.ComponentModel.DataAnnotations;

namespace Cadastro.Domain.Entities
{
    public class BaseModel
    {
        [Key]
        public virtual int Id { get; set; }
    }
}
