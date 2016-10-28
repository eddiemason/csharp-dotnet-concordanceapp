using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConcordanceApp
{
    class Program
    {     
        static private int iCurrentSentenceNum = 1;
        static private SortedDictionary<string, int> sWordOccuranceDictionary = new SortedDictionary<string, int>();
        static private SortedDictionary<string, string> sWordSentenceDictionary = new SortedDictionary<string, string>();

        static void Main(string[] args)
        {
            //Uses the text file within the project.
            string sFilePath = "..\\..\\ConcordanceAppTestFile.txt";
             
            string[] sWords = GetWordsFromFile(sFilePath);
            string sKey = string.Empty;

            foreach (string sWord in sWords)
            {
                sKey = sWord.Trim(new Char[]{ ',' , ':' , '.' } );

                //Keeps track of word occurance
                TrackWordOccurence(sKey);

                //Keeps track of the sentence the word appears in.
                TrackSentenceOccurence(sKey, sWord);
            }

            PrintToConsole(sWordOccuranceDictionary, sWordSentenceDictionary);
        }

        /// <summary>
        /// Retrieves files contents.
        /// </summary>
        /// <param name="sPath">Location of file</param>
        /// <returns>Returns an array of words</returns>
        static private string[] GetWordsFromFile(string sPath)
        {
            string[] sWords = new string[0];
            string sLine = String.Empty;

            using (StreamReader sr = new StreamReader(sPath))
            {
                while (sr.Peek() >= 0)
                {
                    sLine = sr.ReadLine();
                    sWords = sLine.Split();
                }
            }

            return sWords;
        }

        /// <summary>
        /// Determines when a new sentence begins by checking to see if a ".", "!", or "?" is at the end of the word.
        /// </summary>
        /// <param name="sWord">The word being checked for ending punctuation.</param>
        static private void SetSentenceNumber(string sWord)
        {
            string sLastChar = sWord.Substring(sWord.Length - 1);
            int iNumOfPeriods = sWord.Count(c => c == '.');

            if ((iNumOfPeriods == 1) && ((sLastChar.Equals(".")) || (sLastChar.Equals("?")) || (sLastChar.Equals("!"))))
            {
                iCurrentSentenceNum++;
            }

        }

        /// <summary>
        /// Keeps track of how many times the word appears in the file.
        /// </summary>
        /// <param name="sKey"></param>
        static private void TrackWordOccurence(string sKey)
        {
            if (sWordOccuranceDictionary.ContainsKey(sKey))
            {
                sWordOccuranceDictionary[sKey] = sWordOccuranceDictionary[sKey] + 1;
            }
            else
            {
                sWordOccuranceDictionary.Add(sKey, 1);
            }
        }

        /// <summary>
        /// Keeps track of which sentences the word occurs in.
        /// </summary>
        /// <param name="sKey">Dictionary key</param>
        /// <param name="sWord">Word used to determine sentence number</param>
        /// <returns>A dictionary of words and which sentences they appear in.</returns>
        static private void TrackSentenceOccurence(string sKey, string sWord)
        {
            
            if (sWordSentenceDictionary.ContainsKey(sKey))
            {
                sWordSentenceDictionary[sKey] += ", " + iCurrentSentenceNum.ToString();
            }
            else
            {
                sWordSentenceDictionary[sKey] = iCurrentSentenceNum.ToString();
            }

            SetSentenceNumber(sWord);
        }

        /// <summary>
        /// Prints condcordance to console.
        /// </summary>
        /// <param name="sWordDict">Dictionary that keeps track of word occurance</param>
        /// <param name="sSentenceDict">Dictionary that keeps track of sentence word occurs in</param>
        static private void PrintToConsole(SortedDictionary<string, int> sWordDict, SortedDictionary<string, string> sSentenceDict)
        {
            char[] cAlphaArray = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            string sAlphaBullet = String.Empty;
            int iCount = 0;
            int iReset = 1;

            foreach (KeyValuePair<string, int> kvpEntry in sWordDict)
            {
                if (iCount == cAlphaArray.Length)
                {
                    iCount = 0;
                    iReset += 1;
                }

                sAlphaBullet = cAlphaArray[iCount].ToString();

                if (iReset > 1)
                {
                    for (int i = 1; i < iReset; i++)
                    {
                        sAlphaBullet += cAlphaArray[iCount].ToString();
                    }
                }

                Console.WriteLine("{0}. {1} {{ {2}: {3} }}", sAlphaBullet, kvpEntry.Key.ToLower(), kvpEntry.Value, sSentenceDict[kvpEntry.Key]);

                iCount++;
            }
            Console.Read();
        }
    }
}
