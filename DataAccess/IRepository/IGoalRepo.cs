using BussinessObject.Models;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IGoalRepo
    {
        List<GoalDTO> GetAll();
        List<GoalDTO> GetGoalsInAMatch(int matchId);
        void Add(GoalDTO goal);
    }
}
