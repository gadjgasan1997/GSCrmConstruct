using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSCrmLibrary
{
    public static class Utils
    {
        public static string SearchSpecificationConversion(string searchSpecification, dynamic currentRecord)
        {
            string conversionSearchSpecification = string.Empty;
            foreach (string element in searchSpecification.Split("[&"))
            {
                if (element.Contains("]"))
                {
                    string[] elementParts = element.Split("]");
                    string property = string.Empty;
                    string endOfElement = string.Empty;
                    foreach (char c in element.TakeWhile(s => s.ToString() != "]"))
                        property += c;
                    foreach (char c in element.SkipWhile(s => s.ToString() != "]").Skip(1))
                        endOfElement += c;
                    dynamic value = currentRecord.GetType().GetProperty(property).GetValue(currentRecord);
                    if (value != null)
                        conversionSearchSpecification += $"\"{value.ToString()}\"{endOfElement}";
                    else conversionSearchSpecification += $"\"null\"";
                }
                else conversionSearchSpecification += element;
            }
            return conversionSearchSpecification;
        }
        public static string GetPermissibleName(string entityName) => entityName.Replace(' ', '_');
        public static Dictionary<string, string> GetErrorsInfo(Exception ex)
        {
            return new Dictionary<string, string>()
            {
                { "Target", $"Target: {ex.TargetSite}" },
                { "Message", $"Message: {ex.Message}" },
                { "Inner exceptin", $"Inner exceptin: {ex.InnerException?.Message}" },
            };
        }
    }
}
