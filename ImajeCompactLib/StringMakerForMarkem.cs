using System;
using System.Collections.Generic;
using System.Text;

namespace ImajeCompactLib
{
    public class StringMakerForMarkem
    {
        public static string SetForMarkemString(int lineNum, string campo1, string campo2)
        {
            string returnString = string.Empty;
            //CRC   Cyclic Redundancy Checking   순환중복검사
            int sendLength;
            string header;
            string data;
            //created string????
            int CRC_Header;
            int CRC_Data1;
            int CRC_Data2;
            //전송할 문자의 길이
            sendLength = campo1.Length + campo2.Length;
            //헤더
            header = "" + (char)1 + (char)(128 + lineNum) + (char)196 + (char)(10 + sendLength) + (char)0;
            //CRC for the Header command
            CRC_Header = GetCRCHeader(header);

            //String creation with new data
            data = "" + (char)0 + (char)0 + (char)0 + (char)0 + (char)0 + (char)2 + (char)0 + campo1 + (char)0 + (char)1 + campo2 + (char)0;

            CRC_Data1 = GetCRCData(data).Item1;
            CRC_Data2 = GetCRCData(data).Item2;

            returnString = header + (char)CRC_Header + data + (char)CRC_Data1 + (char)CRC_Data2;

            return returnString;
        }

        public static Tuple<int, int> GetCRCData(string data)
        {
            int CRC = 65535;
            int resultC1;
            int resultC2;
            //Call the function ByteCRC for all the characters
            foreach (char item in data)
            {
                CRC = GetByteCRC(CRC, (int)item);
            }
            resultC1 = (CRC & 255);
            resultC2 = ((CRC - resultC1) / 256);

            var tuple = new Tuple<int, int>(resultC1, resultC2);

            return tuple;
        }

        public static int GetByteCRC(int sum, int dat)
        {
            int result;
            sum = sum ^ dat; //xor

            for (int i = 0; i <= 7; i++)
            {
                if ((sum & 1) == 0)
                {
                    sum = (int)(sum / 2);
                }
                else
                {
                    sum = (int)(sum / 2) ^ 40961;
                }
            }

            result = sum;

            return result;
        }

        public static int GetCRCHeader(string headerString)
        {
            //Sum of all the ASCII code of the string and
            //reduce it in a Byte (And 255)
            //문자열의 아스키코드 합을 255와 and연산으로Byte로 줄인다.
            int CRC = 0;

            foreach (char item in headerString)
            {
                CRC = (CRC + (int)item & 255);
            }

            CRC = 255 - CRC;

            return CRC;
        }
    }
}
