using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AssignmentCSJSON.JSONdecode
{
    public class DecodeJsonWithNodes
    {
        public string JsonFile {get;set;}

        public string[] headTags = {"text", "head", "title", "style", "script"};
        public string[] headTagsSingleton = {"doctype", "meta", "link", "base"};
        public string[] bodyTags = { "text", "body", "a", "abbr", "address", "article", "aside", "audio", "b", "bdo", "blockquote", "button", 
                                    "canvas", "caption", "cite", "code", "colgroup", 
                                    "datalist", "dd", "del", "details", "dfn", "div", "dl", "dt", "em", 
                                    "fieldset", "figcaption", "figure", "footer", "form", "frame", "frameset",
                                    "h1", "h2", "h3", "h4", "h5", "h6", "header", "i", "iframe", "ins", "kbd", 
                                    "label", "legend", "li", "map", "mark", "menu", "meter", "nav", "noscript", 
                                    "object", "ol", "optgroup", "option", "output", "p", "param", "pre", "progress", "q", "rp", "rt", "ruby", 
                                    "s", "samp", "script", "section", "select", "small", "span", "strong", "style", "sub", "sup",
                                    "table", "tbody", "td", "textarea", "tfoot", "th", "thead", "time", "tr", "track", 
                                    "u", "ul", "var", "video"};
        public string[] bodyTagsSingleton = {"area", "br", "col", "command", "embed", "hr", "img", "input", 
                                            "keygen", "link", "meta", "param", "source", "track", "wbr"};
        private string[] singletonTags = {"area", "br", "col", "command", "embed", "hr", "img", "input", 
                                            "keygen", "link", "meta", "param", "source", "track", "wbr",
                                            "meta", "link", "base"};
        public DecodeJsonWithNodes(string jsonFile){
            JsonFile = jsonFile;
        }
        JsonNode? fullJsonFile;
        pageNotFoundJson htmlMainTags = new pageNotFoundJson();
        public void Main(){
            
            string jsonString = File.ReadAllText(JsonFile);

            fullJsonFile = JsonNode.Parse(jsonString);
            htmlMainTags.Doctype = fullJsonFile["doctype"];
            htmlMainTags.Language = fullJsonFile["language"];
           
            Console.WriteLine($"full file Section = {fullJsonFile}");
            HtmlBaseWrite();
        }

        private void HtmlBaseWrite(){
            Console.WriteLine($"<!doctype {htmlMainTags.Doctype}>");
            Console.WriteLine($"<html lang = {htmlMainTags.Language}>");
            DoHtmlThing(fullJsonFile);
            Console.Write("</html>");
        }

        public void DoHtmlThing(JsonNode aThing, int amountOfTabs=0){

            foreach (var item in aThing.AsObject())
            {
                int indenting = amountOfTabs;
                for (int i = 0; i < indenting; i++)
                {
                    Console.Write("  ");
                }
                if( ( singletonTags.Contains(item.Key.ToString())) && 
                    (item.Key.ToString() != "doctype")){

                    HandleSingletonTags(item);
                   
                }else if(
                    (item.Key.ToString() != "attributes") && 
                    (item.Key.ToString() != "doctype") && 
                    (item.Key.ToString() != "language")){

                    HandleRegularTags(item);
                 
                }
            }
        }

        private void WriteOpeningTag(string itemKey){
            if(itemKey != "text")
                Console.Write($"<{itemKey}");   
        }

        private void CloseOpeningTag(string itemKey){
            if(itemKey != "text")
                Console.WriteLine(">");
        }
        private void WriteClosingTag(string itemKey){
            Console.WriteLine();
            if(itemKey != "text")
                Console.WriteLine($"</{itemKey}>");   
        }

        private void HandleRegularTags(KeyValuePair<string, JsonNode?> item){
            if(item.Value.GetType() == typeof(JsonObject)){

                WriteOpeningTag(item.Key);
                    
                foreach (var innerItem in item.Value.AsObject())
                {
                    if(innerItem.Key.ToString() == "attributes"){
                        DoHtmlAttributes(innerItem.Value);
                    }
                }

                CloseOpeningTag(item.Key);

                DoHtmlThing(item.Value);  

                WriteClosingTag(item.Key);   

            }else if(item.Value.GetType() == typeof(JsonArray)){

                foreach (var arrayItem in item.Value.AsArray())
                {
                    WriteOpeningTag(item.Key);  

                    if(arrayItem.GetType()==typeof(JsonObject)){

                        foreach (var arrayObjectItem in arrayItem.AsObject())
                        {
                            if(arrayObjectItem.Key.ToString() == "attributes"){
                            
                                DoHtmlAttributes(arrayObjectItem.Value);
                            }      
                        }

                        CloseOpeningTag(item.Key);

                        DoHtmlThing(arrayItem.AsObject()); 

                        WriteClosingTag(item.Key);   

                    }else{
                        CloseOpeningTag(item.Key);

                        Console.Write($"{arrayItem}");   

                        WriteClosingTag(item.Key);
                    }
                }
                
            }else{
                WriteOpeningTag(item.Key);

                CloseOpeningTag(item.Key);

                Console.Write($"{item.Value}");

                WriteClosingTag(item.Key) ; 
            }  
        }
        private void HandleSingletonTags(KeyValuePair<string, JsonNode?> item)
        {
            if(item.Value.GetType() == typeof(JsonArray)){

                foreach (var arrayItem in item.Value.AsArray())
                {
                    if(arrayItem.GetType()== typeof(JsonObject)){

                        WriteOpeningTag(item.Key); 

                        DoHtmlAttributes(arrayItem);
                        CloseOpeningTag(item.Key);
                    }else{

                    if(item.Key.ToString() != "text")
                        
                        Console.Write($"<{item.Key}");   

                        Console.Write($"{arrayItem}");   

                        CloseOpeningTag(item.Key);
                    }
                }
            }else{

                if(item.Value.GetType() == typeof(JsonObject)){
                    
                    WriteOpeningTag(item.Key);

                    DoHtmlAttributes(item.Value);

                    CloseOpeningTag(item.Key);
                }else 
                {
                    Console.WriteLine($"<{item.Key} {item.Value}>");
                }
            }
        }

        public void DoHtmlAttributes(JsonNode attributeNode){
            foreach (var attributeItem in attributeNode.AsObject())
            {
                if(attributeItem.Value.GetType() == typeof(JsonObject)){

                    int i=0;
                    Console.Write($" {attributeItem.Key} = \"");

                    foreach (var innerAttributeItem in attributeItem.Value.AsObject())
                    {
                        i++;
                        Console.Write($"{innerAttributeItem.Key}:{innerAttributeItem.Value}");
                        if(i != attributeItem.Value.AsObject().Count()){
                            Console.Write("; ");
                        }else{
                            Console.Write("\"");
                        }
                    }

                }else{
                    Console.Write($" {attributeItem.Key} = \"{attributeItem.Value}\"");
                }
            }
        }
    }

    public class pageNotFoundJson{

        public JsonNode? Doctype {get; set;}
        public JsonNode? Language {get;set;}
    }
}