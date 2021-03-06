﻿using Chart.Mvc.ComplexChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using T001.Models;
using Tweetinvi.Logic;
using Tweetinvi.Models;

namespace T001.Model_View
    {
    public class View_Connections
        {
        //get data for previous seven days
        public GetUserDetails userDetails;
        public GetLocation location;

        public static string Mentions;
        public static string AvgMentionsPerDay;
        public static string UserMentionYou;
        public static string YourTweetRetweeted;
        public static string YourTweetRetweetedInTotal;
        public static DateTime FromDate;
        public static DateTime ToDate;

        private IUser user;
        public Graph profileGraph;

        public View_Connections(IUser user)
            {
            this.user = user;
            ToDate = DateTime.Today;
            FromDate = DateTime.Today.AddMonths(-1);

            location = new GetLocation();
            userDetails = new GetUserDetails(this.user);
            profileGraph = new Graph(user, FromDate, ToDate);
            }

        //first graph
        Dictionary<List<string>, IEnumerable<ComplexDataset>> LineGraph_NoOfTweetsInWhichUserIsMentioned(IEnumerable<ITweet> query)
            {
            Dictionary<List<string>, IEnumerable<ComplexDataset>> dt = new Dictionary<List<string>, IEnumerable<ComplexDataset>>();

            //TODO: Some Serious Stuff

            return dt;
            }

        //userDetails.MostFollowedUsersThatMentionedYou
        //userDetails.UsersWhoMentionedYouTheMost

        //location.MapLocation

        //second graph
        Dictionary<List<string>, IEnumerable<ComplexDataset>> BarGraph_NoOfTweetsRetweeted(IEnumerable<ITweet> query)
            {
            Dictionary<List<string>, IEnumerable<ComplexDataset>> dt = new Dictionary<List<string>, IEnumerable<ComplexDataset>>();

            //TODO: Some Serious Stuff

            return dt;
            }
        }
    }