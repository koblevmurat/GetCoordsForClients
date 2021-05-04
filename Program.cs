using System;
using System.Collections.Generic;

namespace sample1
{
    struct settings
    {
        public string filePath;
        public bool saveEachRequestSeparately;
        public string googleKey;
        public bool isOk;
    }

    class Program
    {
        private static void printUsage()
        {
            Console.WriteLine("usage: ");
            Console.WriteLine(" -h for help");
	    Console.WriteLine(" <.csv_file_path> <google_key> [saveEachRequestSeparately true/false]");
        }
        private static settings parseArgs(string[] args)
        {
            settings currentSettings = new settings();
            currentSettings.saveEachRequestSeparately = true;
            currentSettings.isOk = true;
            if (args.Length > 0)
            {
                var firstArg = args[0].ToUpper().Trim().Replace("-", "");
                if (System.IO.File.Exists(@firstArg))
                {
                    currentSettings.filePath = firstArg;
                }
                else
                {
                    printUsage();
                    currentSettings.isOk = false;
                }
            }
            else
            {
                printUsage();
                currentSettings.isOk = false;
            }

            if (args.Length > 1) {
                currentSettings.googleKey = args[1]; 
            } else {
                printUsage();
                currentSettings.isOk = false;
            }

            if (args.Length > 2)
            {
                var saveEachRequestSeparately = args[1].ToLower().Trim().Replace("-", "");
                if (saveEachRequestSeparately.Contains("false"))
                    currentSettings.saveEachRequestSeparately = false;
            }   

            return currentSettings;
        }
        static void Main(string[] args)
        {
            settings currentSettings = parseArgs(args);
            if (!currentSettings.isOk)
                return;


            int LineNumber = 0;
            RequestCoords requestCoords = new RequestCoords();
            requestCoords.googleKey = currentSettings.googleKey;
            List<string> results = new List<string>();
            List<addressInfo> myAddressInfo = myTextProvider.ReadMyFile(@currentSettings.filePath);

            System.Console.WriteLine("Total addresses: " + myAddressInfo.Count);

            foreach (addressInfo address in myAddressInfo)
            {
                try
                {
                    LineNumber += 1;
                    var Coords = requestCoords.getCoordsInfo(address.userAddress);
                    results.Add(String.Format("UPDATE public.\"Clients\" Set \"GeoCoordinate\" = ST_GeomFromText('POINT({0} {1})') where \"Id\" = '{2}';", Coords.lng, Coords.lat, address.userId));
                    if (currentSettings.saveEachRequestSeparately)
                    {
                        myTextProvider.SaveResults(results);
                        results.Clear();
                    }
                }
                catch (System.Exception e)
                {
                    Console.WriteLine("error on line: " + LineNumber);
                    Console.Write(e.Message);
                }

            }
            if (!currentSettings.saveEachRequestSeparately) { myTextProvider.SaveResults(results); }
            System.Console.Write("output saved");
        }
    }
}
                                