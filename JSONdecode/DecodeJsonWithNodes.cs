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

        public string[] headTags = {"title", "style", "script"};
        public string[] headTagsSingleton = {"meta", "link", "base"};
        public string[] bodyTags = {"a", "abbr", "address", "article", "aside", "audio", "b", "bdo", "blockquote", "button", 
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

        public void Main(){
            
            string jsonString = File.ReadAllText(JsonFile);

            JsonNode? fullJsonFile = JsonNode.Parse(jsonString);

            pageNotFoundJson.Doctype = fullJsonFile["doctype"];
            Console.WriteLine($"Doctype Section = {pageNotFoundJson.Doctype}");

            pageNotFoundJson.Head = fullJsonFile["head"];
            Console.WriteLine($"Head Section = {pageNotFoundJson.Head}");

            pageNotFoundJson.Body = fullJsonFile["body"];
            Console.WriteLine($"Body Section = {pageNotFoundJson.Body}");

            Console.WriteLine("Head Section Inners");

            foreach (var item in pageNotFoundJson.Head.AsObject())
            {
                Console.WriteLine($"item in Head = {item}");
                Console.WriteLine($"item in Head Key = {item.Key} Value = {item.Value}");

                if(item.Value.GetType() == typeof(JsonArray)){
                    Console.WriteLine("This is an Array");
                }
                if(item.Value.GetType() == typeof(JsonObject)){
                    Console.WriteLine("This is an Object");
                }
                if(item.Value.GetType() == typeof(JsonValue)){
                    Console.WriteLine("This is a Value");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Body Section Inners");
            foreach (var item in pageNotFoundJson.Body.AsObject())
            {
                Console.WriteLine($"item in Body = {item}");
                Console.WriteLine($"item in Body Key = {item.Key} Value = {item.Value}");

                if(item.Value.GetType() == typeof(JsonArray)){
                    Console.WriteLine("This is an Array");
                }
                if(item.Value.GetType() == typeof(JsonObject)){
                    Console.WriteLine("This is an Object");
                }
                if(item.Value.GetType() == typeof(JsonValue)){
                    Console.WriteLine("This is a Value");
                }
                Console.WriteLine();
            }

        }

    }

    public static class pageNotFoundJson{

        public static JsonNode? Doctype {get; set;}
        public static JsonNode? Head {get; set;}
        public static JsonNode? Body {get; set;}
    }

    public class pageNotFoundJsonHeadTag{


    }

}