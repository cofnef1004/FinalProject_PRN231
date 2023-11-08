using AutoMapper;
using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class GoalRepo : IGoalRepo
    {
        FinalProPrn231Context _context;
        IMapper mapper;
        GoalDAO goalInMatchDAO;

        public GoalRepo(FinalProPrn231Context context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public void Add(GoalDTO goal)
        {
            goalInMatchDAO = new GoalDAO(_context);
            var goals = mapper.Map<Goal>(goal);
            goalInMatchDAO.Add(goals);
        }

        public List<GoalDTO> GetAll()
        {
            goalInMatchDAO = new GoalDAO(_context);
            List<GoalDTO> goalInMatches = mapper.Map<List<GoalDTO>>(goalInMatchDAO.GetAll());
            return goalInMatches;
        }

        public List<GoalDTO> GetGoalsInAMatch(int matchId)
        {
            goalInMatchDAO = new GoalDAO(_context);
            List<GoalDTO> goals = mapper.Map<List<GoalDTO>>(goalInMatchDAO.GetGoalsInAMatch(matchId));
            return goals;
        }
    }
}
