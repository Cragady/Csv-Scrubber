﻿using System;

namespace Csv_Scrubber
{
    class Program
    {

        static bool Running = true;

        static void Main(string[] args)
        {           

            while(Running)
            {
                int command;
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("0: Quit\n1: Read File\n2: Write File\n3: Find potential parsing errors");
                // string command = Console.ReadLine();
                try
                {
                    command = Int32.Parse(Console.ReadLine());
                }
                catch
                {
                    command = -1;
                }
                switch(command)
                {
                    case 0:
                        Running = false;
                        break;
                    case 1:
                        try
                        {
                            Console.WriteLine("Please Enter the name of the file you want to read");
                            string path = "./txt-invoice-files/" + Console.ReadLine();
                            FileReader File = new FileReader(path);
                            File.WriteTextConsole(true);
                        }
                        catch
                        { Console.WriteLine("Unhandled Error"); continue; }
                        break;
                    case 2:
                        try
                        {
                            Console.WriteLine("Please Enter the name of the file you want to parse");
                            string path = "./txt-invoice-files/" + Console.ReadLine();
                            Console.WriteLine("Please Enter a name you want for the parsed file");
                            string dest = "./parsed-files/" + Console.ReadLine();
                            FileReader File = new FileReader(path, dest);
                            File.FileWrite();
                        }
                        catch { Console.WriteLine("Unhandled Error"); continue; }
                        break;
                    case 3:
                        try
                        {
                            Console.WriteLine("Please Enter the name of the file you want to read");
                            string path = "./txt-invoice-files/" + Console.ReadLine();
                            FileReader File = new FileReader(path);
                            File.WriteTextConsole(false);
                        }
                        catch { Console.WriteLine("Unhandled Error"); continue; }
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
