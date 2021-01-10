using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp2
{
    public class yandexApi
    {
        public class Goals
        {
            public JObject getGoals(string token, string metrikaId)
            {
                HttpClient client = new ();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth",token);
                var result = client.GetAsync($"https://api-metrika.yandex.net/management/v1/counter/{metrikaId}/goals");
                var content = result.Result.Content.ReadAsStringAsync().Result;
                client.Dispose();
                JObject parsed = JObject.Parse(content);
                return(parsed);
                
            }

            public JObject editGoals(string token, string metrikaId,string goalId)
            {
                HttpClient client = new();
                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", token);
                var pocoObject = new 
                {
                    id = "148237534",
                    name = "goal",
                };
                var myContent = JsonConvert.SerializeObject(pocoObject);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result =
                    client.PutAsync($"https://api-metrika.yandex.net/management/v1/counter/{metrikaId}/goal/{goalId}",
                        byteContent);
                Console.WriteLine((byteContent));
                var content = result.Result.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
                client.Dispose();
                JObject parsed = JObject.Parse(content);
                return (parsed);
            }
            public void createGoals(string token, string metrikaId, Dictionary<string,List<string>> goalslist)
            {
                for (int i = 0; i < goalslist.Count; i++)
                {
                    HttpClient client = new();
                    Dictionary<string, string> condiditionsParameters = new()
                    {
                        {"type", "exact"},
                        {"url", $"{goalslist[Convert.ToString(i)][5]}_{goalslist[Convert.ToString(i)][6]}_{goalslist[Convert.ToString(i)][7]}"},

                    };
                    List<Dictionary<string, string>> conditionsParams = new() {condiditionsParameters};
                    var goalParams = new
                    {
                        name = goalslist[Convert.ToString(i)][0],
                        type = "action",
                        is_retargeting = 0,
                        conditions = conditionsParams,
                    };
                    var data = new
                    {
                        goal = goalParams,
                    };
                    var myContent = JsonConvert.SerializeObject(data);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var body = new ByteArrayContent(buffer);
                    body.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", token);
                    var result =
                        client.PostAsync($"https://api-metrika.yandex.net/management/v1/counter/{metrikaId}/goals",
                            body);
                    var content = result.Result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"Цель {goalslist[Convert.ToString(i)][0]} создана");
                }
                System.Threading.Thread.Sleep(3000);
            }
        }
        
        
    }
}