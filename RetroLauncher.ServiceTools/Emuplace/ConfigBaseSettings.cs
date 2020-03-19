using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace RetroLauncher.ServiceTools.Emuplace
{
    public enum special
    {
        none,
        hq2x,
        hq3x,
        hq4x,
        scale2x,
        scale3x,
        scale4x,
        super2xsai,
        supereagle,
        nn2x,
        nn3x,
        nn4x,
        nny2x,
        nny3x,
        nny4x
    }

    public enum shader
    {
        none,
        autoip,
        autoipsharper,
        scale2x,
        sabr,
        ipsharper,
        ipxnoty,
        ipynotx,
        ipxnotysharper,
        ipynotxsharper,
        goat
    }

    public enum stretch
    {
        none,
        full,
        aspect,
        aspect_int,
        aspect_mult2
    }

    public enum nes__ntsc__preset
    {
        none,
        disabled,
        composite,
        svideo,
        rgb,
        monochrome
    }

    public enum gb_system_type
    {
        auto,
        dmg,
        cgb,
        agb
    }

    public class ConfigBaseSettings
    {

        #region Properties
        /// <summary>
        /// Автоматическая загр./сохр. при загр./закр.  игры
        /// </summary>
        [DescriptionProperty("Автоматическая загр./сохр. при загр./закр. игры")]
        public bool? autosave { get; set; }                        // 

        [DescriptionProperty("Частота автоматического нажатия")]
        public int? input__autofirefreq { get; set; }                // частота автоматического огня (0 through 1000)      

        //ЗВУК      
        [DescriptionProperty("Включить вывод звука")]
        public bool? sound { get; set; }
        //  public int? sound__buffer_time { get; set; }                // Требуемый размер буфера в миллисекундах (0 to 1000)
        //  public string sound__device { get; set; }                   // устройство вывода звука (default)
        //  public string sound__driver { get; set; }                   // аудио-драйвер (default alsa oss wasapish dsound wasapi sdl jack)
        //  public int? sound__period_time { get; set; }                 // размер периода в микросекундах (мкс). 0 to 100000
        [DescriptionProperty("Частота дискретизации звука")]
        public int? sound__rate { get; set; }                        // 22050 to 192000

        [DescriptionProperty("Громкость звука")]
        public int? sound__volume { get; set; }                      // 0 to 150

        //ВИДЕО
        [DescriptionProperty("Синхронизация времени (ожидания) бликования кадров.")]
        public bool? video__blit_timesync { get; set; }             // Синхронизация времени (ожидание) для бликования кадров.

        [DescriptionProperty("Деинтерлейсер, для использования чересстрочного видео")]
        public string video__deinterlacer { get; set; }             // Деинтерлейсер, чтобы использовать для чересстрочного видео (weave bob bob_offset blend blend_rg)        

        [DescriptionProperty("Попытка отключить композицию на рабочем столе")]
        public bool? video__disable_composition { get; set; }       // Попытка отключить композицию на рабочем столе                                                                    

        [DescriptionProperty("Пропуск кадров")]
        public bool? video__frameskip { get; set; }                 // Пропуск кадров

        [DescriptionProperty("Полноэкранный режим")]
        public bool? video__fs { get; set; }                        // Полноэкранный режим

        [DescriptionProperty("Cинхронизировать смену видео-страниц OpenGL.")]
        public bool? video__glvsync { get; set; }                   // Пытаться синхронизировать смену видео-страниц OpenGL с частотой вертикальной развёртки.


        //NES           

        [DescriptionProperty("Предустановки качество видео/тип")]
        public nes__ntsc__preset nes__ntsc__preset { get; set; }               // Предустановки качество видео/тип (disabled composite svideo rgb monochrome)     

        [DescriptionProperty("Включить эффект Scanline")]
        public bool? nes__shader__goat__slen { get; set; }          // Включить эффект Scanline

        [DescriptionProperty("Прозрачность Scanlines")]
        public int? nes__scanlines { get; set; }

        [DescriptionProperty("Шейдер OpenGL")]
        public shader nes__shader { get; set; }                     // шейдер OpenGL (none autoip autoipsharper scale2x sabr ipsharper ipxnoty ipynotx ipxnotysharper ipynotxsharper goat)

        [DescriptionProperty("Использовать указанный метод масштабирования видео")]
        public special nes__special { get; set; }                    // Использовать указанный метод масштабирования видео (none hq2x hq3x hq4x scale2x scale3x scale4x 2xsai super2xsai supereagle nn2x nn3x nn4x nny2x nny3x nny4x)

        [DescriptionProperty("Растянуть на весь экран")]
        public stretch nes__stretch { get; set; }

        [DescriptionProperty("Корректировать соотношение сторон")]
        public bool? nes__correct_aspect { get; set; }

        // Прозрачность Scanlines        

        [DescriptionProperty("Включите временное размытие видео")]
        public bool? nes__tblur { get; set; }                       // Включите временное размытие видео

        [DescriptionProperty("Включить (би)линейную интерполяцию")]
        public bool? nes__videoip { get; set; }                      // Включить (bi) линейную интерполяцию (0 1 x y)        

        [DescriptionProperty("Использование чересстрочной развертки как прогрессивной.")]
        public bool? nes__shader__goat__fprog { get; set; }


        //NES GamePad
        [DescriptionProperty("A")]
        public string nes__input__port1__gamepad__a { get; set; }       //nes, Port 1, Gamepad: A   (keyboard 0x0 91)

        [DescriptionProperty("B")]
        public string nes__input__port1__gamepad__b { get; set; }       //nes, Port 1, Gamepad: B   (keyboard 0x0 90)

        [DescriptionProperty("Rapid A")]
        public string nes__input__port1__gamepad__rapid_a { get; set; } //nes, Port 1, Gamepad: Rapid A

        [DescriptionProperty("Rapid B")]
        public string nes__input__port1__gamepad__rapid_b { get; set; } //nes, Port 1, Gamepad: Rapid B        

        [DescriptionProperty("SELECT")]
        public string nes__input__port1__gamepad__select { get; set; }  //nes, Port 1, Gamepad: SELECT  (keyboard 0x0 43)

        [DescriptionProperty("START")]
        public string nes__input__port1__gamepad__start { get; set; }   //nes, Port 1, Gamepad: START   (keyboard 0x0 40)

        [DescriptionProperty("Вниз ↓")]
        public string nes__input__port1__gamepad__down { get; set; }    //nes, Port 1, Gamepad: DOWN ↓  (keyboard 0x0 22)

        [DescriptionProperty("Влево ←")]
        public string nes__input__port1__gamepad__left { get; set; }    //nes, Port 1, Gamepad: LEFT ←  (keyboard 0x0 4)

        [DescriptionProperty("Вверх ↑")]
        public string nes__input__port1__gamepad__up { get; set; } 	    //nes, Port 1, Gamepad: UP ↑    (keyboard 0x0 26)

        [DescriptionProperty("Вправо → ")]
        public string nes__input__port1__gamepad__right { get; set; }   //nes, Port 1, Gamepad: RIGHT → (keyboard 0x0 7)        




        // Sega Master System       
        [DescriptionProperty("Принудительно использовать монофонический вывод звука")]
        public bool? sms__forcemono { get; set; }

        [DescriptionProperty("Включить эффект Scanline")]
        public bool? sms__shader__goat__slen { get; set; }          // Включить эффект Scanline

        [DescriptionProperty("Прозрачность Scanlines")]
        public int? sms__scanlines { get; set; }                    // Прозрачность Scanlines

        [DescriptionProperty("Шейдер OpenGL")]
        public shader sms__shader { get; set; }

        [DescriptionProperty("Использовать указанный метод масштабирования видео")]
        public special sms__special { get; set; }

        [DescriptionProperty("Растянуть на весь экран")]
        public stretch sms__stretch { get; set; }

        [DescriptionProperty("Корректировать соотношение сторон")]
        public bool? sms__correct_aspect { get; set; }

        [DescriptionProperty("Включите временное размытие видео")]
        public bool? sms__tblur { get; set; }

        [DescriptionProperty("Включить (би)линейную интерполяцию")]
        public bool? sms__videoip { get; set; }


        //SMS Gamepad
        [DescriptionProperty("Pause")]
        public string sms__input__port1__gamepad__rapid_pause { get; set; }

        [DescriptionProperty("I (start)")]
        public string sms__input__port1__gamepad__fire1 { get; set; }

        [DescriptionProperty("II")]
        public string sms__input__port1__gamepad__fire2 { get; set; }

        [DescriptionProperty("Rapid I")]
        public string sms__input__port1__gamepad__rapid_fire1 { get; set; }

        [DescriptionProperty("Rapid II")]
        public string sms__input__port1__gamepad__rapid_fire2 { get; set; }

        [DescriptionProperty("Вниз ↓")]
        public string sms__input__port1__gamepad__down { get; set; }

        [DescriptionProperty("Влево ←")]
        public string sms__input__port1__gamepad__left { get; set; }

        [DescriptionProperty("Вверх ↑")]
        public string sms__input__port1__gamepad__up { get; set; }

        [DescriptionProperty("Вправо → ")]
        public string sms__input__port1__gamepad__right { get; set; }


        //Sega MegaDrive
        [DescriptionProperty("Принудительно использовать монофонический вывод звука")]
        public bool? md__forcemono { get; set; }

        [DescriptionProperty("Включить эффект Scanline")]
        public bool? md__shader__goat__slen { get; set; }          // Включить эффект Scanline

        [DescriptionProperty("Прозрачность Scanlines")]
        public int? md__scanlines { get; set; }                    // Прозрачность Scanlines

        [DescriptionProperty("Шейдер OpenGL")]
        public shader md__shader { get; set; }

        [DescriptionProperty("Использовать указанный метод масштабирования видео")]
        public special md__special { get; set; }

        [DescriptionProperty("Растянуть на весь экран")]
        public stretch md__stretch { get; set; }

        [DescriptionProperty("Корректировать соотношение сторон")]
        public bool? md__correct_aspect { get; set; }

        [DescriptionProperty("Включите временное размытие видео")]
        public bool? md__tblur { get; set; }

        [DescriptionProperty("Включить (би)линейную интерполяцию")]
        public bool? md__videoip { get; set; }


        private string md__input__port1 { get; set; } = "gamepad";


        //Sega Megadrive GamePad
        [DescriptionProperty("A")]
        public string md__input__port1__gamepad__a { get; set; }       //nes, Port 1, Gamepad: A   (keyboard 0x0 91)

        [DescriptionProperty("B")]
        public string md__input__port1__gamepad__b { get; set; }       //nes, Port 1, Gamepad: B   (keyboard 0x0 90)

        [DescriptionProperty("C")]
        public string md__input__port1__gamepad__c { get; set; } //nes, Port 1, Gamepad: Rapid A

        [DescriptionProperty("START")]
        public string md__input__port1__gamepad__start { get; set; }   //nes, Port 1, Gamepad: START   (keyboard 0x0 40)

        [DescriptionProperty("Вниз ↓")]
        public string md__input__port1__gamepad__down { get; set; }    //nes, Port 1, Gamepad: DOWN ↓  (keyboard 0x0 22)

        [DescriptionProperty("Влево ←")]
        public string md__input__port1__gamepad__left { get; set; }    //nes, Port 1, Gamepad: LEFT ←  (keyboard 0x0 4)

        [DescriptionProperty("Вверх ↑")]
        public string md__input__port1__gamepad__up { get; set; } 	    //nes, Port 1, Gamepad: UP ↑    (keyboard 0x0 26)

        [DescriptionProperty("Вправо → ")]
        public string md__input__port1__gamepad__right { get; set; }   //nes, Port 1, Gamepad: RIGHT → (keyboard 0x0 7)        


        //SNES
        [DescriptionProperty("APU выходного качества ресэмплера.")]
        public int? snes__apu__resamp_quality { get; set; }

        [DescriptionProperty("Включить горизонтальный фильтр наложения(размытия).")]
        public int? snes__h_blend { get; set; }

        [DescriptionProperty("Принудительно использовать монофонический вывод звука")]
        public bool? snes__forcemono { get; set; }

        [DescriptionProperty("Включить эффект Scanline")]
        public bool? snes__shader__goat__slen { get; set; }          // Включить эффект Scanline

        [DescriptionProperty("Прозрачность Scanlines")]
        public int? snes__scanlines { get; set; }                    // Прозрачность Scanlines

        [DescriptionProperty("Шейдер OpenGL")]
        public shader snes__shader { get; set; }

        [DescriptionProperty("Использовать указанный метод масштабирования видео")]
        public special snes__special { get; set; }

        [DescriptionProperty("Растянуть на весь экран")]
        public stretch snes__stretch { get; set; }

        [DescriptionProperty("Корректировать соотношение сторон")]
        public bool? snes__correct_aspect { get; set; }

        [DescriptionProperty("Включите временное размытие видео")]
        public bool? snes__tblur { get; set; }

        [DescriptionProperty("Включить (би)линейную интерполяцию")]
        public bool? snes__videoip { get; set; }


        //SNES GamePad
        [DescriptionProperty("A")]
        public string snes__input__port1__gamepad__a { get; set; }       //nes, Port 1, Gamepad: A   (keyboard 0x0 91)

        [DescriptionProperty("B")]
        public string snes__input__port1__gamepad__b { get; set; }       //nes, Port 1, Gamepad: B   (keyboard 0x0 90)

        [DescriptionProperty("X")]
        public string snes__input__port1__gamepad__x { get; set; } //nes, Port 1, Gamepad: Rapid A

        [DescriptionProperty("Y")]
        public string snes__input__port1__gamepad__y { get; set; } //nes, Port 1, Gamepad: Rapid B        

        [DescriptionProperty("L")]
        public string snes__input__port1__gamepad__l { get; set; }

        [DescriptionProperty("R")]
        public string snes__input__port1__gamepad__r { get; set; }

        [DescriptionProperty("SELECT")]
        public string snes__input__port1__gamepad__select { get; set; }  //nes, Port 1, Gamepad: SELECT  (keyboard 0x0 43)

        [DescriptionProperty("START")]
        public string snes__input__port1__gamepad__start { get; set; }   //nes, Port 1, Gamepad: START   (keyboard 0x0 40)

        [DescriptionProperty("Вниз ↓")]
        public string snes__input__port1__gamepad__down { get; set; }    //nes, Port 1, Gamepad: DOWN ↓  (keyboard 0x0 22)

        [DescriptionProperty("Влево ←")]
        public string snes__input__port1__gamepad__left { get; set; }    //nes, Port 1, Gamepad: LEFT ←  (keyboard 0x0 4)

        [DescriptionProperty("Вверх ↑")]
        public string snes__input__port1__gamepad__up { get; set; } 	    //nes, Port 1, Gamepad: UP ↑    (keyboard 0x0 26)

        [DescriptionProperty("Вправо → ")]
        public string snes__input__port1__gamepad__right { get; set; }   //nes, Port 1, Gamepad: RIGHT → (keyboard 0x0 7)        


        //GameBoy      
        [DescriptionProperty("Тип эмуляции GameBoy")]
        public gb_system_type gb__system_type { get; set; }

        [DescriptionProperty("Принудительно использовать монофонический вывод звука")]
        public bool? gb__forcemono { get; set; }

        [DescriptionProperty("Включить эффект Scanline")]
        public bool? gb__shader__goat__slen { get; set; }          // Включить эффект Scanline

        [DescriptionProperty("Прозрачность Scanlines")]
        public int? gb__scanlines { get; set; }                    // Прозрачность Scanlines

        [DescriptionProperty("Шейдер OpenGL")]
        public shader gb__shader { get; set; }

        [DescriptionProperty("Использовать указанный метод масштабирования видео")]
        public special gb__special { get; set; }

        [DescriptionProperty("Растянуть на весь экран")]
        public stretch gb__stretch { get; set; }

        [DescriptionProperty("Корректировать соотношение сторон")]
        public bool? gb__correct_aspect { get; set; }

        [DescriptionProperty("Включите временное размытие видео")]
        public bool? gb__tblur { get; set; }

        [DescriptionProperty("Включить (би)линейную интерполяцию")]
        public bool? gb__videoip { get; set; }


        //SNES GamePad
        [DescriptionProperty("A")]
        public string gb__input__builtin__gamepad__a { get; set; }

        [DescriptionProperty("B")]
        public string gb__input__builtin__gamepad__b { get; set; }

        [DescriptionProperty("Rapid A")]
        public string gb__input__builtin__gamepad__rapid_a { get; set; }

        [DescriptionProperty("Rapid B")]
        public string gb__input__builtin__gamepad__rapid_b { get; set; }

        [DescriptionProperty("SELECT")]
        public string gb__input__builtin__gamepad__select { get; set; }  //nes, Port 1, Gamepad: SELECT  (keyboard 0x0 43)

        [DescriptionProperty("START")]
        public string gb__input__builtin__gamepad__start { get; set; }   //nes, Port 1, Gamepad: START   (keyboard 0x0 40)

        [DescriptionProperty("Вниз ↓")]
        public string gb__input__builtin__gamepad__down { get; set; }    //nes, Port 1, Gamepad: DOWN ↓  (keyboard 0x0 22)

        [DescriptionProperty("Влево ←")]
        public string gb__input__builtin__gamepad__left { get; set; }    //nes, Port 1, Gamepad: LEFT ←  (keyboard 0x0 4)

        [DescriptionProperty("Вверх ↑")]
        public string gb__input__builtin__gamepad__up { get; set; } 	    //nes, Port 1, Gamepad: UP ↑    (keyboard 0x0 26)

        [DescriptionProperty("Вправо → ")]
        public string gb__input__builtin__gamepad__right { get; set; }   //nes, Port 1, Gamepad: RIGHT → (keyboard 0x0 7)        

        #endregion



        #region Methods

        public ConfigBaseSettings()
        {
            var dict = new Dictionary<Type, Action<PropertyInfo>>
            {
                {  typeof(bool), new Action<PropertyInfo>((p) => p.SetValue(this, ConvertBool(Parser.GetValue(p.Name.Replace("__","."))))) },
                {  typeof(bool?), new Action<PropertyInfo>((p) => p.SetValue(this, ConvertBool(Parser.GetValue(p.Name.Replace("__","."))))) },
                {  typeof(double), new Action<PropertyInfo>((p) => p.SetValue(this, ConvertDouble(Parser.GetValue(p.Name.Replace("__","."))))) },
                {  typeof(double?), new Action<PropertyInfo>((p) => p.SetValue(this, ConvertDouble(Parser.GetValue(p.Name.Replace("__","."))))) },
                {  typeof(int), new Action<PropertyInfo>((p) => p.SetValue(this, ConvertInt(Parser.GetValue(p.Name.Replace("__","."))))) },
                {  typeof(int?), new Action<PropertyInfo>((p) => p.SetValue(this, ConvertInt(Parser.GetValue(p.Name.Replace("__","."))))) },
                {  typeof(string), new Action<PropertyInfo>((p) => p.SetValue(this, ConvertString(Parser.GetValue(p.Name.Replace("__","."))))) }
            };

            foreach (var prop in typeof(ConfigBaseSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.PropertyType.IsEnum)
                {
                    var values = Parser.GetValue(prop.Name.Replace("__", "."));
                    if (values != null)
                        prop.SetValue(this, Enum.Parse(prop.PropertyType, ConvertString(values)));
                }
                else if (dict.TryGetValue(prop.PropertyType, out var action))
                {
                    action(prop);
                }
            }
            int i = 0;
        }



        public void Save()
        {
            foreach (var prop in typeof(ConfigBaseSettings).GetProperties())
            {
                var propValue = prop.GetValue(this);
                string[] result = null; ;

                if (prop.Name.Contains("__input__") && !string.IsNullOrEmpty(propValue.ToString()))
                {
                    var key = (Key)Enum.Parse(typeof(Key), propValue.ToString(), true);
                    result = new string[] { "keyboard 0x0 " + Keyassign.VKeyToSdlKey(KeyInterop.VirtualKeyFromKey(key)).ToString() };

                }
                else if (propValue.GetType() == typeof(bool) || propValue.GetType() == typeof(bool?))
                    result = ConvertBool((bool)propValue);
                else if (propValue.GetType() == typeof(double) || propValue.GetType() == typeof(double?))
                    result = ConvertDouble((double)propValue);
                else if (propValue.GetType() == typeof(int) || propValue.GetType() == typeof(int?))
                    result = ConvertInt((int)propValue);
                else if (propValue.GetType() == typeof(string))
                    result = ConvertString((string)propValue);
                else result = ConvertString(propValue.ToString());

                Parser.SetValue(prop.Name.Replace("__", "."), result);
            }
            Parser.SaveConfig();
        }

        #endregion

        #region Converter
        private int? ConvertInt(string[] values)
        {
            if (values != null)
            {
                if (string.IsNullOrEmpty(values[0])) return null;
                return Int32.Parse(values[0]);
            }
            return 0;
        }

        private string[] ConvertInt(int? value)
        {
            if (value != null)
            {
                return new string[] { value.ToString() };
            }
            return null;
        }

        private double? ConvertDouble(string[] values)
        {
            if (values != null)
            {
                if (string.IsNullOrEmpty(values[0])) return null;
                return Double.Parse(values[0], System.Globalization.CultureInfo.InvariantCulture);
            }
            return 0;
        }

        private string[] ConvertDouble(double? value)
        {
            if (value != null)
            {
                return new string[] { value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) };
            }
            return null;
        }

        private bool? ConvertBool(string[] values)
        {
            if (values != null)
            {
                if (string.IsNullOrEmpty(values[0])) return null;
                return values[0] == "1" ? true : false;
            }
            return false;
        }

        private string[] ConvertBool(bool? value)
        {
            if (value != null)
            {
                if (value.Value)
                    return new string[] { "1" };
                return new string[] { "0" };
            }
            return new string[] { "0" };
        }

        private string ConvertString(string[] values)
        {
            if (values != null)
            {
                if (string.IsNullOrEmpty(values[0])) return null;
                return values[0];
            }
            return null;
        }

        private string[] ConvertString(string value)
        {
            if (value != null)
            {
                return new string[] { value };
            }
            return null;
        }

        public string KeyCharToCode(string key)
        {
            switch (key)
            {
                case "A": return "";

                default: return "";
            }
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    class DescriptionProperty : Attribute
    {
        public string Value { get; private set; }

        public DescriptionProperty(string value)
        {
            Value = value;
        }
    }

    public class MaxValue : Attribute
    {
        public int Max;

        public MaxValue(int max)
        {
            Max = max;
        }
    }

}
