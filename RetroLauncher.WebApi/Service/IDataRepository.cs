using RetroLauncher.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroLauncher.WebApi.Service
{
    public interface IDataRepository
    {

        PagingGames GetGames(string name, int[] genres, int[] platforms, int limit = 50, int offset = 0);

        DAL.Model.Game GetGameById(int id);

        List<DAL.Model.Genre> GetGenres();

        List<DAL.Model.Platform> GetPlatforms();
    }
}
