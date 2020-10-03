using GSCrm.Data;
using GSCrm.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GSCrm.Helpers
{
    public static class QuoteHelpers
    {
        private static int QUOTE_NUMBER_LENGTH = 6;
        public static string GetNewNumber(this QuoteViewModel quoteViewModel, ApplicationDbContext context)
        {
            string number = string.Empty;
            while (number == string.Empty || context.Quotes.FirstOrDefault(n => n.Number == number) != null)
                number = GenerateNewNumnber();
            return number;
        }

        private static string GenerateNewNumnber()
        {
            string number = string.Empty;
            for (int i = 0; i < QUOTE_NUMBER_LENGTH; i++)
                number += new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }[new Random().Next(0, 9)];
            return number;
        }
    }
}
