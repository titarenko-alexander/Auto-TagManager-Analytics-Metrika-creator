using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Google.Apis.TagManager.v2.Data;

namespace ConsoleApp2
{
    public class trigger
    {
        private string name;
        private string type;
        private string value;

        public Trigger setBody()
        {
            if (type == "Timer")
            {
                Trigger body = new()
                {
                    Name = name,
                    Type = type,
                    Interval = new Parameter()
                    {
                        Type = "template",
                        Value = "15000"
                    },
                    Limit = new Parameter()
                    {
                        Type = "template",
                        Value = "1"
                    },
                    AutoEventFilter = new List<Condition>()
                    {
                        new Condition()
                        {
                            Type = "matchRegex",
                            Parameter = new List<Parameter>()
                            {
                                new Parameter()
                                {
                                    Type = "template",
                                    Key = "arg0",
                                    Value = "{{eventCategory}}"
                                },
                                new Parameter()
                                {
                                    Type = "template",
                                    Key = "arg1",
                                    Value = ".*"
                                }
                            }
                        }
                    }
                };
                return body;
            } else if (type == "click")
            {
                Trigger body = new()
                {
                    Name = name,
                    Type = type,
                };
                return body;
            } else if (type == "domReady")
            {
                Trigger body = new()
                {
                    Name = name,
                    Type = type,
                };
                return body;
            } else if (type == "customEvent")
            {
                Trigger body = new()
                {
                    Name = name,
                    Type = type,
                    CustomEventFilter = new List<Condition>()
                    {
                        new Condition()
                        {
                            Type = "equals",
                            Parameter = new List<Parameter>()
                            {
                                new Parameter()
                                {
                                    Type = "template",
                                    Key = "arg0",
                                    Value = "{{_event}}"
                                },
                                new Parameter()
                                {
                                    Type = "template",
                                    Key = "arg1",
                                    Value = value,
                                }
                            }
                        }
                    }
                };
                return body;
            } else if (type == "windowLoaded")
            {
                Trigger body = new()
                {
                    Name = name,
                    Type = type,
                };
                return body;
            } else if (type == "pageView")
            {
                Trigger body = new()
                {
                    Name = name,
                    Type = type,
                };
                return body;
            }
            return null;
        }
        public string getName()
        {
            return name;
        }

        public void setName(string n)
        {
            name = n;
        }

        public string getType()
        {
            return type;
        }

        public void setType(string t)
        {
            type = t;
        }

        public string getValue()
        {
            return value;
        }

        public void setValue(string v)
        {
            value = v;
        }
    }
}