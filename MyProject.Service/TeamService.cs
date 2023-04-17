using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using MyProject.Service.Helpers;
using MyProject.Service.Helpers.Encryption;
using MyProject.Service.Helpers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository teamrepository;
        private readonly IUnitOfWork _unitOfWork;

        public TeamService(ITeamRepository teamrepository, IUnitOfWork unitOfWork)
        {
            this.teamrepository = teamrepository;
            this._unitOfWork = unitOfWork;
        }

        #region ITeamService Members

        public IEnumerable<Team> GetTeams()
        {
            var users = teamrepository.GetAll().Where(i => i.IsDeleted == false);
            return users;
        }

        public Team GetTeam(long id)
        {
            var Team = teamrepository.GetById(id);
            return Team;
        }

        public bool CreateTeam(Team Team, ref string message, bool sendMail = false)
        {
            try
            {


                Team.IsActive = true;
                Team.IsDeleted = false;
                Team.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                teamrepository.Add(Team);
                if (Save())
                {
                    message = "WorkForce added successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool UpdateTeam(ref Team Team, ref string message)
        {
            try
            {

                Team CurrentTeam = teamrepository.GetById(Team.ID);

                CurrentTeam.Name = Team.Name;
                CurrentTeam.NameAr = Team.NameAr;
                if (!string.IsNullOrEmpty(Team.Image))
                {
                    CurrentTeam.Image = Team.Image;
                }
                CurrentTeam.Designation = Team.Designation;
                CurrentTeam.DesignationAr = Team.DesignationAr;
                CurrentTeam.Address = Team.Address;
                CurrentTeam.Contact1 = Team.Contact1;
                CurrentTeam.Contact2 = Team.Contact2;
                CurrentTeam.About = Team.About;
                CurrentTeam.AboutAr = Team.AboutAr;
                CurrentTeam.Email = Team.Email;



                teamrepository.Update(CurrentTeam);
                if (Save())
                {
                    Team = CurrentTeam;
                    message = "WorkForce updated successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }

            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool DeleteTeam(long id, ref string message, bool softDelete = true)
        {
            try
            {
                Team Team = teamrepository.GetById(id);

                if (softDelete)
                {
                    Team.IsDeleted = true;
                    teamrepository.Update(Team);
                }
                else
                {
                    teamrepository.Delete(Team);
                }

                if (Save())
                {
                    message = "WorkForce deleted successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool Save()
        {
            try
            {
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }

    public interface ITeamService
    {
        IEnumerable<Team> GetTeams();
        Team GetTeam(long id);
        bool CreateTeam(Team team, ref string message, bool sendMail = false);
        bool UpdateTeam(ref Team team, ref string message);
        bool DeleteTeam(long id, ref string message, bool softDelete = true);
        bool Save();
    }
}
