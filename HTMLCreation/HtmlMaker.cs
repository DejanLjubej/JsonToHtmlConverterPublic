using System;
using System.IO;
using System.Text;
using AssignmentCSJSON.JSONdecode;

namespace HTMLCreation.HtmlMaker{

    class HtmlMakerProgram{
        

       public static void CreateHtml(){
           

           string fileToRead = "D:\\Programming\\JobAssignments\\FlawlessCode\\helloWorld.json";
            DecodeJsonWithNodes decodeDaJeySan =  new DecodeJsonWithNodes(fileToRead); 
            decodeDaJeySan.Main();
            // BaseClassWrapper bscw = DeserializeHtmlJson.Main();
            // StringBuilder htmlString = new StringBuilder();

            //     //Console.Write("bscw "+bscw.Head);
            // htmlString.Append("<!Doctype");
            // // foreach (var item in bscw?.Doctype)
            // // {
            // //     Console.Write(item);
            // //     htmlString.Append(item);
            // // }
            // htmlString.AppendLine(">");

            // htmlString.AppendLine($"<head>");
            // // foreach (var item in bscw?.Head){
            // //     htmlString.AppendLine($"<{item.Key}> {item.Value}");
            // //     htmlString.AppendLine($"</{item.Key}>");
            // // }
            // htmlString.AppendLine($"</head>");

            // Console.Write(htmlString);
       }

        // a method for creating each seperate tag passed in as an argument
        public void createSeperateTag(KeyValuePair<string, object> tag){
            
            Console.WriteLine($"<{tag.Key}>");
                Console.WriteLine($"{tag.Value}");
            Console.WriteLine($"</{tag.Key}>");

        }
    }
}