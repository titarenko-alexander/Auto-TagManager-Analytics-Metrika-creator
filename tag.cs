using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using Google.Apis.TagManager.v2.Data;

namespace ConsoleApp2
{
    public class tag
    {
        private string name;
        private string type;
        private string value;
        private string trigger;
        private string triggerId;

        public Tag setBody(Dictionary<string, string> triggersId)
        {
            if (triggersId.ContainsKey(trigger))
            {
                triggerId = triggersId[trigger];
            }
            else
            {
                triggerId = "2147479553";
            }
            if (type == "html")
            {
                Tag body = new()
                {
                    Name = name,
                    FiringTriggerId = new Collection<string>() {triggerId},
                    Type = "html",
                    Parameter = new Collection<Parameter>()
                    {
                        new Parameter()
                        {
                            Key = "html",
                            Type = "template",
                            Value = value,
                        },
                        new Parameter()
                        {
                            Key = "convertJsValuesToExpressions",
                            Type = "boolean",
                            Value = "true",
                        },
                        new Parameter()
                        {
                            Key = "usePostscribe",
                            Type = "boolean",
                            Value = "false",
                        }
                    }
                };
                return body;
            } else if (type == "uaEvent")
            {
                Tag body = new()
                {
                    FiringTriggerId = new Collection<string>(){triggerId},
                    Name = name,
                    Type = "ua",
                    Parameter = new Collection<Parameter>()
                    {
                        new Parameter()
                        {
                            Type = "boolean",
                            Key = "overrideGaSettings",
                            Value = "true",
                        },
                        new Parameter()
                        {
                            Type = "template",
                            Key = "trackingId",
                            Value = "{{uaSettings}}",
                        },
                        new Parameter()
                        {
                            Type = "template",
                            Key = "trackType",
                            Value = "TRACK_EVENT",
                        },
                        new Parameter()
                        {
                          Type  = "template",
                          Key = "eventValue",
                          Value = "{{eventValue}}",
                        },
                        new Parameter()
                        {
                            Type = "template",
                            Key = "eventCategory",
                            Value = "{{eventCategory}}",
                        },
                        new Parameter()
                        {
                            Type = "template",
                            Key = "eventAction",
                            Value = "{{eventAction}}",
                        },
                        new Parameter()
                        {
                            Type = "template",
                            Key = "eventLabel",
                            Value = "{{eventLabel}}"
                        }
                    }
                };
                return body;
            } else if (type == "ua")
            {
                Tag body = new()
                {
                    FiringTriggerId = new Collection<string>(){triggerId},
                    Name = name,
                    Type = "ua",
                    Parameter = new Collection<Parameter>()
                    {
                        new Parameter()
                        {
                            Type = "boolean",
                            Key = "overrideGaSettings",
                            Value = "true",
                        },
                        new Parameter()
                        {
                            Type = "template",
                            Key = "trackingId",
                            Value = "{{uaSettings}}",
                        },
                        new Parameter()
                        {
                            Type  = "template",
                            Key = "trackType",
                            Value = "TRACK_EVENT",
                        },
                        new Parameter()
                        {
                            Type = "template",
                            Key = "eventValue",
                            Value = "{{eventValue}}",
                        },
                        new Parameter()
                        {
                            Type = "template",
                            Key = "eventCategory",
                            Value = "{{eventCategory}}",
                        },
                        new Parameter()
                        {
                            Type = "template",
                            Key = "eventAction",
                            Value = "{{eventAction}}",
                        },
                        new Parameter()
                        {
                           Type = "template",
                           Key = "eventLabel",
                           Value = "{{eventLabel}}",
                        },
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

        public string setValue()
        {
            return value;
        }

        public void setTrigger(string t)
        {
            trigger = t;
        }

        public string getTrigger()
        {
            return trigger;
        }
    }
}