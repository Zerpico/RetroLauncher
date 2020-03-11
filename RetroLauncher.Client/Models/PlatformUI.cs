using RetroLauncher.DAL.Model;

namespace RetroLauncher.Client.Models
{
    public class PlatformUI : Platform
    {
        public PlatformUI (Platform plat )
        {
            PlatformId = plat.PlatformId;
            PlatformName = plat.PlatformName;
            Alias = plat.Alias;
            //specialName = (obj is Ferrari) ? ((Ferrari)obj).SpecialName : "";
        }

        public override string ToString()
        {
            return $"{PlatformName}\t{Alias}\t{Icon}";
        }

        public string Icon
        {
            get
            {
                var dir = System.AppDomain.CurrentDomain.BaseDirectory;
                switch (Alias)
                {
                    case "nes":
                        return System.IO.Path.Combine(dir, "icons\\nintendo_nes.png");
                    case "sms":
                        return System.IO.Path.Combine(dir, "icons\\sega_master_system.png");
                    case "gen":
                        return System.IO.Path.Combine(dir, "icons\\sega_genesis.png");
                    case "snes":
                        return System.IO.Path.Combine(dir, "icons\\nintendo_supernes.png");
                    case "gbc":
                        return System.IO.Path.Combine(dir, "icons\\nintendo_game_boy_pocket.png");
                    default:
                        return System.IO.Path.Combine(dir, "icons\\nintendo_nes.png");
                }
            }
        }
    }
}
