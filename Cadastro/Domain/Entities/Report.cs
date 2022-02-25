using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Domain.Entities
{
    public class Report : BaseModel
    {
        public Report()
        {

        }
        public Report(string name, List<Product> produtos)
        {
            this.Name = name;
            this.Produtos = produtos;
        }

        public string Name { get; set; }
        public virtual List<Product> Produtos { get; set; }
    }
}
