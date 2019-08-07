using System;
using System.Collections.Generic;
using System.Linq;

namespace RomsDownloader.BaseParser
{
    public class BaseSettings : IParserSettings
    {
        /// <summary>
        /// Сайт с ромами игры
        /// </summary>
        public string BaseUrl { get; set; } = @"http://emu-russia.net/ru/roms/";

        /// <summary>
        /// Список префиксов к сайту для разных платформ
        /// </summary>
        public string[] Prefix { get; set; } =
        {
            "nes/0-Z/full/" ,    //Dendy / Nes
            "snes/0-Z/full/" ,  //Super Nes
            "sms/0-Z/full/" ,    //Sega Master System
            "gen/0-Z/full/" ,    //Sega MegaDrive/Сега 
            "tg16/0-Z/full/" ,  //TurboGrafx-16 
            "gbc/0-Z/full/"      //GameBoy Color (GBC)
        };

        public int CurrentId { get; set; } = 0;

    }

}
