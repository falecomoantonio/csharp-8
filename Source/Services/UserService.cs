using System;
using System.Collections.Generic;
using System.Linq;
using Codenation.Challenge.Models;

namespace Codenation.Challenge.Services
{
    public class UserService : IUserService
    {
        private CodenationContext db;

        public UserService(CodenationContext context) => this.db = context;

        public IList<User> FindByAccelerationName(string name) => 
                            this.db.Users.Join(this.db.Candidates, us => us.Id, cd => cd.UserId, (us, cd) => new { us, cd })
                                .Join(this.db.Accelerations, temp0 => temp0.cd.AccelerationId, ac => ac.Id, (temp0, ac) => new { temp0, ac })
                                .Where(temp1 => (temp1.ac.Name == name)).Select(temp1 => temp1.temp0.us).OrderBy(a => a.Id).Distinct().ToList();
        

        public IList<User> FindByCompanyId(int companyId) =>
                            this.db.Users.Join(this.db.Candidates, us => us.Id, cd => cd.UserId, (us, cd) => new { us, cd })
                                .Join(this.db.Companies, temp0 => temp0.cd.CompanyId, cp => cp.Id, (temp0, cp) => new { temp0, cp })
                                .Where(temp1 => (temp1.cp.Id == companyId))
                                .Select(temp1 => temp1.temp0.us).Distinct().ToList();
        

        public User FindById(int id) => this.db.Users.Find(id);
        
        public User Save(User user)
        {
            if (user is null)
            {
                throw new Exception("Usuario invalido!");
            }

            if (user.Id == 0)
            {
                this.db.Add(user);
            }
            else
            {
                this.db.Update(user);

            }
            this.db.SaveChanges();

            return user;
        }
    }
}