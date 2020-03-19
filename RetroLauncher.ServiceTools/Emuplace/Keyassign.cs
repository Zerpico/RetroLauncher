using System;
using System.Collections.Generic;
using System.Text;

namespace RetroLauncher.ServiceTools.Emuplace
{
    public class Keyassign
    {
        public string Keys { get; set; }
        public int SDLKey { get; set; }
        public int VKey { get; set; }

        public static int VKeyToSdlKey(int vkey)
        {
            int sdlKey = 0;
            switch (vkey)
            {
                case 27:    //Escape
                    sdlKey = 41;
                    break;
                case 112:   //F1
                    sdlKey = 58;
                    break;
                case 113:   //F2
                    sdlKey = 59;
                    break;
                case 114:   //F3
                    sdlKey = 60;
                    break;
                case 115:   //F4
                    sdlKey = 61;
                    break;
                case 116:   //F5
                    sdlKey = 62;
                    break;
                case 117:   //F6
                    sdlKey = 63;
                    break;
                case 118:   //F7
                    sdlKey = 64;
                    break;
                case 119:   //F8
                    sdlKey = 65;
                    break;
                case 120:   //F9
                    sdlKey = 66;
                    break;
                case 121:   //F10
                    sdlKey = 67;
                    break;
                case 122:   //F11
                    sdlKey = 68;
                    break;
                case 123:   //F12
                    sdlKey = 69;
                    break;
                case 49:    //D1
                    sdlKey = 30;
                    break;
                case 50:    //D2
                    sdlKey = 31;
                    break;
                case 51:    //D3
                    sdlKey = 32;
                    break;
                case 52:    //D4
                    sdlKey = 33;
                    break;
                case 53:    //D5
                    sdlKey = 34;
                    break;
                case 54:    //D6
                    sdlKey = 35;
                    break;
                case 55:    //D7
                    sdlKey = 36;
                    break;
                case 56:    //D8
                    sdlKey = 37;
                    break;
                case 57:    //D9
                    sdlKey = 38;
                    break;
                case 48:    //D0
                    sdlKey = 39;
                    break;
                case 81:    //Q
                    sdlKey = 20;
                    break;
                case 87://W
                    sdlKey = 26;
                    break;
                case 69://E
                    sdlKey = 8;
                    break;
                case 82://R
                    sdlKey = 21;
                    break;
                case 84://T
                    sdlKey = 23;
                    break;
                case 89: //Y
                    sdlKey = 28;
                    break;
                case 85://U
                    sdlKey = 24;
                    break;
                case 73://I
                    sdlKey = 12;
                    break;
                case 79://O
                    sdlKey = 18;
                    break;
                case 80://P
                    sdlKey = 19;
                    break;
                case 65: //A
                    sdlKey = 4;
                    break;
                case 83: //S
                    sdlKey = 22;
                    break;
                case 68://D
                    sdlKey = 7;
                    break;
                case 70://F
                    sdlKey = 9;
                    break;
                case 71://G
                    sdlKey = 10;
                    break;
                case 72://H
                    sdlKey = 11;
                    break;
                case 74://J
                    sdlKey = 13;
                    break;
                case 75://K
                    sdlKey = 14;
                    break;
                case 76: //L
                    sdlKey = 15;
                    break;
                case 90://Z
                    sdlKey = 29;
                    break;
                case 88://X
                    sdlKey = 27;
                    break;
                case 67://C
                    sdlKey = 6;
                    break;
                case 86: //V
                    sdlKey = 25;
                    break;
                case 66://B
                    sdlKey = 5;
                    break;
                case 78://N
                    sdlKey = 17;
                    break;
                case 77://M
                    sdlKey = 16;
                    break;
                case 220: //Backslash
                    sdlKey = 49;
                    break;
                case 223://Unused/Generic
                    sdlKey = 96;
                    break;
                case 8://Back
                    sdlKey = 42;
                    break;
                case 9: //Tab
                    sdlKey = 43;
                    break;
                case 13://Return
                    sdlKey = 40;
                    break;
                case 20://Capital
                    sdlKey = 57;
                    break;
                case 219: //[
                    sdlKey = 47;
                    break;
                case 221://]
                    sdlKey = 48;
                    break;
                case 186://;
                    sdlKey = 51;
                    break;
                case 192://tilde
                    sdlKey = 100;
                    break;
                case 222:////
                    sdlKey = 52;
                    break;
                case 16://ShiftKey
                    sdlKey = 225;
                    break;
                case 226://Bracket
                    sdlKey = 47;
                    break;
                case 188://comma
                    sdlKey = 54;
                    break;
                case 190: //Period
                    sdlKey = 55;
                    break;
                case 17://Question
                    sdlKey = 224;
                    break;
                case 91: //LWin
                    sdlKey = 227;
                    break;
                case 18://Alt
                    sdlKey = 226;
                    break;
                case 32://Space
                    sdlKey = 44;
                    break;
                case 92: //RWin
                    sdlKey = 231;
                    break;
                case 93: //Apps
                    sdlKey = 101;
                    break;
                case 44: //PrintScreen
                    sdlKey = 70;
                    break;
                case 145://Scroll
                    sdlKey = 71;
                    break;
                case 19://Pause
                    sdlKey = 72;
                    break;
                case 45://Insert
                    sdlKey = 73;
                    break;
                case 36://Home
                    sdlKey = 74;
                    break;
                case 33://PageUp
                    sdlKey = 75;
                    break;
                case 34://PageDown
                    sdlKey = 78;
                    break;
                case 35://End
                    sdlKey = 77;
                    break;
                case 46://Delete
                    sdlKey = 76;
                    break;
                case 38://Up
                    sdlKey = 82;
                    break;
                case 40://Down
                    sdlKey = 81;
                    break;
                case 37://Left
                    sdlKey = 80;
                    break;
                case 39: //Right
                    sdlKey = 79;
                    break;
                case 181://SelectMedia
                    sdlKey = 263;
                    break;
                case 173://VolumeMute
                    sdlKey = 262;
                    break;
                case 174://VolumeDown
                    sdlKey = 129;
                    break;
                case 175://VolumeUp
                    sdlKey = 128;
                    break;
                case 178://MediaStop
                    sdlKey = 260;
                    break;
                case 177://MediaPreviousTrack
                    sdlKey = 259;
                    break;
                case 179://MediaPlayPause
                    sdlKey = 261;
                    break;
                case 176://MediaNextTrack
                    sdlKey = 258;
                    break;
                case 180://LaunchMail
                    sdlKey = 265;
                    break;
                case 172://BrowserHome
                    sdlKey = 264;
                    break;
                case 144://LaunchApplication2
                    sdlKey = 83;
                    break;
                case 96://KP_0
                    sdlKey = 98;
                    break;
                case 97://KP_1
                    sdlKey = 89;
                    break;
                case 98://KP_2
                    sdlKey = 90;
                    break;
                case 99://KP_3
                    sdlKey = 91;
                    break;
                case 100: //KP_4
                    sdlKey = 92;
                    break;
                case 101://KP_5
                    sdlKey = 93;
                    break;
                case 102://KP_6
                    sdlKey = 94;
                    break;
                case 103: //KP_7
                    sdlKey = 95;
                    break;
                case 104:   //KP_8
                    sdlKey = 96;
                    break;
                case 105:   //KP_9
                    sdlKey = 97;
                    break;
                case 111:   //Divide
                    sdlKey = 84;
                    break;
                case 106:   //Multiply
                    sdlKey = 85;
                    break;
                case 109:   //Subtract
                    sdlKey = 86;
                    break;
                case 107:   //Add
                    sdlKey = 87;
                    break;
                case 110:   //Decimal
                    sdlKey = 220;
                    break;
                case 12:    //Clear   
                    sdlKey = 156;
                    break;
                case 161:    //Shift   
                    sdlKey = 225;
                    break;
                case 162:    //ctrl   
                    sdlKey = 224;
                    break;
                case 163:    //ctrl   
                    sdlKey = 224;
                    break;
            }
            return sdlKey;
        }


        public static int SdlKeyToVKey(int sdlkey)
        {
            int vkey = 0;
            switch (sdlkey)
            {
                case 41:    //Escape
                    vkey = 27;
                    break;
                case 58:   //F1
                    vkey = 112;
                    break;
                case 59:   //F2
                    vkey = 113;
                    break;
                case 60:   //F3
                    vkey = 114;
                    break;
                case 61:   //F4
                    vkey = 115;
                    break;
                case 62:   //F5
                    vkey = 116;
                    break;
                case 63:   //F6
                    vkey = 117;
                    break;
                case 64:   //F7
                    vkey = 118;
                    break;
                case 65:   //F8
                    vkey = 119;
                    break;
                case 66:   //F9
                    vkey = 120;
                    break;
                case 67:   //F10
                    vkey = 121;
                    break;
                case 68:   //F11
                    vkey = 122;
                    break;
                case 69:   //F12
                    vkey = 123;
                    break;
                case 30:    //D1
                    vkey = 49;
                    break;
                case 31:    //D2
                    vkey = 50;
                    break;
                case 32:    //D3
                    vkey = 51;
                    break;
                case 33:    //D4
                    vkey = 52;
                    break;
                case 34:    //D5
                    vkey = 53;
                    break;
                case 35:    //D6
                    vkey = 54;
                    break;
                case 36:    //D7
                    vkey = 55;
                    break;
                case 37:    //D8
                    vkey = 56;
                    break;
                case 38:    //D9
                    vkey = 57;
                    break;
                case 39:    //D0
                    vkey = 48;
                    break;
                case 20:    //Q
                    vkey = 81;
                    break;
                case 26://W
                    vkey = 87;
                    break;
                case 8://E
                    vkey = 69;
                    break;
                case 21://R
                    vkey = 82;
                    break;
                case 23://T
                    vkey = 84;
                    break;
                case 28: //Y
                    vkey = 89;
                    break;
                case 24://U
                    vkey = 85;
                    break;
                case 12://I
                    vkey = 73;
                    break;
                case 18://O
                    vkey = 79;
                    break;
                case 19://P
                    vkey = 80;
                    break;
                case 4: //A
                    vkey = 65;
                    break;
                case 22: //S
                    vkey = 83;
                    break;
                case 7://D
                    vkey = 68;
                    break;
                case 9://F
                    vkey = 70;
                    break;
                case 10://G
                    vkey = 71;
                    break;
                case 11://H
                    vkey = 72;
                    break;
                case 13://J
                    vkey = 74;
                    break;
                case 14://K
                    vkey = 75;
                    break;
                case 15: //L
                    vkey = 76;
                    break;
                case 29://Z
                    vkey = 90;
                    break;
                case 27://X
                    vkey = 88;
                    break;
                case 6://C
                    vkey = 67;
                    break;
                case 25: //V
                    vkey = 86;
                    break;
                case 5://B
                    vkey = 66;
                    break;
                case 17://N
                    vkey = 78;
                    break;
                case 16://M
                    vkey = 77;
                    break;
                case 49: //Backslash
                    vkey = 220;
                    break;
                /*case 96://Unused/Generic
                    vkey = 223;
                    break;*/
                case 42://Back
                    vkey = 8;
                    break;
                case 43: //Tab
                    vkey = 9;
                    break;
                case 40://Return
                    vkey = 13;
                    break;
                case 57://Capital
                    vkey = 20;
                    break;
                case 47: //[
                    vkey = 219;
                    break;
                case 48://]
                    vkey = 221;
                    break;
                case 51://;
                    vkey = 186;
                    break;
                case 100://tilde
                    vkey = 192;
                    break;
                case 52:////
                    vkey = 222;
                    break;
                case 225://ShiftKey
                    vkey = 16;
                    break;
                /*case 47://Bracket
                    vkey = 226;
                    break;*/
                case 54://comma
                    vkey = 188;
                    break;
                case 55: //Period
                    vkey = 190;
                    break;
                case 224://Question
                    vkey = 17;
                    break;
                case 227: //LWin
                    vkey = 91;
                    break;
                case 226://Alt
                    vkey = 18;
                    break;
                case 44://Space
                    vkey = 32;
                    break;
                case 231: //RWin
                    vkey = 92;
                    break;
                case 101: //Apps
                    vkey = 93;
                    break;
                case 70: //PrintScreen
                    vkey = 44;
                    break;
                case 71://Scroll
                    vkey = 145;
                    break;
                case 72://Pause
                    vkey = 19;
                    break;
                case 73://Insert
                    vkey = 45;
                    break;
                case 74://Home
                    vkey = 36;
                    break;
                case 75://PageUp
                    vkey = 33;
                    break;
                case 78://PageDown
                    vkey = 34;
                    break;
                case 77://End
                    vkey = 35;
                    break;
                case 76://Delete
                    vkey = 46;
                    break;
                case 82://Up
                    vkey = 38;
                    break;
                case 81://Down
                    vkey = 40;
                    break;
                case 80://Left
                    vkey = 37;
                    break;
                case 79: //Right
                    vkey = 39;
                    break;
                case 263://SelectMedia
                    vkey = 181;
                    break;
                case 262://VolumeMute
                    vkey = 173;
                    break;
                case 129://VolumeDown
                    vkey = 174;
                    break;
                case 128://VolumeUp
                    vkey = 175;
                    break;
                case 260://MediaStop
                    vkey = 178;
                    break;
                case 259://MediaPreviousTrack
                    vkey = 177;
                    break;
                case 261://MediaPlayPause
                    vkey = 179;
                    break;
                case 258://MediaNextTrack
                    vkey = 176;
                    break;
                case 265://LaunchMail
                    vkey = 180;
                    break;
                case 264://BrowserHome
                    vkey = 172;
                    break;

                case 83://LaunchApplication2
                    vkey = 144;
                    break;
                case 98://KP_0
                    vkey = 96;
                    break;
                case 89://KP_1
                    vkey = 97;
                    break;
                case 90://KP_2
                    vkey = 98;
                    break;
                case 91://KP_3
                    vkey = 99;
                    break;
                case 92: //KP_4
                    vkey = 100;
                    break;
                case 93://KP_5
                    vkey = 101;
                    break;
                case 94://KP_6
                    vkey = 102;
                    break;
                case 95: //KP_7
                    vkey = 103;
                    break;
                case 96:   //KP_8
                    vkey = 104;
                    break;
                case 97:   //KP_9
                    vkey = 105;
                    break;
                case 84:   //Divide
                    vkey = 111;
                    break;
                case 85:   //Multiply
                    vkey = 106;
                    break;
                case 86:   //Subtract
                    vkey = 109;
                    break;
                case 87:   //Add
                    vkey = 107;
                    break;
                case 220:   //Decimal
                    vkey = 110;
                    break;
                case 156:    //Clear   
                    vkey = 12;
                    break;
            }
            return vkey;
        }

    }

}
