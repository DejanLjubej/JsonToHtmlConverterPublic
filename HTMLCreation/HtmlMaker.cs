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
       }

        // a method for creating each seperate tag passed in as an argument
        public void createSeperateTag(KeyValuePair<string, object> tag){
            
            Console.WriteLine($"<{tag.Key}>");
                Console.WriteLine($"{tag.Value}");
            Console.WriteLine($"</{tag.Key}>");

        }
    }
}