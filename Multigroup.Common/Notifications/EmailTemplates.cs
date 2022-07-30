using Multigroup.DomainModel.Shared;
using System;
using System.Configuration;

namespace MCC.Common.Notifications
{
    public static class EmailTemplates
    {
        private static readonly String WelcomeSubject = "Bienvenido a MG";
        private static readonly String WelcomeBody = "<p>ESTIMADO CLIENTE:</p><p>MEDIANTE LA PRESENTE QUEREMOS DARLE LA BIENVENIDA A&nbsp;<strong>MG</strong>. AGRADECER SU CONFIANZA DEPOSITADA EN NOSOTROS, PONIENDO COMO OBJETIVO OBTENER EL BIEN SOLICITADO.</p><p>QUEDAMOS A SU DISPOSICION EN LAS SIGUIENTES DIRECCIONES Y TELEFONOS PARA CUALQUIER TIPO DE DUDA O CONSULTA QUE DESEE REALIZAR.</p><p>&nbsp;</p><p>INFORMACI&Oacute;N DE CONTACTO:</p><ul><li>General Paz 233 Piso 2. TEL&Eacute;FONO: (0351) 422-6394 (casa central)</li></ul><p>&nbsp;</p><p>&nbsp;LE SALUDA ATENTAMENTE&nbsp;<strong>MG</strong></p>";
        private static readonly String WelcomeAttach = "Bienvenido.docx";

        private static readonly String ReminderSubject = "Recordatorio de pago";
        private static readonly String ReminderBody = "<p>SR . CLIENTE LE INFORMAMOS QUE EL PAGO DE SU PLAN ESTA PR&Oacute;XIMO A VENCERSE,ROGAMOS EVITE RECARGOS INNECESARIOS CUMPLIENDO EN TIEMPO Y FORMA .</p><p>NUESTROS MEDIOS DE PAGO Y DE COMUNICACI&Oacute;N ANTES CUALQUIER INCONVENIENTE.</p><p>CASA CENTRAL ; General Paz 233 Piso 2 C&Oacute;RDOBA.</p><p>TEL FIJO:&nbsp; (0351) 422-6394&nbsp;</p><div><p>RECUERDE QUE CUENTA CON EL SERVICIO DE COBRO EN SU DOMICILIO, SOLO DEBE CORROBORAR DISPONIBILIDAD EN SU ZONA.</p><p>EN CASO DE HABER REALIZADO SU PAGO EN EL CORRIENTE MES DESESTIME ESTE MENSAJE.</p></div>";

        private static readonly String IntimationSubject = "Intimación de pago";
        private static readonly String IntimationBody = "<p>SR CLIENTE, AL MOMENTO NO HEMOS REGISTRADOS SU PAGO Y NOS VEMOS EN LA NECESIDAD DE INFORMARLE QUE LOS MISMOS LLEVAR&Aacute;N UN INTER&Eacute;S EL CUAL SER&Aacute; INFORMADO AL MOMENTO EN QUE USTED REGULARICE SU SITUACI&Oacute;N.</p><p>ROGAMOS SE COMUNIQUE CON NOSOTROS LO ANTES POSIBLE.&nbsp;</p>";

        private static readonly String PaymentSubject = "Registro de pago";
        private static readonly String PaymentBody = "<p>SR. %user_name% LE INFORMAMOS QUE EN EL D&Iacute;A&nbsp; %payment_date% HEMOS REGISTRADO EL SIGUIENTE PAGO:</p><p>PESOS : $ %payment_amount%</p>";

        private static readonly String CancelationSubject = "Solicitud de Baja";
        private static readonly String CancelationBody = "<p>Sr. Cliente nos contactamos con el motivo de Informarle el procedimiento para realizar la baja voluntaria de su plan.<br />Adjuntamos documento donde usted debe completar con sus datos, firmar y devolver escaneado por este medio.<br />Agradecemos su atenci&oacute;n.<br />MG.</p>";
        private static readonly String CancelationAttach = "cancelation.docx";

        private static readonly String SignSubject = "Contrato Firmado";
        private static readonly String SignBody = "<p>Sr. %user_name% nos contactamos con el motivo de enviarle su contrato Firmado<br />Adjuntamos Contrato en formato PDF.<br />Agradecemos su atenci&oacute;n.<br />Mg.</p>";
        private static readonly String SignAttach = "contract.docx";

        private static readonly String NewContractSubject = "Nueva Adhesión de cliente %user_name%";
        private static readonly String NewContractBody = "<p>SR. %user_name% nos contactamos con el motivo de Informarle el procedimiento para realizar la adhesi&oacute;n a MG.<br />Adjuntamos link donde usted debera firmar y adjuntar documentaci&oacute;n solicitada.<br />Agradecemos su atenci&oacute;n.<br />MG.</p> <br><br><br> <p><a href='%link%'>%link%</a></p>";

        private static readonly String ContractSubject = "Contrato MG";
        private static readonly String ContractBody = "<p>SR. %user_name% nos contactamos con el motivo de Informarle que su adhesi&oacute;n a MG se ha realizado exitosamente.<br />Adjuntamos contrato<br />Agradecemos su atenci&oacute;n.<br />MG.</p>";
        private static readonly String ContractAttach = "contrato.docx";

        private static readonly String AdjudicationSubject = "Solicitud de Adjudicación";
        private static readonly String AdjudicationBody = "<p>SR CLIENTE,&nbsp;&nbsp;BIENVENIDO AL PROCESO DE ADJUDICACION, SE ENVIA ADJUNTA LA INFORMACION SOBRE EL MISMO.</p>";
        private static readonly String AdjudicationAttach = "adjudication.docx";

        private static readonly String DebtCancelationSubject = "Baja por deuda";
        private static readonly String DebtCancelationBody = "<p>Sr. Cliente : nos cont&aacute;ctamos con usted para informarle que debido a la falta de pago de su plan, se procede a la baja del mismo.</p>";

        private static readonly String CancelationWebSubject = "Cancelación de Contraro";
        private static readonly String CancelationWebBody = "<p>Sr. Cliente nos contactamos con el motivo de Informarle que su contrato ha sido de baja y deja de estar vigente.<br /Agradecemos su atenci&oacute;n.<br />MG.</p>";

        private static readonly String ScoringContractSubject = "Declaración Jurada MG";
        private static readonly String ScoringContractBody = "<p>Estimado/a %user_name%<br /><br /> Para confirmarsu contaro debe firmar la siguiente declaración jurada.</p> <br><br><br> <p> Por favor ingrese al siguiente link <br><br> <a href='%link%'>%link%</a><br /><br /> Agradecemos su atenci&oacute;n.<br />MG.</p>";



        public static String GetScoringContractSubject(Customer customer)
        {
            return ScoringContractSubject.Replace("%user_name%", customer.Surname + " " + customer.Name);
        }

        public static String BuildScoringContractBody(Customer customer, string link)
        {
            return GetScoringContractBody()
                .Replace("%user_name%", customer.Surname + " " + customer.Name).Replace("%link%", link);
        }

        public static String GetScoringContractBody()
        {
            return ScoringContractBody;
        }


        public static String GetWelcomeBody()
        {
            return WelcomeBody;
        }

        public static String GetWelcomeSubject()
        {
            return WelcomeSubject;
        }

        public static String BuildWelcomeBody(Customer customer)
        {
            return GetWelcomeBody().Replace("%user_name%", customer.Surname + " " + customer.Name); ;
        }

        public static String GetReminderBody()
        {
            return ReminderBody;
        }

        public static String GetReminderSubject()
        {
            return ReminderSubject;
        }

        public static String BuildReminderBody()
        {
            return GetReminderBody();
        }

        public static String GetIntimationBody()
        {
            return IntimationBody;
        }

        public static String GetIntimationSubject()
        {
            return IntimationSubject;
        }

        public static String BuildIntimationBody()
        {
            return GetIntimationBody();
        }

        public static String GetPaymentBody()
        {
            return PaymentBody;
        }

        public static String GetNewContractBody()
        {
            return NewContractBody;
        }

        public static String GetPaymentSubject()
        {
            return PaymentSubject;
        }

        public static String BuildPaymentBody(Customer customer, Payment payment)
        {
            return GetPaymentBody()
                .Replace("%user_name%", customer.Surname)
                .Replace("%payment_date%", payment.PaymentDate.ToString())
                .Replace("%payment_amount%", payment.Amount.ToString());
        }


        public static String GetNewContractSubject(Customer customer)
        {
            return NewContractSubject.Replace("%user_name%", customer.Surname + " " + customer.Name);
        }

        public static String BuildNewContractBody(Customer customer, string link)
        {
            return GetNewContractBody()
                .Replace("%user_name%", customer.Surname + " " + customer.Name).Replace("%link%", link);
        }

        public static String GetContractBody()
        {
            return ContractBody;
        }

        public static String GetContractSubject()
        {
            return ContractSubject;
        }

        public static String BuildContractBody(Customer customer)
        {
            return GetPaymentBody()
                .Replace("%user_name%", customer.Surname);
        }

        public static String GetCancelationBody()
        {
            return CancelationBody;
        }

        public static String GetSignBody()
        {
            return SignBody;
        }

        public static String GetCancelationSubject()
        {
            return CancelationSubject;
        }

        public static String GetSingSubject()
        {
            return SignSubject;
        }


        public static String GetCancelationAttachPath()
        {
            String attachPath = ConfigurationManager.AppSettings["AttachmentPath"];
            String fullPath = System.AppDomain.CurrentDomain.BaseDirectory + attachPath;
            String filePath = System.IO.Path.Combine(fullPath.ToString(), CancelationAttach);

            return filePath;
        }

        public static String GetWelcomeAttachPath()
        {
            String attachPath = ConfigurationManager.AppSettings["AttachmentPath"];
            String fullPath = System.AppDomain.CurrentDomain.BaseDirectory + attachPath;
            String filePath = System.IO.Path.Combine(fullPath.ToString(), WelcomeAttach);

            return filePath;
        }

        public static String BuildCancelationBody()
        {
            return GetCancelationBody();
        }

        public static String BuildSignBody(Customer customer)
        {
            return GetSignBody().Replace("%user_name%", customer.Surname + " " + customer.Name); ;
        }

        public static String GetAdjudicationBody()
        {
            return AdjudicationBody;
        }

        public static String GetAdjudicationSubject()
        {
            return AdjudicationSubject;
        }

        public static String GetCancelationWebSubject()
        {
            return CancelationWebSubject;
        }

        public static String BuildCancelationWebBody()
        {
            return GetCancelationWebBody();
        }

        public static String GetCancelationWebBody()
        {
            return CancelationWebBody;
        }


        public static String GetAdjudicationAttachPath()
        {
            String attachPath = ConfigurationManager.AppSettings["AttachmentPath"];
            String fullPath = System.AppDomain.CurrentDomain.BaseDirectory + attachPath;
            String filePath = System.IO.Path.Combine(fullPath.ToString(), AdjudicationAttach);

            return filePath;
        }

        public static String BuildAdjudicationBody()
        {
            return GetAdjudicationBody();
        }

        public static String GetDebtCancelationBody()
        {
            return DebtCancelationBody;
        }

        public static String GetDebtCancelationSubject()
        {
            return DebtCancelationSubject;
        }

        public static String BuildDebtCancelationBody()
        {
            return GetDebtCancelationBody();
        }

    }
}
