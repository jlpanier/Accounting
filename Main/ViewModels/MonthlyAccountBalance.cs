using System;
using System.Collections.Generic;
using System.Text;

namespace Main.ViewModels
{
    public class MonthlyAccountBalance
    {
        public string Month { get; set; }      // "Janvier", "Février", ...
        public decimal Account1 { get; set; }  // Compte courant
        public decimal Account2 { get; set; }  // Livret A
        public decimal Account3 { get; set; }  // PEL (exemple)
    }

}
