using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Domain.Entities
{
    public class Report : BaseModel
    {
        public Report()
        {

        }
        public Report(string name, IEnumerable<Product> produtos, string userId, string value)
        {
            if (produtos == null)
            {
                this.Name = "sem nome";
            }
            else
            {
                this.Name = name;
            }
            if(produtos == null)
            {
                this.Produtos = new List<Product>();
            }
            else
            {
                this.Produtos = produtos;
            }
            this.UserId = userId;
            this.Value = value;
        }

        public string Name { get; set; }
        public string UserId { get; set; }
        public virtual IEnumerable<Product> Produtos { get; set; }

        public virtual Category Category { get; set; }
        public string Value { get; set; }

        public int IdCategory { get; set; }

    }
}
