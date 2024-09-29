using SocialLink.Domain.Interfaces;
using System.Collections.Concurrent;

namespace SocialLink.infrastructure.Services
{
    public class EmailResetCodeValidatorService : IEmailResetCodeValidator
    {

        private ConcurrentDictionary<string, ResetCodeAndStashTime> stash = new ConcurrentDictionary<string, ResetCodeAndStashTime>();



        public void StashEmailResetCode(string email, string ResetCode)
        {
            stash[email] = new ResetCodeAndStashTime(ResetCode, DateTime.UtcNow);
        }

        public bool Validate(string email, string ResetCode)
        {

            if (!stash.TryGetValue(email, out var value))
            {
                return false;
            }

            var difference = DateTime.UtcNow - value.StashTime;

            if (difference.TotalMinutes > 5)
            {
                return false;
            }

            if (ResetCode != value.ResetCode)
            {
                return false;
            }

            stash.TryRemove(email,out _);
            return true;


        }



        record class ResetCodeAndStashTime(string ResetCode, DateTime StashTime);

    }



}
