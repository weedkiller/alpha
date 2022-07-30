using Multigroup.DomainModel.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Multigroup.Portal.Models.Shared;


namespace Multigroup.Portal.Models.Shared
{
    public class UserVm : BaseVm
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string Names { get; set; }
        [Required(ErrorMessage = "El campo Nombres es requerido")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "El campo Apellido  es requerido")]
        public string IdentificationNumber { get; set; }
        [Required(ErrorMessage = "El campo Documento es requerido")]
        public string UserName { get; set; }
        //[DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "El campo Email es requerido")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail no tiene un formato válido")]
        public string Email { get; set; }
        //[Required(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "VM_FieldRequired")]
        [MinLength(6, ErrorMessage="La contraseña debe tener al menos 6 caracteres")]
        [MaxLength(20, ErrorMessage = "La contraseña debe tener un máximo de 20 caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public bool ChangePassword { get; set; }
        [Required]
        public bool Active { get; set; }
        public string SelectedRole { get; set; }
        public string Commission { get; set; }
        public IEnumerable<SelectListItem> ListRoles { get; set; }
        public SingleSelectVm ddlUserType { get; set; }
        public IEnumerable<SelectListItem> ListZones { get; set; }
        public IEnumerable<string> SelectedZone{ get; set; }
        public SingleSelectVm ddlGerente { get; set; }
        public SingleSelectVm ddlSupervisor { get; set; }
        [Required(ErrorMessage = "El campo Caja es requerido")]
        public SingleSelectVm ddlCashier { get; set; }
        public string Observations { get; set; }



        public User ToModelObject()
        {
            var user = new User()
            {
                UserId = UserId,
                Email = Email,
                Names = Names,
                Active = Active,
                Password = Password,
                Roles = Enumerable.Repeat(new Role() { RoleId = (SelectedRole != null) ? int.Parse(SelectedRole) : 0 }, 1),
                Surname = Surname,
                UserName = UserName,
                IdentificationNumber = IdentificationNumber,
                UserType = GetNullableValue(ddlUserType),
                Zones = (SelectedZone != null) ? SelectedZone.Select(x => new Zone() { ZoneId = int.Parse(x.ToString()) }) : null,
                IdGerente = GetNullableValue(ddlGerente),
                IdSupervisor = GetNullableValue(ddlSupervisor),
            };

            return user;
        }


        public UserVm FromModelObject(User user)
        {
            var userVm = new UserVm()
            {
                //ConfirmPassword = user.Password,
                Email = user.Email,
                Names = user.Names,
                //Password = user.Password,
                Surname = user.Surname,
                Active = user.Active,
                UserId = user.UserId,
                SelectedRole = (user.Roles != null) ? user.Roles.FirstOrDefault().RoleId.ToString() : null,
                UserName = user.UserName,
                IdentificationNumber = user.IdentificationNumber,
                SelectedZone = (user.Zones != null) ? user.Zones.Select(x => x.ZoneId.ToString()).ToArray() : null,

            };

            return userVm;
        }
    }
}