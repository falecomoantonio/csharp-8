using System;
using System.Collections.Generic;
using System.Linq;
using Codenation.Challenge.Models;

namespace Codenation.Challenge.Services
{
    public class SubmissionService : ISubmissionService
    {
        private CodenationContext db;

        public SubmissionService(CodenationContext context) => this.db = context;

        public IList<Submission> FindByChallengeIdAndAccelerationId(int challengeId, int accelerationId) =>
                                 this.db.Candidates.Where(candidate => candidate.AccelerationId == accelerationId)
                                     .Join(this.db.Users, candidate => candidate.UserId, user => user.Id, (_, user) => user)
                                     .Join(this.db.Submissions, user => user.Id, submission => submission.UserId, (_, submission) => submission)
                                     .Where(submission => submission.ChallengeId == challengeId).Distinct().ToList();
        

        public decimal FindHigherScoreByChallengeId(int challengeId) =>
                                  this.db.Submissions.Join(this.db.Challenges, sb => sb.ChallengeId, ch => ch.Id, (sb, ch) => new { sb, ch })
                                      .Where(temp0 => (temp0.ch.Id == challengeId))
                                      .Select(temp0 => temp0.sb).Distinct().Max(sb => sb.Score);
        

        public Submission Save(Submission submission)
        {
            if (submission is null)
                throw new Exception("Objeto Invalido!");

            if (this.db.Submissions.Any(sb => sb.ChallengeId == submission.ChallengeId && sb.UserId == submission.UserId))
            {
                this.db.Update(submission);
            }
            else
            {
                this.db.Add(submission);
            }

            this.db.SaveChanges();
            return submission;
        }
    }
}