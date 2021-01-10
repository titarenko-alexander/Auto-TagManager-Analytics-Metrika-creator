using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Logging;
using Google.Apis.Requests;
using Google.Apis.TagManager.v2;
using Google.Apis.Services;
using Google.Apis.TagManager.v2.Data;
using Google.Apis.Util.Store;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Analytics.v3;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace ConsoleApp2
{
    public class gtm
    {
        private string analyticsId;
        private string metrikaId;
        private TagManagerService service;
        private variable var = new();
        private trigger trig = new();
        private tag tag = new();
        private string gtmAccountId;
        private string gtmContainerId;
        private string gtmWorkspaceId;
        private Dictionary<int, List<string>> variablesInfo = new()
        {
            [0] = new List<string>() {"vkId", "c", "0"},
            [1] = new List<string>() {"clientId", "v", "clientId"},
            [2] = new List<string>() {"eventAction", "v", "acntxEventAction"},
            [3] = new List<string>() {"eventCategory", "c", "acntx"},
            [4] = new List<string>() {"eventLabel", "v", "acntxEventLabel"},
            [5] = new List<string>() {"eventValue", "v", "acntxEventValue"},
            [6] = new List<string>()
            {
                "extractVisitParamsAndAttachThemToUaHits", "jsm",
                "function() { var clientIdIndex = 1; return function (model) { var clientId = model.get('clientId'); model.set('dimension' + clientIdIndex, clientId); window.dataLayer.push({ 'event': 'visitParamsExtracted', 'clientId': clientId }); }}}"
            },
            [7] = new List<string>() {"fbId", "c", "0"},
            [8] = new List<string>() {"mtrgtId", "c", "0"},
            [9] = new List<string>() {"optId", "c", "0"},
            [10] = new List<string>() {"uaSettings", "c", "analyticsId"},
            [11] = new List<string>() {"mtrkId", "c", "metrikaId"},
        };

        private Dictionary<int, List<string>> triggersInfo = new()
        {
            [0] = new List<string>(){"15sec", "Timer", "15000"},
            [1] = new List<string>(){"clicks", "click", "all"},
            [2] = new List<string>(){"DOM Ready", "domReady", "all"},
            [3] = new List<string>(){"eventOccurred", "customEvent", "acntxEvent"}, 
            [4] = new List<string>(){"visitParamsExtracted", "customEvent", "visitParamsExtracted"},
            [5] = new List<string>(){"windowLoaded", "windowLoaded", "all"}, 
            [6] = new List<string>(){"AllPages", "pageView", "all"}
        };

        private Dictionary<int, List<string>> tagsInfo = new()
        {
            [0] = new List<string>() {"event_form", "html", "form_submit", "<script> window.dataLayer.push({ \"event\": \"acntxEvent\", \"acntxEventAction\": \"lead\", \"acntxEventLabel\": \"form\", \"acntxEventValue\": \"\" });</script>"},
            [1] = new List<string>(){"event_stay", "html", "15sec" ,"<script> window.dataLayer.push({ \"event\": \"acntxEvent\", \"acntxEventAction\": \"stay\", \"acntxEventLabel\": \"site\", \"acntxEventValue\": \"15\" });</script>"},
            [2] = new List<string>(){"tools_eventsSubscribing", "html", "windowLoaded", "<script> window.trackEvent = function(targetsSelector, eventName, action, label, valueSelector) { var elements = document.querySelectorAll(targetsSelector); var value = \"\"; var valueElem = document.querySelector(valueSelector); if (valueElem != undefined) value = valueElem.textContent.trim().replace(\",\", \".\"); var handler = function() { window.dataLayer.push({ \"event\": \"acntxEvent\", \"acntxEventAction\": action, \"acntxEventLabel\": label, \"acntxEventValue\": value }); }; for (var i = 0; i < elements.length; i++) { elements[i].addEventListener(eventName, handler); } if (elements.length > 0) { var consoleMsg = \"'\" + eventName + \"' will be tracked on elements below (action: \" + action; if (label.length > 0) consoleMsg += \", label: \" + label; if (value.length > 0) consoleMsg += \", value: \" + value; consoleMsg += \"):\"; console.info(consoleMsg, elements); } }; window.trackEvent(\"input\", \"focusin\", \"touch\", \"form\"); window.trackEvent(\"a[href^='tel:']\", \"click\", \"touch\", \"phone\"); window.trackEvent(\"a[href^='mailto:']\", \"click\", \"touch\", \"email\"); window.trackEvent(\"a[href*='m.me']\", \"click\", \"touch\", \"messenger\"); window.trackEvent(\"a[href*='t.me']\", \"click\", \"touch\", \"messenger\"); window.trackEvent(\"a[href*='whatsapp.com']\", \"click\", \"touch\", \"messenger\"); window.trackEvent(\"a[href*='instagram.com']\", \"click\", \"touch\", \"social\"); window.trackEvent(\"a[href*='facebook.com']\", \"click\", \"touch\", \"social\"); window.trackEvent(\"a[href*='fb.com']\", \"click\", \"touch\", \"social\"); window.trackEvent(\"a[href*='vk.com']\", \"click\", \"touch\", \"social\"); //window.trackEvent(\"button[class*='cart']\", \"click\", \"commerce\", \"addCartItem\"); //window.trackEvent(\"#place_order\", \"click\", \"commerce\", \"sendOrder\", \".total-price\"); </script>"},
            [3] = new List<string>(){"tools_postClientIdInForms", "html", "visitParamsExtracted", "<script> for (var i = 0; i < document.forms.length; i++) { var input = document.createElement('input'); input.type = 'hidden'; input.name = \"clientId\"; input.value = {{clientId}}; document.forms[i].appendChild(input); }</script>"},
            [4] = new List<string>(){"track_mtrk", "html", "AllPages", "<script type=\"text/javascript\" > (function(m,e,t,r,i,k,a){m[i]=m[i]||function(){(m[i].a=m[i].a||[]).push(arguments)}; m[i].l=1*new Date();k=e.createElement(t),a=e.getElementsByTagName(t)[0],k.async=1,k.src=r,a.parentNode.insertBefore(k,a)}) (window, document, \"script\", \"https://mc.yandex.ru/metrika/tag.js\", \"ym\"); ym({{mtrkId}}, \"init\", { clickmap:true, trackLinks:true, accurateTrackBounce:true, webvisor:true, ecommerce:\"dataLayer\" });</script>"},
            [5] = new List<string>(){"track_mtrkEvent", "html", "eventOccurred", "<script> var goal = '{{eventCategory}}_{{eventAction}}'; if (!!{{eventLabel}}) { goal = goal + '_{{eventLabel}}' } window['yaCounter{{mtrkId}}'].reachGoal(goal);</script>"},
            [6] = new List<string>(){"track_ua", "ua", "AllPages",  "<!-- Google Analytics --><script>(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)})(window,document,'script','https://www.google-analytics.com/analytics.js','ga');ga('create', '{{uaSettings}}', 'auto');ga('send', 'pageview');</script><!-- End Google Analytics -->"},
            [7] = new List<string>(){"track_uaEvent", "uaEvent", "eventOccurred", "<script> gtag('event', \"Внимание\", { 'event_category': \"arton\", 'event_label': \"lead\", 'value': \"}); </script>"}
        
        };
            
        private Dictionary<string,string> triggersId = new ();

        public Dictionary<string, string> getTriggersId()
        {
            getTriggers();
            return triggersId;
        }
        public gtm(TagManagerService serviceInput, string analyticsIdInput, string metrikaIdInput)
        {
            service = serviceInput;
            analyticsId = analyticsIdInput;
            variablesInfo[10] = new List<string>() {"uaSettings", "c", analyticsId};
            metrikaId = metrikaIdInput;
            variablesInfo[11] = new List<string>() {"mtrkId", "c", metrikaId};
        }

        public void setGtmData(string a, string c, string w)
        {
            gtmAccountId = a;
            gtmContainerId = c;
            gtmWorkspaceId = w;
        }
        public void getAccounts()
        {
            var accounts = service.Accounts.List().Execute();
            for (int i = 0; i < accounts.Account.Count; i++)
            {
                Console.WriteLine(accounts.Account[i].Name);
            }
        }

        public void getContainers()
        {
            var containers = service.Accounts.Containers.List($"accounts/{gtmAccountId}").Execute();
            for (int i = 0; i < containers.Container.Count; i++)
            {
                Console.WriteLine(containers.Container[i].Name);
            }
        }

        public string getVariables()
        {
            var variables = service.Accounts.Containers.Workspaces.Variables
                .List($"accounts/{gtmAccountId}/containers/{gtmContainerId}/workspaces/{gtmWorkspaceId}").Execute();
            for (int i = 0; i < variables.Variable.Count; i++)
            {
                Console.WriteLine(variables.Variable[i].Name);
            }
            return ("");
        }

        public void createVariables()
        {
            for (int i = 0; i < variablesInfo.Count; i++)
            {
                var.setName(variablesInfo[i][0]);
                var.setType(variablesInfo[i][1]);
                var.setValue(variablesInfo[i][2]);
                var newVariable =
                    service.Accounts.Containers.Workspaces.Variables.Create(var.setBody(),
                        $"accounts/{gtmAccountId}/containers/{gtmContainerId}/workspaces/{gtmWorkspaceId}").Execute();
                Console.WriteLine($"Создана переменная {newVariable.Name}");
                System.Threading.Thread.Sleep(3000);
            }
            System.Threading.Thread.Sleep(10000);
        }

        public void getTriggers()
        {
            var triggers = service.Accounts.Containers.Workspaces.Triggers
                .List($"accounts/{gtmAccountId}/containers/{gtmContainerId}/workspaces/{gtmWorkspaceId}").Execute();
            for (int i = 0; i < triggers.Trigger.Count; i++)
            {
                triggersId.Add(triggers.Trigger[i].Name, triggers.Trigger[i].TriggerId);
            }
        }

        public void createTrigges()
        {
            for (int i = 0; i < triggersInfo.Count; i++)
            {
                trig.setName(triggersInfo[i][0]);
                trig.setType(triggersInfo[i][1]);
                trig.setValue(triggersInfo[i][2]);
                var createTriggers = service.Accounts.Containers.Workspaces.Triggers.Create(trig.setBody(),
                    $"accounts/{gtmAccountId}/containers/{gtmContainerId}/workspaces/{gtmWorkspaceId}").Execute();
                Console.WriteLine($"Создан триггер {createTriggers.Name}");
                System.Threading.Thread.Sleep(7000);
            }
            System.Threading.Thread.Sleep(10000);
        }

        public void getTags()
        {
            var tags = service.Accounts.Containers.Workspaces.Tags
                .List($"accounts/{gtmAccountId}/containers/{gtmContainerId}/workspaces/{gtmWorkspaceId}").Execute();
            for(int i =0; i < tags.Tag.Count; i++)
            {
                Console.WriteLine(tags.Tag[i].Name);
            }
        }

        public void createTags(Dictionary<string, string> triggersId)
        {
            Console.WriteLine(triggersId);
            for (int i = 0; i < tagsInfo.Count; i++)
            {
                tag.setName(tagsInfo[i][0]);
                tag.setType(tagsInfo[i][1]);
                tag.setTrigger(tagsInfo[i][2]);
                tag.setValue(tagsInfo[i][3]);
                var newTag =
                    service.Accounts.Containers.Workspaces.Tags.Create(tag.setBody(triggersId),
                        $"accounts/{gtmAccountId}/containers/{gtmContainerId}/workspaces/{gtmWorkspaceId}").Execute();
                Console.WriteLine($"Создан тег {newTag.Name}");
                System.Threading.Thread.Sleep(3000);
            }
            System.Threading.Thread.Sleep(10000);
        }

    }

    public class gAnalytics
    {
        private string AccountId;
        private string WebPropertyId;
        private string profileId;
        
        public void setAnalyticsData(string a, string w, string p)
        {
            AccountId = a;
            WebPropertyId = w;
            profileId = p;
        }
    }

    public class yaMetrika
    {
        private string metrikaId;
        private string token;
        public string createGoals(Dictionary<string,List<string>> goals)
        {
            yandexApi.Goals ya = new();
            return(ya.createGoals(token,metrikaId,goals));
        }

        public JObject editGoals(string goalId)
        {
            yandexApi.Goals ya = new();
            return (ya.editGoals(token, metrikaId, goalId));
        }
        public JObject getGoals()
        {
            yandexApi.Goals ya = new();
            return (ya.getGoals(token, metrikaId));
        }
        public void setMetrikaData(string id, string oAuthtoken)
        {
            metrikaId = id;
            token = oAuthtoken;
        }
    }
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("GTM");
            Console.WriteLine("================================");
            new Program().Run().Wait();
        }

        private async Task Run()
        {
            UserCredential credential;
            using (var stream =
                new FileStream(
                    "client_secret_290942768668-6u6v233nm7besbb9hdca6m98abdtf5u7.apps.googleusercontent.com.json",
                    FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[]
                    {
                        TagManagerService.Scope.TagmanagerManageAccounts,
                        TagManagerService.Scope.TagmanagerEditContainers,
                        AnalyticsService.Scope.AnalyticsEdit,
                    },
                    "user", CancellationToken.None);
            }
            var service = new TagManagerService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
            });
            string gtmAccountId = "6002331513";
            string gtmContainerId = "33474630";
            string gtmWorkspaceId = "18";
            string analyticsAccountId = "184739284";
            string analyticsProfileId = "UA-181312454-1";
            string analyticsWebPropertyId = "235366299";
            string metrikaId = "70081573";
            string token = "AgAAAABITB3qAAa_C7gd041-vUBTkwNKz7VNuH0";
            gtm myacc = new(service,analyticsProfileId,metrikaId);
            gAnalytics myGaAccount = new();
            yaMetrika myYaMetrika = new();
            myYaMetrika.setMetrikaData(metrikaId, token);
            Dictionary<string, List<string>> goals = new()
            {
                ["0"] = new List<string>(){"Интерес", "1", "Event", "1", "true", "acntx", "interest", "site"},
                ["1"] = new List<string>(){"Внимание", "2", "Event", "1", "true", "acntx", "stay", "site"},
                ["2"] = new List<string>(){"Лид", "3", "Event", "1", "true", "acntx", "lead", "form"}
            };
            Console.WriteLine(myYaMetrika.createGoals(goals));
            //myacc.setGtmData(gtmAccountId, gtmContainerId, gtmWorkspaceId);
            //myacc.createVariables();
            //myacc.createTrigges();
            //myacc.createTags(myacc.getTriggersId());
        }
    }
}
