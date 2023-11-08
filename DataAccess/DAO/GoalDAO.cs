using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class GoalDAO
    {
        FinalProPrn231Context _context;

        public GoalDAO(FinalProPrn231Context context)
        {
            _context = context;
        }

        public List<Goal> GetAll()
        {
            return _context.Goals.ToList();
        }

        public List<Goal> GetGoalsInAMatch(int matchId)
        {
            return _context.Goals.Include(p => p.Player).Where(p => p.MatchId == matchId).ToList();
        }

        public void Add(Goal goal)
        {
            _context.Goals.Add(goal);
            _context.SaveChanges();
        }
    }
}
