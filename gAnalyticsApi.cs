using System;
using System.Collections.Generic;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Analytics.v3;
using Google.Apis.Services;

namespace ConsoleApp2
{
    public class gAnalyticsApi
    {
        private string profileId;
        private string webPropertyId;
        private string accountId;
        private AnalyticsService service;
        public void setAnalyticsData(string p, string w, string a, AnalyticsService s)
        {
            profileId = p;
            webPropertyId = w;
            accountId = a;
            service = s;
        }
        public void getGoals()
        {
            var goals = service.Management.Goals.List(accountId,webPropertyId,profileId).Execute();
            for (int i = 0; i < goals.Items.Count; i++)
            {
                Console.WriteLine(goals.Items[i].Name);
            };
        }
        public void createGoals(Dictionary<string, List<string>> goalsList, AnalyticsService service)
        {
            for (int i = 0; i < goalsList.Count; i++)
            {
                Goal goalBody = new Goal()
                {
                    Name = goalsList[Convert.ToString(i)][0],
                    Type = goalsList[Convert.ToString(i)][2],
                    Id = goalsList[Convert.ToString(i)][1],
                    Active = true,
                    EventDetails = new Goal.EventDetailsData()
                    {
                        UseEventValue = true,
                        EventConditions = new List<Goal.EventDetailsData.EventConditionsData>()
                        {
                            new Goal.EventDetailsData.EventConditionsData()
                            {
                                Type = "CATEGORY",
                                MatchType = "EXACT",
                                Expression = goalsList[Convert.ToString(i)][5]
                            },
                            new Goal.EventDetailsData.EventConditionsData()
                            {
                                Type = "ACTION",
                                MatchType = "EXACT",
                                Expression = goalsList[Convert.ToString(i)][6]
                            },
                            new Goal.EventDetailsData.EventConditionsData()
                            {
                                Type = "LABEL",
                                MatchType = "EXACT",
                                Expression = goalsList[Convert.ToString(i)][7]
                            }
                        },
                    },
                };
                var goals = service.Management.Goals.Insert(goalBody,accountId,webPropertyId,profileId).Execute();
                Console.WriteLine($"Цель {goalsList[Convert.ToString(i)][0]} создана в Analytics");
                System.Threading.Thread.Sleep(3000);
            }
        }
    }
}