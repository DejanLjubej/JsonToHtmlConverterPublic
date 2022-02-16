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
        private string JsonFile {get;set;}

        public DecodeJsonWithNodes(string jsonFile){
            JsonFile = jsonFile;
        }

        private string[] singletonTags = {"area", "br", "col", "command", "embed", "hr", "img", "input", 
                                            "keygen", "link", "meta", "param", "source", "track", "wbr",
                                            "meta", "link", "base"};
        JsonNode? fullJsonNode;

        string jsonString;

        HtmlBase htmlMainTags;

        public StringBuilder HtmlStringBuilder {get; private set;}

        public void Main(){

            HtmlStringBuilder = new StringBuilder();
            htmlMainTags = new HtmlBase();

            jsonString = File.ReadAllText(JsonFile);   
            
            fullJsonNode = JsonNode.Parse(jsonString);

            htmlMainTags.Doctype = fullJsonNode["doctype"];
            htmlMainTags.Language = fullJsonNode["language"];
           
            HtmlBaseWrite();
            Console.Write($"Stringbuilder write \n{HtmlStringBuilder}");
        }

        // Write the doctype and html tag seperately since they have different 
        // conditions than the rest of the html.
        // Before the html closing tag it's time to handle the rest of the html
        private void HtmlBaseWrite()
        {
            if(htmlMainTags.Doctype != null)
            {
                WriteOpeningTag("!doctype ");
                WriteContent(htmlMainTags.Doctype.ToString());
                CloseOpeningTag("");
            }

            WriteOpeningTag("html");            
            
            if(htmlMainTags.Language != null)
                WriteContent($" lang = {htmlMainTags.Language}>");
            else
                WriteClosingTag("");
            
            HandleHtmlContent(fullJsonNode);
            
            WriteClosingTag("html");
        }

        // This is the base function for writing the html content
        // It takes the current node (name:value pair) that's being used
        // and for each value calls the correct function.
        // Seperated in to single tags and regular tags
        // whille disregarding ceratin "names" that are handled elsewhere
        public void HandleHtmlContent(JsonNode nodeObject)
        {
            foreach (var item in nodeObject.AsObject())
            {   
                if( ( singletonTags.Contains(item.Key.ToString()) ) && 
                    ( item.Key.ToString() != "doctype" ) )
                {
                    HandleSingletonTags(item);
                }
                else if( ( item.Key.ToString() != "attributes" ) && 
                    ( item.Key.ToString() != "doctype" ) && 
                    ( item.Key.ToString() != "language" ) )
                {
                    HandleRegularTags(item);
                }
            }
        }
        
        private void HandleRegularTags(KeyValuePair<string, JsonNode?> item)
        {
            // When the value of key:value pair is in object
            // we're either dealing with attributes or nesting of tags
            if(item.Value.GetType() == typeof(JsonObject))
            {
                WriteOpeningTag(item.Key);

                foreach (var innerItem in item.Value.AsObject())
                {
                    if(innerItem.Key.ToString() == "attributes")
                    {
                        HandleAttributes(innerItem.Value);
                    }
                }

                CloseOpeningTag(item.Key);
                HandleHtmlContent(item.Value);  
                WriteClosingTag(item.Key);   
            }
            // When the value on key:value pair is an array
            // we're dealing with multiple tags of the same type
            else if(item.Value.GetType() == typeof(JsonArray))
            {
                foreach (var arrayItem in item.Value.AsArray())
                {
                    WriteOpeningTag(item.Key);  

                    if(arrayItem.GetType() == typeof(JsonObject))
                    {
                        foreach (var arrayObjectItem in arrayItem.AsObject())
                        {
                            if(arrayObjectItem.Key.ToString() == "attributes")
                            {
                                HandleAttributes(arrayObjectItem.Value);
                            }      
                        }

                        CloseOpeningTag(item.Key);
                        HandleHtmlContent(arrayItem.AsObject()); 
                        WriteClosingTag(item.Key);   
                    }
                    else
                    {
                        CloseContentClosingTagPattern(item.Key, arrayItem.ToString(), item.Key);
                    }
                }
            }
            else
            {
                WriteOpeningTag(item.Key);

                CloseContentClosingTagPattern(item.Key, item.Value.ToString(), item.Key);
            }  
        }

        private void HandleSingletonTags(KeyValuePair<string, JsonNode?> item)
        {
            if(item.Value.GetType() == typeof(JsonArray))
            {
                foreach (var arrayItem in item.Value.AsArray())
                {
                    if(arrayItem.GetType()== typeof(JsonObject))
                    {
                        WriteOpeningTag(item.Key); 
                        HandleAttributes(arrayItem);
                        CloseOpeningTag(item.Key);
                    }
                    else
                    {
                        if(item.Key.ToString() != "text")
                            CloseContentClosingTagPattern(item.Key, arrayItem.ToString(), item.Key);
                    }
                }
            }
            else
            {
                if(item.Value.GetType() == typeof(JsonObject))
                {
                    WriteOpeningTag(item.Key);
                    HandleAttributes(item.Value);
                    CloseOpeningTag(item.Key);
                }
                else 
                {
                    WriteOpeningTag(item.Key);
                    WriteContent(item.Value.ToString());
                    CloseOpeningTag("");
                }
            }
        }

        public void HandleAttributes(JsonNode attributeNode)
        {
            foreach (var attributeItem in attributeNode.AsObject())
            {
                if(attributeItem.Value.GetType() == typeof(JsonObject))
                {
                    int i=0;
                    WriteContent($" {attributeItem.Key} = \"");

                    foreach (var innerAttributeItem in attributeItem.Value.AsObject())
                    {
                        i++;

                        WriteContent($"{innerAttributeItem.Key}:{innerAttributeItem.Value}");

                        if(i != attributeItem.Value.AsObject().Count())
                        {
                            WriteContent("; ");
                        }
                        else
                        {
                            WriteContent("\"");
                        }
                    }
                }
                else
                {
                    WriteContent($" {attributeItem.Key} = \"{attributeItem.Value}\"");
                }
            }
        }

        // All the methods below are created for better code readability
        // The names tell you enough 
        void CloseContentClosingTagPattern(string closingArrow, string content, string closginTag)
        {
            CloseOpeningTag(closingArrow);
            WriteContent(content);
            WriteClosingTag(closginTag);
        }

        // The reason for  "!= text" is for names/keys that are for plain text
        private void WriteOpeningTag(string itemKey)
        {
            HtmlStringBuilder.Append("\n");

            if(itemKey != "text")
            {
                HtmlStringBuilder.Append($"<{itemKey}");   
            }
        }
       
        private void CloseOpeningTag(string itemKey)
        {
            if(itemKey != "text")
            {
                HtmlStringBuilder.Append(">\n");
            }
        }

        private void WriteClosingTag(string itemKey)
        {
            HtmlStringBuilder.Append("\n");

            if(itemKey != "text")
            {
                HtmlStringBuilder.Append($"</{itemKey}>\n");
            }
        }

        private void WriteContent(string content)
        {
            HtmlStringBuilder.Append(content);
        }
    }
  
    public class HtmlBase
    {
        public JsonNode? Doctype {get; set;}
        public JsonNode? Language {get;set;}
    }
}