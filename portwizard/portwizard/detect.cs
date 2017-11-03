/*
* FILE : detect.cs
* PROJECT : SQII - Assignment portwizard
* PROGRAMMER : Arindm Sharma & Zivojin Pecin
* DESCRIPTION : This file contains all the methods that are requiered in order to detect the libraries and the main for the
* input file. Once the main is detected, the program proceeds further to detect each line and switch its library function from c to c#
*/
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace portwizard
{
    class detect
    {
        //Class variables

        static bool flag = false;
        static bool flag2 = false;
        static bool flag3 = false;
        private const int maxHeaders = 15;
        private const int maxVariables = 25;

        static List<string> functions = new List<string>();


        /*
        * FUNCTION : isMatch
        *
        * DESCRIPTION : This function checks what type of functions/values does the provided line contain.
        * 
        * PARAMETERS : string line
        * RETURNS : string retval
        */
        public static string isMatch(string line)
        {
            //check if the line is comment
            string retVal = "";
            if (line == "/*")
            {
                flag = true;
            }
            if (line == " */")
            {
                flag = false;
                return line;
            }
            //check if line contains main
            if (flag == false)
            {
                flag2 = false;
                if ((line.Contains("int main (void)")) || (line.Contains("int main(void)")))
                {
                    line = "namespace " + MyGlobals.NamespaceName + "\n{\n\tclass Program\n\t{\n\t\tstatic int Main(string[] args)";
                    flag2 = true;
                    
                }
                //check if line contains fprintf
                if (line.Contains("fprintf"))
                {
                    line = fprintfHandle(line);
                    flag2 = true;
                }
                //check if line contains printf
                else if (line.Contains("printf"))
                {
                    line = printfHandle(line);
                    flag2 = true;
                }
                //check if line contains NULL
                if (line.Contains("NULL"))
                {
                    line = line.Replace("NULL", "null");
                }
                //check if line contains include
                if (line.Contains("#include"))
                {
                    line = includeHandle(line);
                    flag2 = true;
                }
                //check if line contains return 0
                if (line.Contains("return 0;"))
                {
                    line = "\t\t" + line;
                    line += "\n\t\t}\n\t}";
                    flag2 = true;
                    flag3 = true;
                }
                //check if line contains return 1
                if (line.Contains("return 1;"))// in case of error returns
                {
                    line = "\t\t" + line;
                    flag2 = true;
                }
                //check if line contains if statemenet
                if ((line.Contains("if (")) || (line.Contains("if(")))
                {
                    line = "\t\t" + line;
                    flag2 = true;
                }
                //check if line contains FILE
                if (line.Contains("FILE"))
                {
                    flag2 = true;
                    string temp = "";
                    return temp;
                }
                //check if line contains fopen
                if (line.Contains("fopen"))
                {
                    line = fopenHandle(line);
                    flag2 = true;
                }
                //check if line contains fgets
                if (line.Contains("fgets"))
                {
                    line = fGetsHandle(line);
                    flag2 = true;
                }
                //check if line contains fclose
                if (line.Contains("fclose"))
                {
                    line = closeHandle(line);
                    flag2 = true;
                }
                //check if line contains char
                if (line.Contains("char"))
                {
                    line = charHandle(line);
                    flag2 = true;
                }
                //check if line contains atoi
                if (line.Contains("atoi"))
                {
                    line = atoiHandle(line);
                    flag2 = true;
                }
                //check if line contains gets
                if (line.Contains("gets"))
                {
                    line = GetsHandle(line);
                    flag2 = true;
                }
                if (flag2 == false)
                {
                    if (flag3 == false)
                    {
                        line = "\t\t" + line;
                    }
                    else
                    {
                       
                        flag3 = false;
                    }
                }
            }
            else
            {   //check if line contains the c file iname, if it does replace it with c# filename
                if (line.Contains(MyGlobals.CFilename))
                {
                    line = line.Replace(MyGlobals.CFilename, MyGlobals.CsFilename);
                }
                retVal = line;
            }
            retVal = line;
            return retVal;
        }


        /*
       * FUNCTION : includeHandle
       *
       * DESCRIPTION : This file checks the include files that are in the provided file
       *
       * PARAMETERS : string toSwitch
       * RETURNS : string retval
       */
        public static string includeHandle(string toSwitch)
        {
            string retval = "";

            //checking for stdio
            Match match = Regex.Match(toSwitch, @"(^|[^\w])<stdio.h>([^\w]|$)");
            if (match.Success)
            {
                retval = "using System;\nusing System.Collections.Generic;\nusing System.Text;";
                return retval;

            }

            //check for stdlib
            match = Regex.Match(toSwitch, @"(^|[^\w])<stdlib.h>([^\w]|$)");
            if (match.Success)
            {
                functions.Add("atoi");
            }

            // checking for string.h
            match = Regex.Match(toSwitch, @"(^|[^\w])<string.h>([^\w]|$)");
            if (match.Success)
            {
                functions.Add("strlen");
            }
            return retval;
        }
        /*
       * FUNCTION : printfHandle
       *
       * DESCRIPTION : This file checks the printf's that are in the provided string
       *
       * PARAMETERS : string toSwitch
       * RETURNS : string retval
       */
        public static string printfHandle(string toSwitch)
        {
            string retval = "";
            //if it has type specifiers or not.
            if (toSwitch.Contains("%") == true)
            {
                string subString = "";
                //replace printf with System.Console.WriteLine
                retval = toSwitch.Replace("printf", "System.Console.WriteLine");
                //delimit so that we have 
                string specifier = toSwitch.Split('"', '"')[1];

                string original = specifier;
                // how many are there...
                int count = new Regex(Regex.Escape("%")).Matches(specifier).Count;
            
                for (int i = 0; i < count; i++)
                {
                    int Percent = specifier.IndexOf("%");
                    int dvalue = specifier.IndexOf("d");
                    // if %d is the current specifier.
                    if (dvalue < 0)
                    {
                        dvalue = specifier.IndexOf("s");
                 
                        subString = "{" + i + "," + specifier[Percent + 0] + "}";
                        // change type specifier to c# specifier.
                        subString = subString.Replace("%", "0");
                    }
                    //if %f is the current specifier.
                    else
                    {
                        subString = "{" + i + "," + specifier[Percent + 1] + "}";
                    }
                    specifier = specifier.Insert(dvalue + 1, subString);
                    specifier = specifier.Remove(Percent, (dvalue + 1) - Percent);
                    dvalue = 0;
                }
                //replace original with modified
                retval = retval.Replace(original, specifier);
            }
            else
            {
                retval = toSwitch.Replace("printf", "System.Console.WriteLine");
            }

            if (retval.Contains("strlen"))
            {
                retval = strlenHandle(retval);
            }
            if (retval.Contains("atoi"))
            {
                retval = retval.Replace("atoi", "Int32.Parse");
            }
            
            return "\t\t" + retval;
        }

        /*
       * FUNCTION : strlenHandle
       *
       * DESCRIPTION : This file checks the strlen method calls that are in the provided string
       *
       * PARAMETERS : string toswitch
       * RETURNS : string retval
       */
        public static string strlenHandle(string toSwitch)
        {
            string retval = "";
            //count how many times strlen exists in the file.
            int count = Regex.Matches(toSwitch, "strlen.+").Count;
            // if the count is greater than 0
            if (count > 0)
            {
                //replace the strlen's
                toSwitch = toSwitch.Replace("strlen", "^");
                //infinite while loop
                while (true)
                {
                    //how many places we put the temp symbol.
                    count = toSwitch.IndexOf('^');

                    for (int i = count; i <= toSwitch.Length; i++)
                    {
                        if (toSwitch[i] == ')')
                        {
                            toSwitch = toSwitch.Insert(i + 1, ".Length");
                            toSwitch = toSwitch.Remove(count, 1);
                            break;
                        }
                    }
                    if (!toSwitch.Contains("^"))
                    {
                        break;
                    }
                }
            }
            retval = toSwitch;
            return retval;
        }
        /*
       * FUNCTION : fopenHandle
       *
       * DESCRIPTION : This file checks the fopen method calls that are in the provided string
       *
       * PARAMETERS : string currentString
       * RETURNS : string retval
       */
        public static string fopenHandle(string currentString)
        {
            string retVal = "";
            string read = "\"r\"";
            string fileName = "";
            int start = currentString.IndexOf("(");
            int end = currentString.IndexOf(",");
            fileName = currentString.Substring(start + 1, end - start - 1);
            // if the case is of file reading
            if (currentString.Contains(read))
            {
                retVal = "\t\t\t" + "System.IO.StreamReader fp = new System.IO.StreamReader (" + fileName + ");";
            }
            //if the case if of file writing
            else
            {
                retVal = "\t\t\t" + "System.IO.StreamWriter fp = new System.IO.StreamWriter (" + fileName + ");";
            }

            return retVal;
        }
        /*
      * FUNCTION : fGetsHandle
      *
      * DESCRIPTION : This file checks the fGets method calls that are in the provided string
      *
      * PARAMETERS : string currentString
      * RETURNS : string retval - modified string
      */
        public static string fGetsHandle(string currentString)
        {
            string retVal = "";
            //while (fgets(inBuff, 100, fp) != NULL) from
            //while ((inBuff = fp.ReadLine()) != null) to
            int start = currentString.IndexOf("(");
            int end = currentString.IndexOf(")");
            string substring = currentString.Substring(start, end - start + 1);
            int destStart = currentString.LastIndexOf("(");
            int destEnd = currentString.IndexOf(",");
            string destString = currentString.Substring(destStart + 1, destEnd - destStart - 1);
            int ptrstart = currentString.LastIndexOf(",");
            int ptrend = currentString.IndexOf(")");
            string ptr = currentString.Substring(ptrstart + 1, ptrend - ptrstart - 1);

            string custom = "";
            custom = "((" + destString + " = " + ptr + ".ReadLine())";
            retVal = "\t\t" + currentString.Replace(substring, custom);


            return retVal;
        }
        /*
     * FUNCTION : closeHandle
     *
     * DESCRIPTION : This file checks the close method calls that are in the provided string
     *
     * PARAMETERS : string currentString
     * RETURNS : string retval - modified string
     */
        public static string closeHandle(string currentString)
        {
            // fclose(fp);from
            //fp.Close();to
            string retval = "";
            int start = currentString.IndexOf("(");
            int end = currentString.IndexOf(")");
            string substring = currentString.Substring(start + 1, end - start - 1);
            retval = substring + ".Close();";
            return "\t\t\t" + retval;
        }
        /*
     * FUNCTION : charHandle
     *
     * DESCRIPTION : This file checks the char variable intializations that are in the provided string
     *
     * PARAMETERS : string currentString
     * RETURNS : string retval - modified string
     */
        public static string charHandle(string currentString)
        {
            //		char	inBuff[100];from
            // string inBuff;
            string retval = "";

            string temp = currentString.Replace("char", "");
            int start = currentString.IndexOf("[");
            int end = currentString.LastIndexOf("]");
            string substring = currentString.Substring(start, end - start+1);
            temp = temp.Replace(substring, "");
            temp = temp.Replace("\t", "");
             
            retval = "\t\t\tstring " + temp;
            return retval;
        }
        /*
     * FUNCTION : fprintfHandle
     *
     * DESCRIPTION : This file checks the fprintf method calls that are in the provided string
     *
     * PARAMETERS : string currentString
     * RETURNS : string retval - modified string
     */
        public static string fprintfHandle(string currentString)
        {
            // from fprintf (fp, "hello world\n");

            // to fp.WriteLine("hello world");
            string retval = "";
            //getting the filepointer out of the string
            int fileStart = currentString.IndexOf("(");
            int fileEnd = currentString.IndexOf(",");
            //getting the text out of the string.
            int start = currentString.IndexOf("\"");
            int end = currentString.LastIndexOf("\"");
            string filename = currentString.Substring(fileStart + 1, fileEnd - fileStart - 1);
            string substring = currentString.Substring(start + 1, end - start - 1);

            substring = filename + ".WriteLine(\"" + substring + "\");";
            retval = "\t\t\t" + substring;

            return retval;
        }
        /*
    * FUNCTION : atoiHandle
    *
    * DESCRIPTION : This file checks the atoi method calls that are in the provided string
    *
    * PARAMETERS : string currentString
    * RETURNS : string retval - modified string
    */
        public static string atoiHandle(string currentString)
        {
            //table = atoi (buffer); from
            //table = Int32.Parse (buffer); to 
            string retval = "";
            string replacedString = currentString.Replace("atoi", "Int32.Parse");
            retval = "\t\t" + replacedString;
            return retval;
        }
        /*
    * FUNCTION : GetsHandle
    *
    * DESCRIPTION : This file checks the gets method calls that are in the provided string
    *
    * PARAMETERS : string currentString
    * RETURNS : string retval - modified string
    */
        public static string GetsHandle(string currentString)
        {
            //gets (buffer); from
            //buffer = System.Console.ReadLine();to
            string retval = "";
            int fstart = currentString.IndexOf("(");
            int fend = currentString.LastIndexOf(")");
            string substring = currentString.Substring(fstart + 1, fend - fstart - 1);
            retval = "\t\t\t"+substring + " = System.Console.ReadLine();";
            return retval;
        }
    }
}
