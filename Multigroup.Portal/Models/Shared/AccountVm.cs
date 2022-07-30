using Multigroup.DomainModel.Shared;
using System.ComponentModel.DataAnnotations;

namespace Multigroup.Portal.Models.Shared
{
    public class AccountVm
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [Required]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        public User ToModelObject()
        {
            return new User
            {
                Email = Email,
                Names = Name,
                Surname = LastName,
                UserName = UserName,
                Password = Password,
            };
        }
    }
}
