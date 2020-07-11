using System;
using System.Collections.Generic;
using System.Linq;
using Codenation.Challenge.Models;

namespace Codenation.Challenge.Services
{
    public class AccelerationService : IAccelerationService
    {
        private CodenationContext db;

        public AccelerationService(CodenationContext context) => this.db = context;

        public IList<Acceleration> FindByCompanyId(int companyId) =>
                      this.db.Accelerations
                          .Join(this.db.Candidates, a => a.Id, c => c.AccelerationId, (a, c) => new { a, c })
                          .Join(this.db.Companies, temp0 => temp0.c.CompanyId, cp => cp.Id, (temp0, cp) => new { temp0, cp })
                          .Where(temp1 => (temp1.cp.Id == companyId))
                          .Select(temp1 => temp1.temp0.a).OrderBy(a => a.Id).Distinct().ToList();
        

        public Acceleration FindById(int id) => this.db.Accelerations.Find(id);

        public Acceleration Save(Acceleration acceleration)
        {
            if (acceleration is null)
            {
                throw new Exception("Aceleração inválida!");
            }

            if (acceleration.Id == 0)
            {
                this.db.Add(acceleration);
            }
            else
            {
                this.db.Update(acceleration);

            }
            this.db.SaveChanges();

            return acceleration;
        }
    }
}