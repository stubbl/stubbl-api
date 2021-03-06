﻿using System.Collections.Generic;
using MongoDB.Bson;

namespace Stubbl.Api.Data.Collections.Users
{
    public class User
    {
        public User()
        {
            Teams = new Team[0];
        }

        public string EmailAddress { get; set; }
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Sub { get; set; }
        public IReadOnlyCollection<Team> Teams { get; set; }
    }
}