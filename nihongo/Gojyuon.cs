using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nihongo
{
    public class Gojyuon
    {
        public static readonly string hiragana = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをんっ一";//平假名
        public static readonly string katagana = "アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲンッー";//片假名
        public static readonly string[] duyin1 = new string[] { "a", "i", "u", "e", "o", "ka", "ki", "ku", "ke", "ko", "sa", "shi", "su", "se", "so", "ta", "chi", "tsu", "te", "to", "na", "ni", "nu", "ne", "no", "ha", "hi", "fu", "he", "ho", "ma", "mi", "mu", "me", "mo", "ya", "yu", "yo", "ra", "ri", "ru", "re", "ro", "wa", "wo","n","ci","yi" };

        public static readonly string zhuoyin1 = "がぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷペぽ";
        public static readonly string zhuoyin2 = "ガギグゲゴザジズゼゾダヂヅデドバビブべボパピプペポ";
        public static readonly string[] duyin2 = new string[] { "ga", "gi", "gu", "ge", "go", "za", "ji", "zu", "ze", "zo", "da", "di", "du", "de", "do", "ba", "bi", "bu", "be", "bo", "pa", "pi", "pu", "pe", "po" };

        public static readonly string[] aoyin1 = new string[] { "きゃ", "きゅ", "きょ", "しゃ", "しゅ", "しょ", "ちゃ", "ちゅ", "ちょ", "にゃ", "にゅ", "にょ", "ひゃ", "ひゅ", "ひょ", "みゃ", "みゅ", "みょ", "りゃ", "りゅ", "りょ", "ぎゃ", "ぎゅ", "ぎょ", "じゃ", "じゅ", "じょ", "びゃ", "びゅ", "びょ", "ぴゃ", "ぴゅ", "ぴょ" };
        public static readonly string[] aoyin2 = new string[] { "キャ", "キュ", "キョ", "シャ", "シュ", "ショ", "チャ", "チュ", "チョ", "ニャ", "ニュ", "ニョ", "ヒャ", "ヒュ", "ヒョ", "ミャ", "ミュ", "ミョ", "リャ", "リュ", "リョ", "ギャ", "ギュ", "ギョ", "ジャ", "ジュ", "ジョ", "ビャ", "ビュ", "ビョ", "ピャ", "ピュ", "ピョ" };
        public static readonly string[] duyin3 = new string[] { "kya", "kyu", "kyo", "sya", "syu", "syo", "cya", "cyu", "cyo", "nya", "nyu", "nyo", "hya", "hyu", "hyo", "mya", "myu", "myo", "rya", "ryu", "ryo", "gya", "gyu", "gyo", "zya", "zyu", "zyo", "bya", "byu", "byo", "pya", "pyu", "pyo" };

    }
}
