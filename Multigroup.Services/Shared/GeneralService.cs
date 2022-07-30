using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO; 

namespace Multigroup.Services.Shared
{
    public class GeneralService
    {
        private GeneralRepository _generalRepository;

        public GeneralService()
        {
            _generalRepository = new GeneralRepository();
        }

        /// <summary>
        /// Genera una copia de la tabla componentStatuData registrando el estado de los componentes en un momento
        /// determinado. Si recibe un id de faena por paremtro lo hace para esa faena.. si no recibe idFaena
        /// lo realiza para todas las faenas que tenga configurado el corte semanal en el día 
        /// 
        /// </summary>
        public void InsertComponentStatusDataLog(int? SiteId)
        {
            _generalRepository.InsertComponentStatusDataLog(SiteId);
        }

        /// <summary>
        /// Genera una copia de la tabla RoutineStatuData registrando el estado de los componentes en un momento
        /// determinado. Si recibe un id de faena por paremtro lo hace para esa faena.. si no recibe idFaena
        /// lo realiza para todas las faenas que tenga configurado el corte semanal en el día 
        /// 
        /// </summary>
        public void InsertRoutineStatusDataLog(int? SiteId)
        {
            _generalRepository.InsertRoutineStatusDataLog(SiteId);
        }

        /// <summary>
        /// Inserto un registro en la base de navegación de usuarios
        /// </summary>
        /// <param name="_navigation"></param>
        public void InsertNavigationLog(NavigationLog _navigation)
        {
            _generalRepository.InsertNavigationLog(_navigation);
        }


        public string Encriptar(string _cadenaAencriptar)
        {
            string EncryptionKey = "INGARXMG";
            string encriptar = _cadenaAencriptar;

            byte[] clearBytes = Encoding.Unicode.GetBytes(encriptar);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encriptar = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encriptar;
        }

        public string DesEncriptar(string _cadenaAdesencriptar)
        {
            string EncryptionKey = "INGARXMG";
            byte[] cipherBytes = Convert.FromBase64String(_cadenaAdesencriptar.Replace(" ", "+"));
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    _cadenaAdesencriptar = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return _cadenaAdesencriptar;
        }

        public string ClaveEncriptar(string nombre, string apellido, string dni, string numeroContrato)
        {
            string EncryptionKey = "INGARXMG";
            string encriptar = nombre + " " + apellido + " " + dni + " " + numeroContrato + " " + DateTime.Now.ToShortDateString();

            byte[] clearBytes = Encoding.Unicode.GetBytes(encriptar);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encriptar = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encriptar;
        }
    }
}
