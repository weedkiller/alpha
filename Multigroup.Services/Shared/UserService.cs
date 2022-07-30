using Multigroup.Common.Logging;
using Multigroup.Common.Notifications;
using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using Multigroup.Services.Shared;
using Multigroup.Services;
using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Threading.Tasks;


namespace Multigroup.Services.Shared
{
    public class UserService
    {
        private UserRepository _userRepository;
        private RoleService _roleService;
        private IAuditLogHelper _auditLogHelper;
        private readonly IEmailSender _emailSender;
        private EmployeeService _employeeService;

        public UserService()
        {
            _userRepository = new UserRepository();
            _roleService = new RoleService();
            _auditLogHelper = new AuditLogHelper();
            _emailSender = new EmailSender();
            _employeeService = new EmployeeService();
        }

        public IEnumerable<UserSummary> GetUserSummary()
        {
            try
            {
                var users = _userRepository.GetUserSummary();
                return users;
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message + " " + ex.StackTrace,
                    IntegrationType = AuditLogType.UserService
                };
                _auditLogHelper.LogAuditFail(log);
                throw ex;
            }
        }
        public IEnumerable<User> GetUsers()
        {
            try
            {
                var users = _userRepository.GetUsers();
                return users;
            }
            catch (Exception ex)
            {
                var log = new AuditLog() { 
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message + " " + ex.StackTrace, 
                    IntegrationType = AuditLogType.UserService };
                _auditLogHelper.LogAuditFail(log);
                throw ex;
            }
        }

        public IEnumerable<User> GetUsersSurName()
        {
                List<User> apellidos = new List<User>();
                var users = _userRepository.GetUsers();
                foreach (User usuario in users)
                {
                    usuario.Names = usuario.Names + " " + usuario.Surname;
                    apellidos.Add(usuario);
                }
                return apellidos;
        }

        public IEnumerable<User> GetUsersComplete()
        {
            List<User> usuarios = new List<User>();
             
                var users = _userRepository.GetUsers();
            foreach (User usuario in users)
            {
                usuario.Names = usuario.Surname + " " + usuario.Names;
                usuarios.Add(usuario);
            }


            return usuarios;
        }

        public IEnumerable<User> GetUsersAuthorized(double amount)
        {
            List<User> usuarios = new List<User>();

            var users = _userRepository.GetUsers();
            foreach (User usuario in users)
            {
                if (usuario.Limit >= amount)
                {
                    usuario.Names = usuario.Surname + " " + usuario.Names;
                    usuarios.Add(usuario);
                }
            }
            return usuarios;
        }

        public IEnumerable<User> GetUsersCollection()
        {
            try
            {
                var users = _userRepository.GetUsersCollection();
                return users;
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message + " " + ex.StackTrace,
                    IntegrationType = AuditLogType.UserService
                };
                _auditLogHelper.LogAuditFail(log);
                throw ex;
            }
        }

        public IEnumerable<User> GetSellers()
        {
                var users = _userRepository.GetSellers();
                return users;
        }

        public bool existeUsuario(string Documento, string userName)
        {
            bool existe = false;
            var users = _userRepository.GetUsers();
            foreach (User user in users)
                if (user.IdentificationNumber == Documento && user.UserName != userName)
                    existe = true;

            return existe;
        }

        public User GetZoneCollector(int ZoneId)
        {
            try
            {
                var userData = _userRepository.GetZoneCollector(ZoneId);
                return userData;
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message + " " + ex.StackTrace,
                    InputParam = ZoneId,
                    IntegrationType = AuditLogType.UserService
                };
                _auditLogHelper.LogAuditFail(log);
                throw ex;
            }
        }
        public User GetUser(int userId)
        {
            try
            {
                var userData = _userRepository.GetUser(userId);
                if(userData != null)
                { 
                    userData.Roles = _roleService.GetRolesByUser(userId);
                    userData.Zones = _userRepository.GetZonesByIdUser(userId);

                }

                return userData;
            }
            catch(Exception ex)
            {
                var log = new AuditLog() { 
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message + " " + ex.StackTrace, 
                    InputParam = userId, 
                    IntegrationType = AuditLogType.UserService };
                _auditLogHelper.LogAuditFail(log);
                throw ex;
            }
        }

        public IEnumerable<UserType> GetUserTypes()
        {
            return _userRepository.GetUserTypes();
        }

        public IEnumerable<UserCashier> GetUserCashiers()
        {
            return _userRepository.GetUserCashiers();
        }

        public UserCashier GetUserCashierByUserId(int UserId)
        {
            return _userRepository.GetUserCashierByUserId(UserId);
        }

        public IEnumerable<BaseEntity> GetUsersCollector()
        {
            return _userRepository.GetUsersByType(2);
        }

        public IEnumerable<BaseEntity> GetUsersTelemarketer()
        {
            return _userRepository.GetUsersByType(7);
        }

        public IEnumerable<BaseEntity> GetUsersByType(int userTypeId)
        {
            return _userRepository.GetUsersByType(userTypeId);
        }

        public User GetUserByUserName(string userName)
        {
            return _userRepository.GetUserByUserName(userName);
        }

        public User GetUserByMail(string email)
        {
            return _userRepository.GetUserByMail(email);
        }
        public User GetUserByDocument(string document)
        {
            var users = _userRepository.GetUsers();
            foreach (User user in users)
                if (user.IdentificationNumber == document)
                    return user;
            return null;
        }

        public void AddUser(User user, int cashierId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    user.UserName = user.UserName.ToLower();
                    if (user.UserType != 1)
                        user.IdSupervisor = null;
                    if (user.UserType != 4)
                        user.IdGerente = null;
                    var userId = _userRepository.Insert(user);
                    _userRepository.AddUserCashier(userId, cashierId);

                    Provider pro = _employeeService.GetProviderByDocument(user.IdentificationNumber);

                    if (pro != null)
                    {
                        pro.ProviderTypeId = user.UserType.Value;
                        pro.UserId = userId;
                        _employeeService.UpdateProvider(pro);
                    }

                    if (user.Roles != null)
                    {
                        user.Roles.ForEach(x => _roleService.InsertRoles(x.RoleId, userId));
                    }

                    if (user.Zones != null)
                    {
                        user.Zones.ForEach(x => _userRepository.UpdateZonexUser(x.ZoneId, userId));
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    var log = new AuditLog() { 
                        LogLevel = AuditLogLevel.Error,
                        Description = ex.Message + " " + ex.StackTrace, 
                        InputParam = user, 
                        IntegrationType = AuditLogType.UserService };
                    _auditLogHelper.LogAuditFail(log);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Se inactiva la cuenta del usuario que recibe por parámetro
        /// </summary>
        /// <param name="user"></param>
        public void InactiveUserUser(User user)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                   
                    _userRepository.Update(user);

 
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    var log = new AuditLog()
                    {
                        LogLevel = AuditLogLevel.Error,
                        Description = ex.Message + " " + ex.StackTrace,
                        InputParam = user,
                        IntegrationType = AuditLogType.UserService
                    };
                    _auditLogHelper.LogAuditFail(log);
                    throw ex;
                }
            }
        }

        public IEnumerable<UserTypeHistory> GetUserTypeHistoryByUserId(int userId)
        {
            return _userRepository.GetUserTypeHistoryByUserId(userId);
        }

        public void AddUserTypeHistory(int newType, int oldType, string observations, int userId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _userRepository.AddUserTypeHistory(newType, oldType, observations, DateTime.Now.Date, userId);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public void AddUserLogin(string userName, string Name)
        {
            _userRepository.AddUserLogin(userName, Name, DateTime.Now);
        }

        public void UpdatetUser(User user, int cashierId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    user.UserName = user.UserName.ToLower();

                    if (user.UserType != 1)
                        user.IdSupervisor = null;
                    if (user.UserType != 4)
                        user.IdGerente = null;

                    User usuarioViejo = _userRepository.GetUser(user.UserId);

                    _userRepository.Update(user);
                    UserCashier userCachier = _userRepository.GetUserCashierByUserId(user.UserId);
                    if (userCachier != null)
                    _userRepository.DeleteUserCashier(userCachier.UserCashierId);
                    _userRepository.AddUserCashier(user.UserId, cashierId);

                    if (usuarioViejo.IdentificationNumber != user.IdentificationNumber)
                    {

                        Provider pro = _employeeService.GetProviderByUserId(user.UserId);

                        if (pro != null)
                        {
                            pro.Document = user.IdentificationNumber;
                            pro.ProviderTypeId = user.UserType.Value;
                            pro.UserId = user.UserId;
                            _employeeService.UpdateProvider(pro);
                        }
                        else
                        {
                             pro = _employeeService.GetProviderByDocument(user.IdentificationNumber);

                            if (pro != null)
                            {
                                pro.ProviderTypeId = user.UserType.Value;
                                pro.UserId = user.UserId;
                                _employeeService.UpdateProvider(pro);
                            }
                        }

                    }
                    else
                    {
                        Provider pro = _employeeService.GetProviderByDocument(user.IdentificationNumber);

                        if (pro != null)
                        {
                            pro.ProviderTypeId = user.UserType.Value;
                            pro.UserId = user.UserId;
                            _employeeService.UpdateProvider(pro);
                        }

                    }                

                    ///Limpiar todos los roles y minas asignados anteriormente al usuario x
                    _roleService.DeleteRoles(user.UserId);
                    
                    if(user.Roles != null)
                        user.Roles.ForEach(x => _roleService.InsertRoles(x.RoleId, user.UserId));

                    _userRepository.DeleteZonexUser(user.UserId);
                    if (user.Zones != null)
                    {
                        user.Zones.ForEach(x => _userRepository.UpdateZonexUser(x.ZoneId, user.UserId));
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    var log = new AuditLog() { 
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message + " " + ex.StackTrace, 
                    InputParam = user, 
                    IntegrationType = AuditLogType.UserService };
                    _auditLogHelper.LogAuditFail(log);
                    throw ex;
                }
            }
        }


        public void UpdateLimit(int id, double Limit)
        {
            _userRepository.UpdateLimit(id, Limit);
        }

        /// <summary>
        /// Retorna el listado de usuarios sin actividad (sin logearse en el sistema)
        /// en los últimos meses (parametro reccibido)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetUsersWithoutActivity(int monthCount)
        {
            try
            {
                var users = _userRepository.GetUsersWithoutActivity(monthCount);
                return users;
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message + " " + ex.StackTrace,
                    IntegrationType = AuditLogType.UserService
                };
                _auditLogHelper.LogAuditFail(log);
                throw ex;
            }
        }

        public User Login(User user)
        {
            try
            {
                var userData = _userRepository.UserLogin(user);
                
                return userData;
            }
            catch (Exception ex)
            {
                var log = new AuditLog() { 
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message + " " + ex.StackTrace, 
                    InputParam = user, 
                    IntegrationType = AuditLogType.UserService };
                _auditLogHelper.LogAuditFail(log);
                throw ex;
            }
        }

        public void InactiveAccountNotification(User user)
        {
            var writeLog = new Task(() => AsyncInactiveAccountNotification(user));
            writeLog.Start();
        }

        public void RecoverPassword(User user)
        {
            var writeLog = new Task(() => AsyncRecoverPassword(user));
            writeLog.Start();
        }

        public IEnumerable<User> GetUsersMigration()
        {
            var users = _userRepository.GetUsersMigration();

            return users;
        }

        private void AsyncInactiveAccountNotification(User user)
        {
            StringBuilder body = new StringBuilder();
            body.Append("<!DOCTYPE html>");
            body.Append("<html>");
            body.Append("<head lang=\"es\">");
            body.Append("    <meta charset=\"UTF-8\">");
            body.Append("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
            body.Append("    <title>Finnning</title>");
            body.Append("    <link href='http://fonts.googleapis.com/css?family=Roboto+Slab:400,700' rel='stylesheet' type='text/css'>");
            body.Append("    <link href='http://fonts.googleapis.com/css?family=Raleway:400,500,700,600,800,900,300' rel='stylesheet' type='text/css'>");
            body.Append("    <style type=\"text/css\">");
            body.Append("body {");
            body.Append("  font-family: 'Open Sans', sans-serif;");
            body.Append("}");
            body.Append(".container {");
            body.Append("  width: 600px;");
            body.Append("}");
            body.Append(".principal {");
            body.Append("  height: auto;");
            body.Append("  padding: 20px;");
            body.Append("  background-color: rgb(80, 80, 80);");
            body.Append("}");
            body.Append(".superior {");
            body.Append("  height: 110px;");
            body.Append("  padding-top: 10px;");
            body.Append("}");
            body.Append(".medio {");
            body.Append("  background-color: #FFF;");
            body.Append("  height: 220px;");
            body.Append("  padding-top: 10px;");
            body.Append("}");
            body.Append(".inferior {");
            body.Append("  height: 40px;");
            body.Append("  padding: 10px;");
            body.Append("  background-color: #F2F2F2;");
            body.Append("  border-top: 1px dashed rgb(100, 102, 102);");
            body.Append("  text-align: center;");
            body.Append("  vertical-align: middle;");
            body.Append("  border-bottom: 1px dashed rgb(100, 102, 102);");
            body.Append("  font-weight: 400;");
            body.Append("  color: #9F9F9B;");
            body.Append("  font-family: 'Roboto', sans-serif;");
            body.Append("}");
            body.Append(".footer {");
            body.Append("  height: 25px;");
            body.Append("  background-color: #FDC82F;");
            body.Append("  text-align: center;");
            body.Append("  font-weight: bold;");
            body.Append("  padding-top: 8px;");
            body.Append("}");
            body.Append("h3 {");
            body.Append("  font-family: 'Roboto', sans-serif;");
            body.Append("  color: #FFF;");
            body.Append("  font-size: 12px;");
            body.Append("  font-weight: 500;");
            body.Append("  padding: 0;");
            body.Append("  margin: 0;");
            body.Append("}");
            body.Append(".logo {");
            body.Append("  width: 160px;");
            body.Append("  padding-bottom: 10px;");
            body.Append("}");
            body.Append(".line {");
            body.Append("  padding: 8px;");
            body.Append("}");
            body.Append("a {");
            body.Append("  color: rgb(225, 178, 26);");
            body.Append("}");
            body.Append("a:hover {");
            body.Append("  color: #FDC82F;");
            body.Append("}");
            body.Append("    </style>");
            body.Append("</head>");
            body.Append("<body>");
            body.Append("<div class=\"container\">");
            body.Append("	<div class=\"principal\">");
            body.Append("		<div class=\"superior\">");
            body.Append("			<img src=\"http://www.finningsudamerica.com/Sitefinity/WebsiteTemplates/ThemeFinning/App_Themes/ThemeFinning/Images/logotipo_223x59.png\" class=\"logo\">");
            body.Append("		</div>");
            body.Append("	<div style='background-color:white'>");
            body.Append("          <br> Su cuenta ha sido desactivada ya que no presenta actividad en los últimos 3 meses.<br><br>");
            body.Append("	<div/>");
            body.Append(" 	<div class=\"footer\">");
            body.Append("			Atte.<br>");
            body.Append("			Monitoreo de Condiciones Centralizado Finning Chile.<br>");
            body.Append("			+56 55 2591012<br>");
            body.Append("			+56 55 2591394<br>");
            body.Append("			Cel +56 9 66287337<br>");
            body.Append("		</div>");
            body.Append("	</div>");
            body.Append("</div>");
            body.Append("</body>");
            body.Append("</html>");
            var email = new List<string>();
            email.Add(user.Email);
            _emailSender.SendHtmlMail(body.ToString(), email.ToArray(), null, "Argos - Cuenta Inactiva", null, null);
        }

        private void AsyncRecoverPassword(User user)
        {
            StringBuilder body = new StringBuilder();
            body.Append("<!DOCTYPE html>");
            body.Append("<html>");
            body.Append("<head lang=\"es\">");
            body.Append("    <meta charset=\"UTF-8\">");
            body.Append("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
            body.Append("    <title>Finnning</title>");
            body.Append("    <link href='http://fonts.googleapis.com/css?family=Roboto+Slab:400,700' rel='stylesheet' type='text/css'>");
            body.Append("    <link href='http://fonts.googleapis.com/css?family=Raleway:400,500,700,600,800,900,300' rel='stylesheet' type='text/css'>");
            body.Append("    <style type=\"text/css\">");
            body.Append("body {");
            body.Append("  font-family: 'Open Sans', sans-serif;");
            body.Append("}");
            body.Append(".container {");
            body.Append("  width: 600px;");
            body.Append("}");
            body.Append(".principal {");
            body.Append("  height: auto;");
            body.Append("  padding: 20px;");
            body.Append("  background-color: rgb(80, 80, 80);");
            body.Append("}");
            body.Append(".superior {");
            body.Append("  height: 110px;");
            body.Append("  padding-top: 10px;");
            body.Append("}");
            body.Append(".medio {");
            body.Append("  background-color: #FFF;");
            body.Append("  height: 220px;");
            body.Append("  padding-top: 10px;");
            body.Append("}");
            body.Append(".inferior {");
            body.Append("  height: 40px;");
            body.Append("  padding: 10px;");
            body.Append("  background-color: #F2F2F2;");
            body.Append("  border-top: 1px dashed rgb(100, 102, 102);");
            body.Append("  text-align: center;");
            body.Append("  vertical-align: middle;");
            body.Append("  border-bottom: 1px dashed rgb(100, 102, 102);");
            body.Append("  font-weight: 400;");
            body.Append("  color: #9F9F9B;");
            body.Append("  font-family: 'Roboto', sans-serif;");
            body.Append("}");
            body.Append(".footer {");
            body.Append("  height: 25px;");
            body.Append("  background-color: #FDC82F;");
            body.Append("  text-align: center;");
            body.Append("  font-weight: bold;");
            body.Append("  padding-top: 8px;");
            body.Append("}");
            body.Append("h3 {");
            body.Append("  font-family: 'Roboto', sans-serif;");
            body.Append("  color: #FFF;");
            body.Append("  font-size: 12px;");
            body.Append("  font-weight: 500;");
            body.Append("  padding: 0;");
            body.Append("  margin: 0;");
            body.Append("}");
            body.Append(".logo {");
            body.Append("  width: 160px;");
            body.Append("  padding-bottom: 10px;");
            body.Append("}");
            body.Append(".line {");
            body.Append("  padding: 8px;");
            body.Append("}");
            body.Append("a {");
            body.Append("  color: rgb(225, 178, 26);");
            body.Append("}");
            body.Append("a:hover {");
            body.Append("  color: #FDC82F;");
            body.Append("}");
            body.Append("    </style>");
            body.Append("</head>");
            body.Append("<body>");
            body.Append("<div class=\"container\">");
            body.Append("	<div class=\"principal\">");
            body.Append("		<div class=\"superior\">");
            body.Append("			<img src=\"http://www.finningsudamerica.com/Sitefinity/WebsiteTemplates/ThemeFinning/App_Themes/ThemeFinning/Images/logotipo_223x59.png\" class=\"logo\">");
            body.Append("		</div>");
            body.Append("	<div style='background-color:white'>");
            body.Append("			<b><h1>Recuperación de contraseña</h1></b><br><br>");
            body.Append("           Su nueva contraseña en el sistema Argos es: " + user.Password + "<br><br>");
            body.Append("	<div/>");
            body.Append(" 	<div class=\"footer\">");
            body.Append("			Atte.<br>");
            body.Append("			Monitoreo de Condiciones Centralizado Finning Chile.<br>");
            body.Append("			+56 55 2591012<br>");
            body.Append("			+56 55 2591394<br>");
            body.Append("			Cel +56 9 66287337<br>");
            body.Append("		</div>");
            body.Append("	</div>");
            body.Append("</div>");
            body.Append("</body>");
            body.Append("</html>");
            var email = new List<string>();
            email.Add(user.Email);
            _emailSender.SendHtmlMail(body.ToString(), email.ToArray(), null, "Argos - Recordatorio contraseña", null, null);
        }
    }
}
