using Cadastro.Enumerations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Domain.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            this.CreatedAt = DateTime.Now;
            this.IsActive = true;

        }
        [DisplayName("Nome")]
        public string Name { get; set; }
        [DisplayName("Sobrenome")]
        public string LastName { get; set; }
        [DisplayName("Ativo")]
        public bool IsActive { get; set; }
        [DisplayName("Data Cadastro")]
        public DateTime CreatedAt { get; set; }
        [DisplayName("Código")]
        public int Code { get; set; }
        public AccountType AccountType { get; set; }
    }
}
