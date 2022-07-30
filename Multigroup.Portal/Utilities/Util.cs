using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Models.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Multigroup.Portal.Utilities
{
    public class Util
    {
        public static string viewCarguioRules = "CarguioRules";
        public static string viewOperator = "Operator";
        public static string viewHealth = "Health";
        public static string viewWorkload = "Workload";
        public static string viewMaintenance = "Maintenance";
        public static string viewOperatingHour = "OperatingHour";
        public static string viewRalentiPercentage = "RalentiPercentage";

        public static string formatPdf = "application/pdf";
        public static string formatExcel = "application/excel";

        public static string GetFrenchFormatDate(DateTime? date)
        {
            return (date != null) ? date.Value.ToString("dd/MM/yyyy") : null;
        }
        public static List<BaseEntity> GetMonths()
        {
            List<BaseEntity> months = new List<BaseEntity>();
            BaseEntity enero = new BaseEntity();
            BaseEntity febrero = new BaseEntity();
            BaseEntity marzo = new BaseEntity();
            BaseEntity abril = new BaseEntity();
            BaseEntity mayo = new BaseEntity();
            BaseEntity junio = new BaseEntity();
            BaseEntity julio = new BaseEntity();
            BaseEntity agosto = new BaseEntity();
            BaseEntity septiembre = new BaseEntity();
            BaseEntity octubre = new BaseEntity();
            BaseEntity noviembre = new BaseEntity();
            BaseEntity diciembre = new BaseEntity();
            enero.Id = 1;
            enero.Name = "Enero";
            febrero.Id = 2;
            febrero.Name = "Febrero";
            marzo.Id = 3;
            marzo.Name = "Marzo";
            abril.Id = 4;
            abril.Name = "Abril";
            mayo.Id = 5;
            mayo.Name = "Mayo";
            junio.Id = 6;
            junio.Name = "Junio";
            julio.Id = 7;
            julio.Name = "Julio";
            agosto.Id = 8;
            agosto.Name = "Agosto";
            septiembre.Id = 9;
            septiembre.Name = "Septiembre";
            octubre.Id = 10;
            octubre.Name = "Octubre";
            noviembre.Id = 11;
            noviembre.Name = "Noviembre";
            diciembre.Id = 12;
            diciembre.Name = "Diciembre";
            months.Add(enero);
            months.Add(febrero);
            months.Add(marzo);
            months.Add(abril);
            months.Add(mayo);
            months.Add(junio);
            months.Add(julio);
            months.Add(agosto);
            months.Add(septiembre);
            months.Add(octubre);
            months.Add(noviembre);
            months.Add(diciembre);

            return months;
        } 
        public static string GetFrenchFormatDateTime(DateTime date)
        {
            return (date != null) ? date.ToString("dd/MM/yyyy HH:mm") : null;
        }

        public static string GetFrenchFormatDateTimeNull(DateTime? date)
        {
            return (date != null) ? date.Value.ToString("dd/MM/yyyy HH:mm") : null;
        }

        public static double RoudDouble(Double d)
        {
            double dReturn = Math.Round(d);
            return dReturn;
        }

        public static DateTime? ParseDateTime(string dateTime)
        {
            DateTime? date;
            date = (!string.IsNullOrEmpty(dateTime)) ? (DateTime?)Convert.ToDateTime(dateTime) : null;

            return date;
        }
        //public static DateTime? ParseDateTimeAmPm(string dateTime)
        //{
        //    DateTime? date;
        //    date = (!string.IsNullOrEmpty(dateTime)) ? (DateTime?)DateTime.ParseExact(dateTime, "dd/mm/aaaa h:mm tt", CultureInfo.InvariantCulture): null;

        //    return date;
        //}
        public static int? ParseIntNullable(IEnumerable<string> value)
        {
            if (value != null)
            {
                return (int?)int.Parse(value.FirstOrDefault());
            }
            else return null;
        }

        public static string ValidateArrayValues(IEnumerable<string> array1, IEnumerable<string> array2)
        {
            if (array1 != null && array2 != null)
            {
                if (array1.Intersect(array2).Any())
                {
                   return  String.Join(",", array1.Intersect(array2).ToList());
                }
            }
            return "";
        }

        public static int? ParseIntNullable(IEnumerable<int> value)
        {
            if (value != null)
            {
                return (int?)value.FirstOrDefault();
            }
            else return null;
        }

        public static int? ParseIntNullable(string value)
        {
            if (value != null)
            {
                return (int?)int.Parse(value);
            }
            else return null;
        }

        public static int ParseInt(IEnumerable<string> value)
        {
            if (value != null)
            {
                return int.Parse(value.FirstOrDefault());
            }
            else return 0;
        }

        public static string IsNullOrEmpty(string value)
        {
            return !string.IsNullOrEmpty(value) ? value : null; 
        }

        public static double? TryParseToDouble(string value)
        {
            double i;
            bool result = Double.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out i);
            
            if (result == true)
            {
                return i;
            }
            else
            {
                return null;
            }
        }

        public static string Replace(string value, string oldValue, string newValue )
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace(oldValue, newValue);
                return value;
            }
            else
                return null;
        }

        public static IEnumerable<DomainModel.Shared.DayOfWeek> GetDayNames()
        {
            if (CultureInfo.CurrentCulture.Name.StartsWith("en-"))
            {
                return GetDaysOfWeek(new[] { "Monday", "Tuesday", "Wednesday", "Thursday",
                        "Friday", "Saturday", "Sunday" });
            }
            else
            {
                return GetDaysOfWeek(CultureInfo.CurrentCulture.DateTimeFormat.DayNames);
            }
        }

        private static IEnumerable<DomainModel.Shared.DayOfWeek> GetDaysOfWeek(string[] days)
        {
            var list = new List<DomainModel.Shared.DayOfWeek>();
            for(int i=0; i< days.Length; i++ )
            {
                list.Add(new DomainModel.Shared.DayOfWeek { Id = i + 1, Name = days[i] });
            }
            return list;
        }

        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
    }

    public static class DateTimeExtensions
    {
        public static string ToYMD(this DateTime theDate)
        {
            return theDate.ToString("yyyyMMdd");
        }

        public static string ToYMD(this DateTime? theDate)
        {
            return theDate.HasValue ? theDate.Value.ToYMD() : string.Empty;
        }

        public static DateTime FirstDayOfWeek(this DateTime dt)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
                diff += 7;
            return dt.AddDays(-diff).Date.AddDays(1);
        }

        public static DateTime LastDayOfWeek(this DateTime dt)
        {
            return dt.FirstDayOfWeek().AddDays(6);
        }

        public static DateTime LastDayOfCurrentWeek(this DateTime dt)
        {
            if (dt > DateTime.Now)
            {
                return LastDayOfWeek(dt);
            }
            
            var date = dt.FirstDayOfWeek().AddDays(6);

            if (date > DateTime.Now)
                return DateTime.Now;
            else
                return date;
        }

        public static DateTime FirstDayOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime dt)
        {
            return dt.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        public static DateTime FirstDayOfNextMonth(this DateTime dt)
        {
            return dt.FirstDayOfMonth().AddMonths(1);
        }


    }
}