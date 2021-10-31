using Cadastro.Domain.Entities;
using Cadastro.Infrastructure.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static Cadastro.Areas.Identity.Pages.Account.RegisterModel;

namespace App.Cidadao.Api.Areas
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = "Identity.Application")]
    public class EditModel : PageModel
    {
        RegisterContext _ctx;

        public EditModel(RegisterContext ctx)
        {
            _ctx = ctx;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            
            Input = new InputModel();
            Input.Id = user.Id;
            Input.UserName = user.Email;
            Input.Email = user.Email;
            Input.Name = user.Name;
            Input.LastName = user.LastName;
            Input.IsActive = user.IsActive;
            Input.PhoneNumber = user.PhoneNumber;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == Input.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.UserName = Input.Email;
                user.Email = Input.Email;
                user.Name = Input.Name;
                user.LastName = Input.LastName;
                user.IsActive = Input.IsActive;
                user.PhoneNumber = Input.PhoneNumber;

                await _ctx.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await UserExists(Input.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> UserExists(string id)
        {
            return (await _ctx.Users.FirstOrDefaultAsync(x=>x.Id == id)) == null;
        }
    }
}
