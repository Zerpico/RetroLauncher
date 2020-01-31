using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetroLauncher.DAL;
using RetroLauncher.DAL.Model;

namespace RetroLauncher.DesktopClient.Service
{
    public class FileService //: IFileService
    {

        public FileService()
        {

        }

        public async void UpdateGame(Game game)
        {
            using (var db = new LocalGameContext())
            {
                if(db.Set<Game>().Any( a => a.GameId == game.GameId))
                {
                    db.Games.Update(game);
                }
                else
                {
                    db.Games.Add(game);
                }
                await db.SaveChangesAsync();
            }
        }

        public async Task<Game> GetGame(Game game)
        {
            using (var db = new LocalGameContext())
            {
                if( await db.Set<GamePath>().AnyAsync( a => a.GameId == game.GameId))
                {
                    var find = db.GamePaths.Where(d => d.GameId == game.GameId).First();

                    game.LocalPath = find;
                    return game;
                }

            }
            return game;
        }
    }

    
}