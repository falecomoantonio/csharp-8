using System;
using System.Linq;
using System.Collections.Generic;
using Codenation.Challenge.Models;

namespace Codenation.Challenge.Services
{
    public class ChallengeService : IChallengeService
    {
        private CodenationContext db;

        public ChallengeService(CodenationContext context) => this.db = context;

        public IList<Models.Challenge> FindByAccelerationIdAndUserId(int accelerationId, int userId) =>
                                 this.db.Challenges.Join(this.db.Accelerations, ch => ch.Id, ac => ac.ChallengeId, (ch, ac) => new { ch, ac })
                                     .Join(this.db.Candidates, temp0 => temp0.ac.Id, ca => ca.AccelerationId, (temp0, ca) => new { temp0, ca })
                                     .Join(this.db.Users, temp1 => temp1.ca.UserId, us => us.Id, (temp1, us) => new { temp1, us })
                                     .Where(temp2 => ((temp2.temp1.temp0.ac.Id == accelerationId) && (temp2.us.Id == userId)))
                                     .Select(temp2 => temp2.temp1.temp0.ch).OrderBy(a => a.Id).Distinct().ToList();
        

        public Models.Challenge Save(Models.Challenge challenge)
        {
            if (challenge is null)
                throw new Exception("Objeto Invalido!");

            if (challenge.Id == 0)
            {
                this.db.Add(challenge);
            }
            else
            {
                this.db.Update(challenge);
            }

            this.db.SaveChanges();
            return challenge;
        }
    }
}