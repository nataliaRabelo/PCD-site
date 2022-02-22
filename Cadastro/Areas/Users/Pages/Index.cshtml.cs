using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cadastro.Domain.Entities;
using Cadastro.Infrastructure.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace App.Cidadao.Api.Areas
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin", AuthenticationSchemes = "Identity.Application")]
    public class IndexModel : PageModel
    {
        RegisterContext _ctx;
        public IndexModel(RegisterContext ctx)
        {
            _ctx = ctx;
        }

        public IList<User> UsersList { get; set; }
        public int TotalOfItens { get; set; }

        public async Task OnGetAsync([FromQuery] int page = 1, [FromQuery] int size = 5)
        {
            this.UsersList = await _ctx.Users.ToListAsync();
        }
    }
}
