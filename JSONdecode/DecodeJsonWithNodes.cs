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
        public string[] headTagsSingleton = {"meta", "link", "base"};
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
        public DecodeJsonWithNodes(string jsonFile){
            JsonFile = jsonFile;
        }

        public List<Dictionary<string, object>> listOfTags =  new List<Dictionary<string, object>>();
        public List<JsonNode> listOfJNTags = new List<JsonNode>();
        public void Main(){
            
            string jsonString = File.ReadAllText(JsonFile);

            JsonNode? fullJsonFile = JsonNode.Parse(jsonString);

            pageNotFoundJson.Doctype = fullJsonFile["doctype"];
            // Console.WriteLine($"Doctype Section = {pageNotFoundJson.Doctype}");

            pageNotFoundJson.Head = fullJsonFile["head"];
            // Console.WriteLine($"Head Section = {pageNotFoundJson.Head}");
            
            pageNotFoundJson.Body = fullJsonFile["body"];
            // Console.WriteLine($"Body Section = {pageNotFoundJson.Body}");
            Console.WriteLine($"full file Section = {fullJsonFile}");
            DoHtmlThing(fullJsonFile);

            Console.WriteLine("Head Section Inners");

            // foreach (var item in pageNotFoundJson.Head.AsObject())
            // {
            //     Console.WriteLine($"item in Head = {item.GetType()}");
            //     Console.WriteLine($"item in Head Key = {item.Key} Value = {item.Value}");

            //     if(item.Value.GetType() == typeof(JsonArray)){
            //         Console.WriteLine("This is an Array");
            //     }
            //     if(item.Value.GetType() == typeof(JsonObject)){
            //         Console.WriteLine("This is an Object");
            //     }
            //     if(item.Value.GetType() == typeof(JsonValue)){
            //         Console.WriteLine("This is a Value");
            //     }
            //     Console.WriteLine();
            // }

            // Console.WriteLine("Body Section Inners");
            // foreach (var item in pageNotFoundJson.Body.AsObject())
            // {
            //     Console.WriteLine($"item in Body = {item}");
            //     Console.WriteLine($"item in Body Key = {item.Key} Value = {item.Value}");

            //     if(item.Value.GetType() == typeof(JsonArray)){
            //         Console.WriteLine("This is an Array");
            //     }
            //     if(item.Value.GetType() == typeof(JsonObject)){
            //         Console.WriteLine("This is an Object");
            //     }
            //     if(item.Value.GetType() == typeof(JsonValue)){
            //         Console.WriteLine("This is a Value");
            //     }
            //     Console.WriteLine();
            // }

        }

        public void GoThroughNodes(JsonNode jsonNode){
            foreach(var jn in jsonNode.AsObject()){
                string nodeName = jn.Key.ToString();
                IterativeDictionary tag = new IterativeDictionary();
                tag.tagTest = jn.ToString();
                listOfJNTags.Add(tag.tagTest);
            }
        }

        public KeyValuePair<string, object>? returnJsonTagAsDictionary(JsonNode jsonNode){


            
            return null;
        }

        public void DoHtmlThing(JsonNode aThing, int amountOfTabs=0){

            for (var i = 0; i < amountOfTabs; i++)
            {
                Console.Write("   ");
            }

            foreach (var item in aThing.AsObject())
            {
                if( ( bodyTags.Contains(item.Key.ToString()) ) || ( headTags.Contains(item.Key.ToString()) ) ){

                    
                    if(item.Value.GetType() == typeof(JsonObject)){
                        Console.Write($"<{item.Key}");   
                            
                        foreach (var innerItem in item.Value.AsObject())
                        {
                            if(innerItem.Key.ToString() == "attributes"){
                                DoHtmlAttributes(innerItem.Value);
                            }
                        }

                        Console.WriteLine(">");
                        DoHtmlThing(item.Value, amountOfTabs++);                            
                        Console.WriteLine($"</{item.Key}>");   

                    }else if(item.Value.GetType() == typeof(JsonArray)){
                        foreach (var arrayItem in item.Value.AsArray())
                        {
                            Console.Write($"<{item.Key}");  
                            if(arrayItem.GetType()==typeof(JsonObject)){

                                foreach (var arrayObjectItem in arrayItem.AsObject())
                                {
                                        if(arrayObjectItem.Key.ToString() == "attributes"){
                                       
                                            DoHtmlAttributes(arrayObjectItem.Value);
                                        }

                                    // foreach (var arrayObjectInnerItem in arrayObjectItem.Value.AsObject())
                                    // {
                                    // }

                                            
                                }
                                Console.WriteLine(">");
                                DoHtmlThing(arrayItem.AsObject(), amountOfTabs++);                            
                                Console.WriteLine($"</{item.Key}>");   
                            }else{
                               //Console.Write($"<{item.Key}");   
                                Console.Write(">");
                                Console.Write($"{arrayItem}");   
                                Console.WriteLine($"</{item.Key}>");   
                            }
                        }
                        
                    }else{
                        Console.Write($"<{item.Key}");   
                        Console.Write(">");
                        Console.Write($"{item.Value}");
                        Console.WriteLine($"</{item.Key}>");   
                    }  

                }else if( ( bodyTagsSingleton.Contains(item.Key.ToString()) ) || ( headTagsSingleton.Contains(item.Key.ToString()) ) ){
                    
                    if(item.Value.GetType() == typeof(JsonArray)){
                        foreach (var arrayItem in item.Value.AsArray())
                        {
                            if(arrayItem.GetType()== typeof(JsonObject)){
                                Console.Write($"<{item.Key}");   
                                DoHtmlAttributes(arrayItem);
                                Console.WriteLine($">");
                            }else{
                            Console.Write($"<{item.Key}");   
                            Console.Write($"{arrayItem}");   
                            Console.WriteLine($">");

                            }
                        }
                    }else{
                        Console.Write($"<{item.Key}");
                        DoHtmlAttributes(item.Value);
                        Console.WriteLine($">");
                    }
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

        public void HandleTags(){
            
        }

        public void HandleSingletonTags(){
            

        }
    }



    public static class pageNotFoundJson{

        public static JsonNode? Doctype {get; set;}
        public static JsonNode? Head {get; set;}
        public static JsonNode? Body {get; set;}
        public static Dictionary<string, IterativeDictionary>? Bodys {get; set;}
    }

    public class IterativeDictionary{
        public Dictionary<string, object>? Test {get; set;}
        public JsonNode? tagTest {get; set;}
    }

    public class pageNotFoundJsonHeadTag{

        public KeyValuePair<string, object>? tag {get; set;}

    }

}