﻿using System;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Users
{
    public class LoggedInUser
    {
        public int UserId { get; private set; }

        public LoggedInUser(int userId)
        {
            UserId = userId;
        }
    }
}