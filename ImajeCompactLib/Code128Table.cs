using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ImajeCompactLib
{
    public class Code128Table
    {
        /// <summary>
        /// Code 128의 CodeSet A,B,C
        /// Unknown < C < A < B 
        /// </summary>
        public enum CODESET : int
        {
            /// <summary>
            /// CodeSet C : Only digits 0-9, encoded in pairs, very compact code
            /// </summary>
            C,
            /// <summary>
            /// CodeSet A : Partial ASCII set, no lower case, but ASCII control chars (TAB, CR/LF etc.)
            /// </summary>
            A,
            /// <summary>
            /// CodeSet B : Full ASCII set, no ASCII control chars
            /// </summary>
            B
        }

        ///// <summary>
        ///// 변환 코드셋
        ///// </summary>
        //private CODESET _codeSet;
        ///// <summary>
        ///// 입력 문자열
        ///// </summary>
        //private char _inputData;
        /// <summary>
        /// 하...이거
        /// ReadOnlyCollection<string> 으로 사용했다가.....쩝 데이터 테이블로
        /// 다른사람이 한거보고 더 그럴싸해서 변경함....사실 
        /// Pattern Widths 중 택1로 반환받으려면
        /// 노가다 맘먹고있었는데...잘되었음 ㅠㅠ
        /// http://blog.lenscloth.io/post/2018/11/02/code-128 참고
        /// </summary>
        private DataTable code128Table = new DataTable("Code128Table");


        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="codeset">코드셋</param>
        /// <param name="inputData">입력문자열</param>
        public Code128Table()
        {
            init_Code128Table();

            //this._codeSet = codeset;
            //this._inputData = inputData;
        }

        /// <summary>
        /// I TypeValue()
        /// </summary>
        public string GetTypeValue(CODESET codeset, char inputData)
        {
            string _val = TypeValue(codeset, inputData); // code128테이블 value


            //In the V24 barcode structure, we can use the ASCII table to insert the
            //characters bytes.
            //If we use the Code 128 characters table, we have to add 20h
            //to the characters to be encoded.
            //In the code A, we have to add 20h to the special characters
            //(NUL, SOH, STX…).For example, to insert STX, we have to
            //send 62h(In the code 128 table STX = 66 decimal = 42h).
            //In the Code C, a couple of digit have to be inserted with 2
            //bytes(example 07 will be 30h 37h).
            //That is not necessary to add 20h to the variable delimiter and
            //variable elements (counter, date, etc).
            //V24 바코드 구조에서는 ASCII 테이블을 사용하여
            //문자 바이트.
            //코드 128 문자 표를 사용하는 경우 20h를 추가해야합니다.
            //인코딩 할 문자.
            //코드 A에서 특수 문자에 20h를 추가해야합니다.
            //(NUL, SOH, STX ...).예를 들어 STX를 삽입하려면
            //62h를 보내십시오 (코드 128의 STX 테이블에서 66 = 10 진수 = 42h).
            //코드 C에서 2 자리 숫자를 2로 삽입해야합니다.
            //바이트(예 07은 30h 37h가됩니다).
            //변수 구분 기호에 20h를 추가 할 필요는 없습니다.
            //가변 요소 (카운터, 날짜 등).
            //return string.Format("0x{0}", (int.Parse(_val) + 0x20).ToString("X"));
            Console.WriteLine(inputData + " = " + _val + " = " + (Convert.ToInt32(_val, 16) + 0x20).ToString("X"));
            return (Convert.ToInt32(_val, 16) + 0x20).ToString("X");
            //return TypeValue(codeset, inputData);
        }

        /// <summary>
        /// Codeset에 일치하는 code128의 value반환
        /// </summary>
        /// <returns>value 값</returns>
        private string TypeValue(CODESET codeset, char inputData)
        {
            DataRow[] rows;

            switch (codeset)
            {
                case CODESET.C:
                    rows = this.code128Table.Select(string.Format("C = '{0}'", inputData));
                    break;
                case CODESET.A:
                    rows = this.code128Table.Select(string.Format("A = '{0}'", inputData));
                    break;
                case CODESET.B:
                    rows = this.code128Table.Select(string.Format("B = '{0}'", inputData));
                    break;
                default:
                    rows = this.code128Table.Select(string.Format("B = '{0}'", inputData));
                    break;
            }

            return rows[0]["Value"].ToString();
        }

        #region Code128 테이블정보
        /// <summary>
        /// initialization Code128 DataTable
        /// https://en.wikipedia.org/wiki/Code_128 기준
        /// </summary>
        private void init_Code128Table()
        {
            //CodeSet 비교할때(A와 B) datatable에서 대소문자를 구분해야함!!
            this.code128Table.CaseSensitive = true;

            //항목
            //Value	128A 128B 128C 	(x)Code(ASCII) Pattern Widths
            this.code128Table.Columns.Add("Value", typeof(string));
            this.code128Table.Columns.Add("A", typeof(string));
            this.code128Table.Columns.Add("B", typeof(string));
            this.code128Table.Columns.Add("C", typeof(string));
            this.code128Table.Columns.Add("Pattern", typeof(string));
            this.code128Table.Columns.Add("Widths", typeof(string));

            //하..노가다...data
            //this.code128Table.Rows.Add(new object[] {"AAA", "AAA", "AAA", "AAA", "AAA", "AAA" });
            this.code128Table.Rows.Add(new object[] { "0", " ", " ", "00", "11011001100", "212222" });
            this.code128Table.Rows.Add(new object[] { "1", "!", "!", "01", "11001101100", "222122" });
            this.code128Table.Rows.Add(new object[] { "2", "\"", "\"", "02", "11001100110", "222221" });
            this.code128Table.Rows.Add(new object[] { "3", "#", "#", "03", "10010011000", "121223" });
            this.code128Table.Rows.Add(new object[] { "4", "$", "$", "04", "10010001100", "121322" });
            this.code128Table.Rows.Add(new object[] { "5", "%", "%", "05", "10001001100", "131222" });
            this.code128Table.Rows.Add(new object[] { "6", "&", "&", "06", "10011001000", "122213" });
            this.code128Table.Rows.Add(new object[] { "7", "'", "'", "07", "10011000100", "122312" });
            this.code128Table.Rows.Add(new object[] { "8", "(", "(", "08", "10001100100", "132212" });
            this.code128Table.Rows.Add(new object[] { "9", ")", ")", "09", "11001001000", "221213" });
            this.code128Table.Rows.Add(new object[] { "10", "*", "*", "10", "11001000100", "221312" });
            this.code128Table.Rows.Add(new object[] { "11", "+", "+", "11", "11000100100", "231212" });
            this.code128Table.Rows.Add(new object[] { "12", ",", ",", "12", "10110011100", "112232" });
            this.code128Table.Rows.Add(new object[] { "13", "-", "-", "13", "10011011100", "122132" });
            this.code128Table.Rows.Add(new object[] { "14", ".", ".", "14", "10011001110", "122231" });
            this.code128Table.Rows.Add(new object[] { "15", "/", "/", "15", "10111001100", "113222" });
            this.code128Table.Rows.Add(new object[] { "16", "0", "0", "16", "10011101100", "123122" });
            this.code128Table.Rows.Add(new object[] { "17", "1", "1", "17", "10011100110", "123221" });
            this.code128Table.Rows.Add(new object[] { "18", "2", "2", "18", "11001110010", "223211" });
            this.code128Table.Rows.Add(new object[] { "19", "3", "3", "19", "11001011100", "221132" });
            this.code128Table.Rows.Add(new object[] { "20", "4", "4", "20", "11001001110", "221231" });
            this.code128Table.Rows.Add(new object[] { "21", "5", "5", "21", "11011100100", "213212" });
            this.code128Table.Rows.Add(new object[] { "22", "6", "6", "22", "11001110100", "223112" });
            this.code128Table.Rows.Add(new object[] { "23", "7", "7", "23", "11101101110", "312131" });
            this.code128Table.Rows.Add(new object[] { "24", "8", "8", "24", "11101001100", "311222" });
            this.code128Table.Rows.Add(new object[] { "25", "9", "9", "25", "11100101100", "321122" });
            this.code128Table.Rows.Add(new object[] { "26", ":", ":", "26", "11100100110", "321221" });
            this.code128Table.Rows.Add(new object[] { "27", ";", ";", "27", "11101100100", "312212" });
            this.code128Table.Rows.Add(new object[] { "28", "<", "<", "28", "11100110100", "322112" });
            this.code128Table.Rows.Add(new object[] { "29", "=", "=", "29", "11100110010", "322211" });
            this.code128Table.Rows.Add(new object[] { "30", ">", ">", "30", "11011011000", "212123" });
            this.code128Table.Rows.Add(new object[] { "31", "?", "?", "31", "11011000110", "212321" });
            this.code128Table.Rows.Add(new object[] { "32", "@", "@", "32", "11000110110", "232121" });
            this.code128Table.Rows.Add(new object[] { "33", "A", "A", "33", "10100011000", "111323" });
            this.code128Table.Rows.Add(new object[] { "34", "B", "B", "34", "10001011000", "131123" });
            this.code128Table.Rows.Add(new object[] { "35", "C", "C", "35", "10001000110", "131321" });
            this.code128Table.Rows.Add(new object[] { "36", "D", "D", "36", "10110001000", "112313" });
            this.code128Table.Rows.Add(new object[] { "37", "E", "E", "37", "10001101000", "132113" });
            this.code128Table.Rows.Add(new object[] { "38", "F", "F", "38", "10001100010", "132311" });
            this.code128Table.Rows.Add(new object[] { "39", "G", "G", "39", "11010001000", "211313" });
            this.code128Table.Rows.Add(new object[] { "40", "H", "H", "40", "11000101000", "231113" });
            this.code128Table.Rows.Add(new object[] { "41", "I", "I", "41", "11000100010", "231311" });
            this.code128Table.Rows.Add(new object[] { "42", "J", "J", "42", "10110111000", "112133" });
            this.code128Table.Rows.Add(new object[] { "43", "K", "K", "43", "10110001110", "112331" });
            this.code128Table.Rows.Add(new object[] { "44", "L", "L", "44", "10001101110", "132131" });
            this.code128Table.Rows.Add(new object[] { "45", "M", "M", "45", "10111011000", "113123" });
            this.code128Table.Rows.Add(new object[] { "46", "N", "N", "46", "10111000110", "113321" });
            this.code128Table.Rows.Add(new object[] { "47", "O", "O", "47", "10001110110", "133121" });
            this.code128Table.Rows.Add(new object[] { "48", "P", "P", "48", "11101110110", "313121" });
            this.code128Table.Rows.Add(new object[] { "49", "Q", "Q", "49", "11010001110", "211331" });
            this.code128Table.Rows.Add(new object[] { "50", "R", "R", "50", "11000101110", "231131" });
            this.code128Table.Rows.Add(new object[] { "51", "S", "S", "51", "11011101000", "213113" });
            this.code128Table.Rows.Add(new object[] { "52", "T", "T", "52", "11011100010", "213311" });
            this.code128Table.Rows.Add(new object[] { "53", "U", "U", "53", "11011101110", "213131" });
            this.code128Table.Rows.Add(new object[] { "54", "V", "V", "54", "11101011000", "311123" });
            this.code128Table.Rows.Add(new object[] { "55", "W", "W", "55", "11101000110", "311321" });
            this.code128Table.Rows.Add(new object[] { "56", "X", "X", "56", "11100010110", "331121" });
            this.code128Table.Rows.Add(new object[] { "57", "Y", "Y", "57", "11101101000", "312113" });
            this.code128Table.Rows.Add(new object[] { "58", "Z", "Z", "58", "11101100010", "312311" });
            this.code128Table.Rows.Add(new object[] { "59", "[", "[", "59", "11100011010", "332111" });
            this.code128Table.Rows.Add(new object[] { "60", @"\", @"\", "60", "11101111010", "314111" });
            this.code128Table.Rows.Add(new object[] { "61", "]", "]", "61", "11001000010", "221411" });
            this.code128Table.Rows.Add(new object[] { "62", "^", "^", "62", "11110001010", "431111" });
            this.code128Table.Rows.Add(new object[] { "63", "_", "_", "63", "10100110000", "111224" });
            this.code128Table.Rows.Add(new object[] { "64", "\0", "`", "64", "10100001100", "111422" });
            this.code128Table.Rows.Add(new object[] { "65", Convert.ToChar(1).ToString(), "a", "65", "10010110000", "121124" });
            this.code128Table.Rows.Add(new object[] { "66", Convert.ToChar(2).ToString(), "b", "66", "10010000110", "121421" });
            this.code128Table.Rows.Add(new object[] { "67", Convert.ToChar(3).ToString(), "c", "67", "10000101100", "141122" });
            this.code128Table.Rows.Add(new object[] { "68", Convert.ToChar(4).ToString(), "d", "68", "10000100110", "141221" });
            this.code128Table.Rows.Add(new object[] { "69", Convert.ToChar(5).ToString(), "e", "69", "10110010000", "112214" });
            this.code128Table.Rows.Add(new object[] { "70", Convert.ToChar(6).ToString(), "f", "70", "10110000100", "112412" });
            this.code128Table.Rows.Add(new object[] { "71", Convert.ToChar(7).ToString(), "g", "71", "10011010000", "122114" });
            this.code128Table.Rows.Add(new object[] { "72", Convert.ToChar(8).ToString(), "h", "72", "10011000010", "122411" });
            this.code128Table.Rows.Add(new object[] { "73", Convert.ToChar(9).ToString(), "i", "73", "10000110100", "142112" });
            this.code128Table.Rows.Add(new object[] { "74", Convert.ToChar(10).ToString(), "j", "74", "10000110010", "142211" });
            this.code128Table.Rows.Add(new object[] { "75", Convert.ToChar(11).ToString(), "k", "75", "11000010010", "241211" });
            this.code128Table.Rows.Add(new object[] { "76", Convert.ToChar(12).ToString(), "l", "76", "11001010000", "221114" });
            this.code128Table.Rows.Add(new object[] { "77", Convert.ToChar(13).ToString(), "m", "77", "11110111010", "413111" });
            this.code128Table.Rows.Add(new object[] { "78", Convert.ToChar(14).ToString(), "n", "78", "11000010100", "241112" });
            this.code128Table.Rows.Add(new object[] { "79", Convert.ToChar(15).ToString(), "o", "79", "10001111010", "134111" });
            this.code128Table.Rows.Add(new object[] { "80", Convert.ToChar(16).ToString(), "p", "80", "10100111100", "111242" });
            this.code128Table.Rows.Add(new object[] { "81", Convert.ToChar(17).ToString(), "q", "81", "10010111100", "121142" });
            this.code128Table.Rows.Add(new object[] { "82", Convert.ToChar(18).ToString(), "r", "82", "10010011110", "121241" });
            this.code128Table.Rows.Add(new object[] { "83", Convert.ToChar(19).ToString(), "s", "83", "10111100100", "114212" });
            this.code128Table.Rows.Add(new object[] { "84", Convert.ToChar(20).ToString(), "t", "84", "10011110100", "124112" });
            this.code128Table.Rows.Add(new object[] { "85", Convert.ToChar(21).ToString(), "u", "85", "10011110010", "124211" });
            this.code128Table.Rows.Add(new object[] { "86", Convert.ToChar(22).ToString(), "v", "86", "11110100100", "411212" });
            this.code128Table.Rows.Add(new object[] { "87", Convert.ToChar(23).ToString(), "w", "87", "11110010100", "421112" });
            this.code128Table.Rows.Add(new object[] { "88", Convert.ToChar(24).ToString(), "x", "88", "11110010010", "421211" });
            this.code128Table.Rows.Add(new object[] { "89", Convert.ToChar(25).ToString(), "y", "89", "11011011110", "212141" });
            this.code128Table.Rows.Add(new object[] { "90", Convert.ToChar(26).ToString(), "z", "90", "11011110110", "214121" });
            this.code128Table.Rows.Add(new object[] { "91", Convert.ToChar(27).ToString(), "{", "91", "11110110110", "412121" });
            this.code128Table.Rows.Add(new object[] { "92", Convert.ToChar(28).ToString(), "|", "92", "10101111000", "111143" });
            this.code128Table.Rows.Add(new object[] { "93", Convert.ToChar(29).ToString(), "}", "93", "10100011110", "111341" });
            this.code128Table.Rows.Add(new object[] { "94", Convert.ToChar(30).ToString(), "~", "94", "10001011110", "131141" });

            this.code128Table.Rows.Add(new object[] { "95", Convert.ToChar(31).ToString(), Convert.ToChar(127).ToString(), "95", "10111101000", "114113" });
            this.code128Table.Rows.Add(new object[] { "96", Convert.ToChar(202).ToString()/*FNC3*/, Convert.ToChar(202).ToString()/*FNC3*/, "96", "10111100010", "114311" });
            this.code128Table.Rows.Add(new object[] { "97", Convert.ToChar(201).ToString()/*FNC2*/, Convert.ToChar(201).ToString()/*FNC2*/, "97", "11110101000", "411113" });
            this.code128Table.Rows.Add(new object[] { "98", "SHIFT", "SHIFT", "98", "11110100010", "411311" });
            this.code128Table.Rows.Add(new object[] { "99", "CODE_C", "CODE_C", "99", "10111011110", "113141" });
            this.code128Table.Rows.Add(new object[] { "100", "CODE_B", Convert.ToChar(203).ToString()/*FNC4*/, "CODE_B", "10111101110", "114131" });
            this.code128Table.Rows.Add(new object[] { "101", Convert.ToChar(203).ToString()/*FNC4*/, "CODE_A", "CODE_A", "11101011110", "311141" });
            this.code128Table.Rows.Add(new object[] { "102", Convert.ToChar(200).ToString()/*FNC1*/, Convert.ToChar(200).ToString()/*FNC1*/, Convert.ToChar(200).ToString()/*FNC1*/, "11110101110", "411131" });
            this.code128Table.Rows.Add(new object[] { "103", "START_A", "START_A", "START_A", "11010000100", "211412" });
            this.code128Table.Rows.Add(new object[] { "104", "START_B", "START_B", "START_B", "11010010000", "211214" });
            this.code128Table.Rows.Add(new object[] { "105", "START_C", "START_C", "START_C", "11010011100", "211232" });
            this.code128Table.Rows.Add(new object[] { "", "STOP", "STOP", "STOP", "11000111010", "233111" });
        } // init_Code128Table
        #endregion
    }
}
