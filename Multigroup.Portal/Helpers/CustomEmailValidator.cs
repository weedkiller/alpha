using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Multigroup.Portal.Helpers
{
    public class CustomEmailValidator  
    {
        public static string Validate(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    var emails = value.ToString().Split(';');
                    foreach (var email in emails)
                    {
                        if (!Regex.IsMatch(email, @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", RegexOptions.IgnoreCase))
                        {
                            return "Ingrese un email valido";
                        }
                    }
                    return string.Empty;
                }
                catch
                {
                    return "Ingrese un email valido";
                }
            }

            return string.Empty;
        }
    }
}