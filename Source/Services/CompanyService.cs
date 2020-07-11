using System;
using System.Collections.Generic;
using System.Linq;
using Codenation.Challenge.Models;
using Microsoft.EntityFrameworkCore;

namespace Codenation.Challenge.Services
{
    public class CompanyService : ICompanyService
    {
        private CodenationContext db;

        public CompanyService(CodenationContext context) => this.db = context;

        public IList<Company> FindByAccelerationId(int accelerationId) => this.db.Candidates.Where(c => c.AccelerationId == accelerationId).Include(c => c.Company).Select(c => c.Company).ToList();

        public Company FindById(int id) => this.db.Companies.Find(id);

        public IList<Company> FindByUserId(int userId) => this.db.Candidates.Where(c => c.UserId == userId).Include(c => c.Company).Select(c => c.Company).ToList();

        public Company Save(Company company)
        {
            if (company is null)
            {
                throw new Exception("Objeto invalido!");
            }

            if (company.Id == 0)
            {
                this.db.Add(company);
            }
            else
            {
                this.db.Update(company);
            }

            this.db.SaveChanges();
            return company;
        }
    }
}