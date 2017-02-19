﻿using CodeWarfares.Data.Contracts;
using CodeWarfares.Data.Models;
using CodeWarfares.Data.Services.Contracts.Account;
using CodeWarfares.Data.Services.Enums;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodeWarfares.Data.Services.Account
{
    public class UserServices : IUserServices
    {
        private IRepository<User> users;

        public UserServices(IRepository<User> users)
        {
            if (users == null)
            {
                throw new ArgumentNullException("Users cannot be null");
            }

            this.users = users;
        }

        public IQueryable<User> GetAll()
        {
            return this.users.All();
        }

        public void AssignRole(User user, IdentityUserRole role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user cannot be null");
            }

            user.Roles.Add(role);
            this.users.SaveChanges();
        }

        public void AddSubmitionToUser(string userId, Submition submition)
        {
            if (submition == null)
            {
                throw new ArgumentNullException("submition cannot be null");
            }

            this.users.GetById(userId).Submition.Add(submition);
            this.users.SaveChanges();
        }

        public void AddProblemToUser(string userId, Problem problem)
        {
            if (problem == null)
            {
                throw new ArgumentNullException("problem cannot be null");
            }

            var user = this.users.GetById(userId);
            bool shouldAddProblem = user.Problems.FirstOrDefault(p => p.Id == problem.Id) == null;

            if (shouldAddProblem)
            {
                user.Problems.Add(problem);
            }

            this.users.SaveChanges();
        }

        public User GetByUsername(string username)
        {
            return this.users.All().FirstOrDefault(x => x.UserName == username);
        }

        public IEnumerable<User> GetAllUsersWithPoints()
        {
            var allUsers = this.GetAll().ToList();

            foreach (var user in allUsers)
            {
                var problems = user.Problems;

                user.TotalPoints = 0;

                foreach (var problem in problems)
                {
                    double biggestCompletePercentage = problem.Submitions.Where(s => s.AuthorId == user.Id)
                        .OrderByDescending(x => x.CompletedPercentage).FirstOrDefault().CompletedPercentage;

                    user.TotalPoints += (long)((biggestCompletePercentage / 100) * problem.Xp);
                }
            }

            this.users.SaveChanges();

            return allUsers;
        }
    }
}
