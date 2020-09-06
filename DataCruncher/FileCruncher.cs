using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Csv_Scrubber
{

    enum eLink { CaMain = 0, CaLinker = 8, ChMain = 7, ChLinker = 16, PuMain = 2, PuLinker = 16,  SaMain = 0, SaLinker = 23, SaLinker2 = 174 }

    public class FileCruncher
    {

        private FileReader Cash;
        private FileReader Checks;
        private FileReader Purchases;
        private FileReader Sales;

        private string[,] CashLA;
        private string[,] ChecksLA;
        private string[,] PurchasesLA;
        private string[,] SalesLA;

        public string[,] cash { get{ return CashLA; } }
        public string[,] checks { get { return ChecksLA; } }
        public string[,] purchases { get { return PurchasesLA; } }
        public string[,] sales { get { return SalesLA; } }

        public FileCruncher()
        {
            Cash = new FileReader("./parsed-files/Cash-Parsed.csv");
            Checks = new FileReader("./parsed-files/Checks-Parsed.csv");
            Purchases = new FileReader("./parsed-files/Purchases-Parsed.csv");
            Sales = new FileReader("./parsed-files/Sales-Parsed.csv");

            TextToList();
        }

        public void TextToList()
        {
            CashLA = Cash.CsvToArrayList();
            ChecksLA = Checks.CsvToArrayList();
            PurchasesLA = Purchases.CsvToArrayList();
            SalesLA = Sales.CsvToArrayList();
            
            Console.WriteLine("DONE WITH STORING IN MEMORY");
        }

    }
}