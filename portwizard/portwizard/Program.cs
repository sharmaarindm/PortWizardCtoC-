/*
* FILE : Program.cs
* PROJECT : SQII - Assignment portwizard
* PROGRAMMER : Arindm Sharma & Zivojin Pecin
* DESCRIPTION : This file checks if there is enough arguments provided, once that it completed it will take the name of inputted file
* and save it as the namescape for the c# file. Then it will read line by line and write it to the output file with the proper syntax.
*/


using System;
using System.Text.RegularExpressions;
using System.IO;

namespace portwizard
{
    class Program
    {
        static int Main(string[] args)
        {
            //local main variables
            Match retval;
            string line = "";
            StreamWriter output;
            StreamReader inputfile;
            string detectRet;
            string finalCode = "";

            MyGlobals.CsFilename = args[3];
            MyGlobals.CFilename = args[1];

            //check if enough arguments are provided
            if (args.Length != 4)
            {
                return -1;
            }
            if ((args[0] != "-i") || (args[2] != "-o"))
            {
                Console.WriteLine("invalid parameters provided");
                Console.WriteLine("portwizard -i C_source_file -o C#_output_file");
                return -1;
            }
            //check if the input file is c type file
            retval = Regex.Match(args[1], @"(\w+).c");
            if(retval.Success == false)
            {
                return -1;
            }
            //check if the output file is cs type file
            retval = Regex.Match(args[3], @"(\w+).cs");
            if (retval.Success == false)
            {
                return -1;
            }

            try
            {
                //create stream readeder and writer
                inputfile = new System.IO.StreamReader(args[1]);

                output = new System.IO.StreamWriter(args[3]);
                //get name of file
                string csFilename = Path.GetFileNameWithoutExtension(args[3]);

                //set namespace to be the name of file
                MyGlobals.NamespaceName = csFilename;
                //read line by line 
                while ((line = inputfile.ReadLine()) != null)
                {
                    //check for each line.
                    detectRet = detect.isMatch(line);

                    finalCode += detectRet;
                    //add new line to each line in code
                    finalCode += "\n";
                }
                //write into output file from the input file
                output.WriteLine(finalCode);
                //close both files once done
                inputfile.Close();
                output.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }
    }
}
