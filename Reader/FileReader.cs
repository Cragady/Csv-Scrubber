using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Csv_Scrubber
{
    public class FileReader
    {
        // string[] Text = System.IO.File.ReadAllLines("../Sales-Invoicing.csv");
        // string[] Text = File.ReadAllLines(@"C:\Users\DreamWalker64\dev2\Cascade\Sales-Invoicing.csv");
        private string[] Text;
        public string[] text
        {
            get
            {
                return Text;
            }
        }

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

        public FileReader(bool placeHolder){}

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
                    FilteredText[FilteredText.Count - 1] += String.Join(",&|,", bucket);
                    ParseError = false;
                } 
                else if(bucketPass)
                {
                    FilteredText.Add(String.Join(",&|,", bucket));
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

        public string[,] CsvToArrayList()
        {
            int headRow = Text[0].Split(",&|,").Length;
            string[,] listPass = new string[Text.Length, Text[0].Split(",&|,").Length];
            
            for(int i = 0; i < Text.Length; i++)
            {
                string[] bucket = Text[i].Split(",&|,");
                for(int j = 0; j < bucket.Length; j++)
                {
                    if(bucket[j].Length > 0)
                    {
                        bucket[j] = bucket[j].Replace(",", "");
                        if(bucket[j].Substring(0, 1) == "(" && bucket[j].Substring(bucket[j].Length - 1) == ")")
                        {
                            bucket[j] = bucket[j].Replace("(", "-").Replace(")", "");
                        }
                        if(bucket[j].Contains("$"))
                        {
                            bucket[j] = bucket[j].Replace("$", "");
                        }
                    }
                    
                    listPass[i, j] = bucket[j];
                }
            }

            return listPass;
        }

        public void PreFileWrite(string destinationFile)
        {
            string[] lineCorrection = Text[1].Split("|");
            for(int i = 0; i < lineCorrection.Length; i++)
            {
                if(i == 0 || i == lineCorrection.Length - 1) continue;
                int oldLength = lineCorrection[i].Length;
                lineCorrection[i] = lineCorrection[i].Trim() + " " + (i - 1);
                int spaceCorrection = (oldLength - lineCorrection[i].Length) / 2;
                string spaceInserts = "";
                if(lineCorrection[i].Length % 2 != 0)
                {
                    lineCorrection[i] += " ";
                }
                for(int j = 0; j < spaceCorrection; j++)
                {
                    spaceInserts += " ";
                }
                lineCorrection[i] = spaceInserts + lineCorrection[i];
                lineCorrection[i] += spaceInserts;
            }
            Text[1] = String.Join("|", lineCorrection);

            File.WriteAllLines(destinationFile, Text);
        }
    }
}