using Google.Apis.TagManager.v2.Data;
using System.Collections.Generic;

namespace ConsoleApp2
{
    public class variable
    {
        private string name;
        private string type;
        private string value;
        
        
        public Variable setBody()
        {
            if (type == "c")
            {
                Variable body = new()
                {
                    Name = name,
                    Type = type,
                    Parameter = new List<Parameter>()
                    {
                        new Parameter()
                        {
                            Type = "template",
                            Key = "value",
                            Value = value,
                        }
                    }
                };
                return body;
            }
            else if (type == "v")
            {
                Variable body = new()
                {
                    Name = name,
                    Type = type,
                    Parameter = new List<Parameter>()
                    {
                        new Parameter()
                        {
                            Type = "template",
                            Key = "name",
                            Value = value,
                        }
                    }
                };
                return body;
            }
            else if(type == "gas")
            {
                Variable body = new()
                {
                    Name = name,
                    Type = type,
                    Parameter = new List<Parameter>()
                    {
                        new Parameter()
                        {
                            Type = "template",
                            Key = "cookieDomain",
                            Value = value,
                        }
                    }
                };
                return body;
            }
            else if(type == "jsm")
            {
                Variable body = new()
                {
                    Name = name,
                    Type = type,
                    Parameter = new List<Parameter>()
                    {
                        new Parameter()
                        {
                            Type = "template",
                            Key = "javascript",
                            Value = value,
                        }
                    }
                };
                return body;
            }
            return null;
        }

        public void setName(string n)
        {
            name = n;
        }

        public string getName()
        {
            return name;
        }

        public void setType(string t)
        {
            type = t;
        }

        public string getType()
        {
            return type;
        }

        public void setValue(string v)
        {
            value = v;
        }

        public string getValue()
        {
            return value;
        }
        
    }
}
