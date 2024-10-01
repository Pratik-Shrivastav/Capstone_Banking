﻿using Capstone_Banking.Model;

namespace Capstone_Banking.Repository
{
    public interface IUserAuthRepository
    {
        public Task<User> AddUser(User newUser);

        public Task<User> GetUserByUserName(string userName);

    }
}
