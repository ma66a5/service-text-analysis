using System;

namespace Service.TextAnalysis.Models
{
    public sealed class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }
    }
}