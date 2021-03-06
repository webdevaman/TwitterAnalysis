﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tweetinvi.Models;

namespace T001.Models
    {
    public class Graph
        {
        IEnumerable<ITweet> temp;
        IUser user;

        public GraphData _LineGraph_NoOfTweetsByDate;
        public GraphData _BarGraph_NoOfTweetsByDayOfWeek;
        public GraphData _BarGraph_NoOfTweetsByHour;
        public GraphData _LineGraph_Mentions;
        public GraphData _LineGraph_Retweets;

        private Dictionary<DayOfWeek, string> converterOfDays = new Dictionary<DayOfWeek, string>();

        public Graph(IUser user, DateTime fromDate, DateTime toDate)
            {
            this.user = user;
            temp = GetTweets.GetTweetsForKeyowrds(user, true)
                            .Where(x => toDate.Date >= x.CreatedAt.Date && x.CreatedAt.Date >= fromDate.Date);

            _LineGraph_NoOfTweetsByDate = new GraphData();
            _BarGraph_NoOfTweetsByDayOfWeek = new GraphData();
            _BarGraph_NoOfTweetsByHour = new GraphData();

            temp = GetTweets.GetTweetsForConnection(user);

            _LineGraph_Mentions = new GraphData();
            _LineGraph_Retweets = new GraphData();

            feedData();

            LineGraph_NoOfTweetsByDate(fromDate, toDate);
            _LineGraph_NoOfTweetsByDate.Run();

            BarGraph_NoOfTweetsByDayOfWeek();
            _BarGraph_NoOfTweetsByDayOfWeek.Run();

            BarGraph_NoOfTweetsByHour();
            _BarGraph_NoOfTweetsByHour.Run();

            LineGraph_Mentions();
            _LineGraph_Mentions.Run();

            LineGraph_Retweets();
            _LineGraph_Retweets.Run();
            }

        public void LineGraph_Mentions()
            {
            _LineGraph_Mentions.LabelText = "Tweets in which you are mentions.";
            temp.ToList().Where(x => (x.CreatedBy.ScreenName != user.ScreenName) && x.UserMentions.Any(y => y.Id == user.Id))
                .GroupBy(x => x.CreatedAt.Date.ToString("dd. MMM-yy"))
                .ToList().ForEach(x =>
                {
                    _LineGraph_Mentions.Label.Add(x.Key);
                    _LineGraph_Mentions.Data.Add(x.Count());
                });
            }

        public void LineGraph_Retweets()
            {
            _LineGraph_Retweets.LabelText = "Tweets in which retweeted by others";
            temp.ToList().Where(x => (x.CreatedBy.ScreenName == user.ScreenName) && x.RetweetCount > 0)
                .GroupBy(x => x.CreatedAt.Date.ToString("dd. MMM-yy"))
                .ToList().ForEach(x =>
                {
                    _LineGraph_Retweets.Label.Add(x.Key);
                    _LineGraph_Retweets.Data.Add(x.Count());
                });
            }

        private void feedData()
            {
            converterOfDays.Add(DayOfWeek.Sunday, "Sunday");
            converterOfDays.Add(DayOfWeek.Monday, "Monday");
            converterOfDays.Add(DayOfWeek.Tuesday, "Tuesday");
            converterOfDays.Add(DayOfWeek.Wednesday, "Wednesday");
            converterOfDays.Add(DayOfWeek.Thursday, "Thursday");
            converterOfDays.Add(DayOfWeek.Friday, "Friday");
            converterOfDays.Add(DayOfWeek.Saturday, "Saturday");
            }

        public void LineGraph_NoOfTweetsByDate(DateTime fromDate, DateTime toDate)
            {
            bool IsPresent = false;
            _LineGraph_NoOfTweetsByDate.LabelText = "Tweets By Date";
            var flag = temp.OrderByDescending(x => x.CreatedAt.Date)
                           .GroupBy(x => x.CreatedAt.Date.ToString("dd. MMM-yy"));

            while (toDate.Date != fromDate.Date)
                {
                foreach (var item in flag)
                    {
                    IsPresent = false;
                    if (item.First().CreatedAt.Date == toDate.Date)
                        {
                        IsPresent = true;
                        _LineGraph_NoOfTweetsByDate.Label.Add(item.Key);
                        _LineGraph_NoOfTweetsByDate.Data.Add(item.Count());
                        break;
                        }
                    }
                if (!IsPresent)
                    {
                    _LineGraph_NoOfTweetsByDate.Label.Add(toDate.Date.ToString("dd. MMM-yy"));
                    _LineGraph_NoOfTweetsByDate.Data.Add(0);
                    }
                toDate = toDate.AddDays(-1);
                }
            }

        public void BarGraph_NoOfTweetsByDayOfWeek()
            {
            _BarGraph_NoOfTweetsByDayOfWeek.LabelText = "Tweets By Day of week";
            string value;
            temp.GroupBy(x => x.CreatedAt.DayOfWeek).ToList()
            .ForEach(x =>
            {
                if (converterOfDays.TryGetValue(x.Key, out value))
                    {
                    _BarGraph_NoOfTweetsByDayOfWeek.Label.Add(value);
                    _BarGraph_NoOfTweetsByDayOfWeek.Data.Add(x.Count());
                    }
            });
            }

        public void BarGraph_NoOfTweetsByHour()
            {
            _BarGraph_NoOfTweetsByHour.LabelText = "Tweets By Hour of Day";
            temp.GroupBy(x => x.CreatedAt.Hour).ToList()
            .ForEach(x =>
            {
                _BarGraph_NoOfTweetsByHour.Label.Add(x.Key.ToString());
                _BarGraph_NoOfTweetsByHour.Data.Add(x.Count());
            });
            }
        }
    }