using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Domain.Entities
{
    public class User
    {
        public string Id { get; }
        public string Username { get; }
        public string DisplayName { get; }
        public string Email { get; }
        public string PasswordHash { get; }

        internal User(string username, string email, string displayName, string id, string passwordHash)
        {
            Username = username;
            Email = email;
            DisplayName = displayName;
            Id = id;
            PasswordHash = passwordHash;
        }
    }
}
