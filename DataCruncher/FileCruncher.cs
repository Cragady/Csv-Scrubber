using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Csv_Scrubber
{

    /*
        This class will crash the program if you cloned.
        Leaving it in for reasons. Just rework this class
        if you need your own custom data-crunching. Your
        files exported from Microsoft Access will be 
        different from mine.
    */

    enum eLink { CaMain = 0, CaLinker = 8, CaAm = 26,
        ChMain = 7, ChLinker = 16, ChAm = 8,
        PuMain = 2, PuLinker = 16, PuAm = 15,
        SaMain = 0, SaLinker = 23, SaLinker2 = 174 }

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
        private double[] SalesMi;

        private int caLength;
        private int chLength;
        private int puLength;
        private int saLength;

        public string[,] cash { get{ return CashLA; } }
        public string[,] checks { get { return ChecksLA; } }
        public string[,] purchases { get { return PurchasesLA; } }
        public string[,] sales { get { return SalesLA; } }
        public double[] salesMi { get { return SalesMi; } }

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

            caLength = CashLA.GetLength(0);
            chLength = ChecksLA.GetLength(0);
            puLength = PurchasesLA.GetLength(0);
            saLength = SalesLA.GetLength(0);

            SalesMi = new double[saLength];

            for(int i = 0; i < saLength; i++)
            {
                if(i == 0) continue;
                SalesMi[i] = SaTo(i);
            }
            
            Console.WriteLine("DONE WITH STORING IN MEMORY");
        }

        public double SaTo(int arrIndex)
        {
            double SaAm = 0;

            for(int i = 142; i < 158; i++)
            {
                if(SalesLA[arrIndex, i] != ""){
                    if(arrIndex == 3753) 
                    {
                        Console.WriteLine(SalesLA[arrIndex, i]);
                    }
                    SaAm += Double.Parse(SalesLA[arrIndex, i]);
                    if(arrIndex == 3753) Console.WriteLine(SaAm);
                }
            }
            return SaAm;
        }

        public void CrunchSData()
        {
            int pSaProb = 0;
            int saEmLink = 0;
            bool cashLinked = false;
            int cashIndex = -1;

            for(int i = 1; i < saLength; i++)
            {
                cashLinked = false;
                for(int j = 1; j < caLength; j++)
                {
                    if(CashLA[j, (int)eLink.CaLinker] == SalesLA[i, (int)eLink.SaMain])
                    {
                        cashIndex = j;
                        cashLinked = true;
                        continue;
                    }
                    else if(j == caLength - 1)
                    {
                        if(SalesLA[i, (int)eLink.SaLinker] == "" 
                            && SalesLA[i, (int)eLink.SaLinker2] != ""
                            && SalesMi[i] != 0)
                        {
                            saEmLink++;
                        }
                    }
                }
                if(cashLinked)
                {
                    double calSa = Math.Floor(SalesMi[i]);
                    double calCa = Math.Floor(double.Parse(CashLA[cashIndex, (int)eLink.CaAm]));

                    if(!(calCa - 1 <= calSa && calSa <= calCa + 1) && SalesLA[i, (int)eLink.SaLinker2] != "")
                    {
                        Console.WriteLine("Estimated from sales: " + calSa + " Estimated Cash Slip: " + calCa + " Sales Number: " + SalesLA[i, (int)eLink.SaMain] + " Cash number: " + CashLA[cashIndex, (int)eLink.CaMain]);
                        pSaProb++;
                    }
                }
            }
            Console.WriteLine(saLength + " Records assessed.");
            Console.WriteLine("Potential Sales Problems: " + pSaProb);
            Console.WriteLine("Sales Empty Link Field: " + saEmLink);
        }

        public void CrunchPData()
        {
            int pPuProb = 0;
            int puEmLink = 0;
            bool cashLinked = false;
            int cashIndex = -1;

            for(int i = 1; i < puLength; i++)
            {
                cashLinked = false;
                for(int j = 1; j < caLength; j++)
                {
                    if(CashLA[j, (int)eLink.CaLinker] == PurchasesLA[i, (int)eLink.PuMain])
                    {
                        cashIndex = j;
                        cashLinked = true;
                        continue;
                    }
                    else if(j == caLength - 1)
                    {
                        if(PurchasesLA[i, (int)eLink.PuLinker] == ""
                            && PurchasesLA[i, (int)eLink.PuAm] != "")
                        {
                            puEmLink++;
                        }
                    }
                }
                if(cashLinked)
                {
                    double calPu = Math.Floor(double.Parse(PurchasesLA[i, (int)eLink.PuAm]));
                    calPu *= -1;
                    double calCa = Math.Floor(double.Parse(CashLA[cashIndex, (int)eLink.CaAm]));

                    if(!(calCa - 1 <= calPu && calPu <= calCa + 1)
                        && Int32.Parse(PurchasesLA[i, (int)eLink.PuMain]) > 305610)
                    {
                        Console.WriteLine("Estimated from purchases: " + calPu + " Estimated Cash Slip: " + calCa + " Purchase Number: " + PurchasesLA[i, (int)eLink.PuMain] + " Cash number: " + CashLA[cashIndex, (int)eLink.CaMain]);
                        pPuProb++;
                    }
                }
            }

            Console.WriteLine(puLength + " Records assessed.");
            Console.WriteLine("Potential Purchase Problems: " + pPuProb);
            Console.WriteLine("Purchases Empty Link Field: " + puEmLink);

        }

    }
}