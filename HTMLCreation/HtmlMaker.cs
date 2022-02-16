using System;
using System.IO;
using System.Text;
using AssignmentCSJSON.JSONdecode;


namespace HTMLCreation.HtmlMaker{

    class HtmlMakerProgram{

        static string? jsonDir = "jsonFiles";
        static string? htmlDir = "htmlFiles";

        static string[]? jsonFiles;
       
       public static void CreateHtml(){

            jsonFiles = Directory.GetFiles(jsonDir); 
          
            DecodeJsonWithNodes decodeJsonFiles;
            string contentToWrite;

            foreach (var item in jsonFiles)
            {
                if(item.Contains(".json"))
                {
                    decodeJsonFiles =  new DecodeJsonWithNodes(item); 
                    decodeJsonFiles.Main();    
                    contentToWrite  = decodeJsonFiles.HtmlStringBuilder.ToString();
                    string readFilename = item.Replace(jsonDir, "").Replace(".json", ".html");
                    File.WriteAllText(htmlDir+readFilename, contentToWrite);
                }
            }
 
       }
    }
}
//Unhandled exception. System.Text.Json.JsonReaderException: '<' is an invalid start of a value
//System.Text.Json.JsonReaderException: '0x0D' is invalid within a JSON string. The string should be correctly escaped