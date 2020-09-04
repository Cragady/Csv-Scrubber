using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Csv_Scrubber
{
    public class FileReader
    {
        // string[] Text = System.IO.File.ReadAllLines("../Sales-Invoicing.csv");
        // string[] Text = File.ReadAllLines(@"C:\Users\DreamWalker64\dev2\Cascade\Sales-Invoicing.csv");
        private string[] Text;
        private string destinationFile;
        private List<string> FilteredText = new List<string>();
        private int PotentialParseError = 0;
        private bool ParseError = false;
        private bool ParseErrorThrown = false;

        public FileReader(){
            try
            {
                Text = File.ReadAllLines("../Sales-Invoicing.csv");
            }
            catch (Exception err)
            {
                Console.WriteLine("Could not find file. To see full error, press \"y\". To exit this message, press anything else.");
                if(Console.ReadLine() == "y"){
                    Console.WriteLine(err);
                };
            }
        }

        public FileReader(string path){
            try
            {
                Text = File.ReadAllLines(path);
            }
            catch (Exception err)
            {
                Console.WriteLine("Could not find file. To see full error, press \"y\". To exit this message, press anything else.");
                if(Console.ReadLine() == "y"){
                    Console.WriteLine(err);
                };
            }
        }

        public FileReader(string path, string dest)
        {
            try
            {
                Text = File.ReadAllLines(path);
                destinationFile = dest;
            }
            catch (Exception err)
            {
                Console.WriteLine("Could not find file. To see full error, press \"y\". To exit this message, press anything else.");
                if(Console.ReadLine() == "y"){
                    Console.WriteLine(err);
                };
            }
        }

        public void ParseErrorWarn()
        {
            Console.WriteLine("Potential parsing error found, count: " + PotentialParseError);
        }

        public void WriteTextConsole(bool console)
        {
            if(Text.Length == 0) return;

            for(int i = 0; i < Text.Length; i++){
                if(Text[i].Substring(0, 2) == "--"){
                    continue;
                };

                if(Text[i].Substring(0, 2) == " |" || Text[i].Substring(0, 2) != "| ")
                {
                    ParseError = true;
                    ParseErrorThrown = true;
                    PotentialParseError++;
                }

                string[] bucket = Text[i].Split("|");
                bucket = bucket.Skip(1).ToArray();
                bucket = bucket.Take(bucket.Length - 1).ToArray();
                bool bucketPass = true;

                for(int j = 0; j < bucket.Length; j++)
                {
                    bucket[j] = bucket[j].Trim();
                }

                if(bucket[0] == "")
                {
                    if(bucket.All((str) => { return str == ""; } )) 
                    {
                        bucketPass = false;
                    }
                }

                if(ParseError && bucketPass)
                {
                    FilteredText[FilteredText.Count - 1] += String.Join(", ", bucket);
                    ParseError = false;
                } 
                else if(bucketPass)
                {
                    FilteredText.Add(String.Join(", ", bucket));
                }
            }

            if(console)
            {
                for(int i = 0; i < FilteredText.Count; i++)
                {
                    Console.WriteLine(FilteredText[i]);
                }
            }

            if(ParseErrorThrown)
            {
                ParseErrorWarn();
            }
            else 
            {
                Console.WriteLine("Parsing completed. No forseeable problems found.");
            }
        }

        public void FileWrite()
        {
            WriteTextConsole(false);
            File.WriteAllLines(destinationFile, FilteredText);
            Console.WriteLine("Written to: " + destinationFile);
        }
    }
}