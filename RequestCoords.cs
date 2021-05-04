using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
class RequestCoords
{
    private string gkey = "AIzaSyC_"; 

    public string googleKey {
        set {this.gkey = value;}
    }
    
    private String getReq(string req)
    {
        StringBuilder sb_respo = new StringBuilder();
        WebRequest request = WebRequest.Create(req);
        WebResponse response = request.GetResponse();
        Stream ReceiveStream = response.GetResponseStream();
        Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

        StreamReader readStream = new StreamReader(ReceiveStream, encode); 
        Char[] read = new Char[256];

        // Read 256 charcters at a time.    
        int count = readStream.Read(read, 0, 256); 
        while (count > 0)
        {
            // Dump the 256 characters on a string and display the string onto the console.
            String str = new String(read, 0, count);
            sb_respo.Append(str);
            count = readStream.Read(read, 0, 256);
        } 
        // Release the resources of stream object.
        readStream.Close();

        // Release the resources of response object.
        response.Close();

        return sb_respo.ToString();
    }
    public CoordsInfo getCoordsInfo(string address)
    {
        var requestStr = String.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}", address, this.gkey);
        CoordsInfo coordsInfo = new CoordsInfo();
        var addressInfo = JsonDocument.Parse(getReq(requestStr));
        try
        {
            var location = addressInfo.RootElement.GetProperty("results")[0].GetProperty("geometry").GetProperty("location");
            coordsInfo.lat = location.GetProperty("lat").ToString();
            coordsInfo.lng = location.GetProperty("lng").ToString(); 
        }
        catch (Exception e)
        {
            throw(e);
        } 
        return coordsInfo; 
    }
}