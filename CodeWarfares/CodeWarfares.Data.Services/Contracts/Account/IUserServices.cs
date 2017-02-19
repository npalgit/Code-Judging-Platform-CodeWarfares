﻿using CodeWarfares.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarfares.Data.Services.Contracts.Account
{
    public interface IUserServices
    {
        void AddSubmitionToUser(string userId, Submition submition);

        void AddProblemToUser(string userId, Problem problem);

        IQueryable<User> GetAll();

        IEnumerable<User> GetAllUsersWithPoints();

        User GetByUsername(string username);
    }
}
