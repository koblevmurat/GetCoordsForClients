using System;
using System.IO;
using System.Collections.Generic;

public struct addressInfo
{
    public string userId;
    public string userAddress;
}
class myTextProvider
{
    public static List<addressInfo> ReadMyFile(string filePath)
    {
        List<addressInfo> lst = new List<addressInfo>();
        if (File.Exists(filePath))
        {
            using (StreamReader sr = new StreamReader(@filePath))
            {
                while (sr.Peek() >= 0)
                {
                    var anotherLine = sr.ReadLine();
                    addressInfo newAddressInfo = new addressInfo();
                    newAddressInfo.userId = anotherLine.Substring(0, anotherLine.IndexOf(",")).Replace('"', ' ').Trim();
                    newAddressInfo.userAddress = anotherLine.Substring(anotherLine.IndexOf(",") + 1).Replace('"', ' ').Trim();
                    lst.Add(newAddressInfo);
                }
            }
        }
        return lst;
    }

    public static void SaveResults(List<string> sql_updates)
    {
        using (StreamWriter sw = new StreamWriter("Output.txt", true))
        {
            foreach (string req in sql_updates)
            {
                sw.WriteLine(req);
            }
        }
    }
}
