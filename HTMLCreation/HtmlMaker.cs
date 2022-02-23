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
            int amountOfFilesConverted = 0;
            
            if(jsonFiles.Length>0)
            {
                DecodeJsonWithNodes decodeJsonFiles;
                string contentToWrite;

                foreach (var item in jsonFiles)
                {
                    if(item.Contains(".json"))
                    {
                        try{

                        decodeJsonFiles =  new DecodeJsonWithNodes(item); 
                        decodeJsonFiles.Main();    
                        contentToWrite  = decodeJsonFiles.HtmlStringBuilder.ToString();
                        string readFileName = item.Replace(jsonDir, "").Replace(".json", ".html");
                        File.WriteAllText(htmlDir+readFileName, contentToWrite);
                        amountOfFilesConverted ++;
                        Console.WriteLine($"The file {item} was converted");
                        
                        }catch(System.Exception ex){
                            Console.WriteLine($"The file that was used did not follow the rules \"{item}\" in html Writer");
                            Console.WriteLine($"Exception was thrown {ex}");
                            Console.WriteLine("Press any key to continue");
                            Console.Read();
                            continue;
                        }
                    }
                }

                if(amountOfFilesConverted>0){
                    Console.WriteLine($"{amountOfFilesConverted} Files Have Been Converted");
                    Console.WriteLine("Saved in the htmlFiles directory");
                    Console.WriteLine("Press any key to continue");
                    Console.Read();
                }
                else
                {
                    Console.WriteLine("No json Files found");
                    Console.WriteLine("Put any json file you want converted in to the JsonFiles directory");
                    Console.WriteLine("Press any key to continue");
                    Console.Read();
                }
            }
            else
            {
                Console.WriteLine("There are no files in this directory");
                Console.WriteLine("Press any key to continue");
                Console.Read();
            }
 
       }
    }
}