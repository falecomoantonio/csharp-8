using System;
using System.Collections.Generic;
using System.Linq;
using Codenation.Challenge.Models;

namespace Codenation.Challenge.Services
{
    public class CandidateService : ICandidateService
    {
        private CodenationContext db;
        public CandidateService(CodenationContext context) => this.db = context;

        public IList<Candidate> FindByAccelerationId(int accelerationId) => 
                      this.db.Candidates
                          .Join(this.db.Companies, cd => cd.CompanyId, cp => cp.Id, (cd, cp) => new { cd, cp })
                          .Where(temp0 => (temp0.cd.AccelerationId == accelerationId))
                          .Select(temp0 => temp0.cd).Distinct().ToList();
        

        public IList<Candidate> FindByCompanyId(int companyId) =>
                      this.db.Candidates
                          .Join(this.db.Companies, cd => cd.CompanyId, cp => cp.Id, (cd, cp) => new { cd, cp })
                          .Where(temp0 => (temp0.cp.Id == companyId))
                          .Select(temp0 => temp0.cd).Distinct().ToList();
        

        public Candidate FindById(int userId, int accelerationId, int companyId) =>
                      this.db.Candidates
                          .Where(p => p.UserId == userId && p.AccelerationId == accelerationId && p.CompanyId == companyId)
                          .FirstOrDefault();
        
        public Candidate Save(Candidate candidate)
        {
            if (candidate is null)
            {
                throw new Exception("Usuario invalido!");
            }

            if (this.db.Candidates.Any(sb => sb.AccelerationId == candidate.AccelerationId && sb.CompanyId == candidate.CompanyId))
            {
                this.db.Update(candidate);
            }
            else
            {
                this.db.Add(candidate);
            }
            this.db.SaveChanges();

            return candidate;
        }
    }
}