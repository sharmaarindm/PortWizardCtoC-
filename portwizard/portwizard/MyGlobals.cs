/*
* FILE : MyGlobals.cs
* PROJECT : SQII - Assignment portwizard
* PROGRAMMER : Arindm Sharma & Zivojin Pecin
* DESCRIPTION : This file contains name space, cfile name and csfilename as global variables
* 
*/

namespace portwizard
{
    class MyGlobals
    {
        //variable decleration
        private static string namespaceName;
        private static string csFilename;
        private static string cFilename;

        public static string NamespaceName
        {
            get
            {
                return namespaceName;
            }

            set
            {
                namespaceName = value;
            }
        }

        public static string CsFilename
        {
            get
            {
                return csFilename;
            }

            set
            {
                csFilename = value;
            }
        }

        public static string CFilename
        {
            get
            {
                return cFilename;
            }

            set
            {
                cFilename = value;
            }
        }
    }
}
