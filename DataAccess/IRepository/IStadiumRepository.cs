using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IStadiumRepository
    {
        List<StadiumDTO> GetStadiums();
    }
}
