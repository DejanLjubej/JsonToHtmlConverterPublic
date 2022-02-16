using System;
using System.IO;
using System.Text;
using AssignmentCSJSON.JSONdecode;


namespace HTMLCreation.HtmlMaker{

    class HtmlMakerProgram{

        string? _jsonFileToDecode;
        DecodeJsonWithNodes _JsonToHtmlConverter;

       public static void CreateHtml(){
           try
           {
                string fileToRead = "D:\\Programming\\JobAssignments\\FlawlessCode\\helloWorld.json";
                DecodeJsonWithNodes decodeDaJeySan =  new DecodeJsonWithNodes(fileToRead); 
                decodeDaJeySan.Main();
                string contentToWrite  = decodeDaJeySan.HtmlStringBuilder.ToString();
                File.WriteAllText("HtmlFile.html", contentToWrite);
           }
           catch (System.Exception ex)
           {
               Console.Write($"Exception on html maker {ex}");
                // TODO
           }
       }
    }
}
//Unhandled exception. System.Text.Json.JsonReaderException: '<' is an invalid start of a value
//System.Text.Json.JsonReaderException: '0x0D' is invalid within a JSON string. The string should be correctly escaped