using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetroLauncher.DAL;
using RetroLauncher.DAL.Model;

namespace RetroLauncher.DesktopClient.Service
{
    public class FileService //: IFileService
    {
        public static async void UpdateGame(Game game)
        {
            await using var db = new LocalGameContext();

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

        public static async Task<Game> GetGame(Game game)
        {
            await using var db = new LocalGameContext();
            if (!await db.Set<GamePath>().AnyAsync(a => a.GameId == game.GameId))
            {
                return game;
            }

            var find = db.GamePaths.First(d => d.GameId == game.GameId);

            game.LocalPath = find;
            return game;
        }
    }
}