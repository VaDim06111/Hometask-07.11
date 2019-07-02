using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileParser
{
    class TextAssistent
    {
        static string path = AppDomain.CurrentDomain.BaseDirectory + "\\.." + "\\.." + @"\sample.txt";
        static string pathInfo = AppDomain.CurrentDomain.BaseDirectory + "\\.." + "\\.." + @"\sampleInfo.txt";
        static string pathCount = AppDomain.CurrentDomain.BaseDirectory + "\\.." + "\\.." + @"\sampleWordsCount.txt";
        Parser parser = new Parser(path);
        string Text { get; set; }
        List<string> Sentences = new List<string>();
        List<string> Words = new List<string>();
        List<string> Phrases = new List<string>();
        public TextAssistent()
        {
            Text = parser.TakeTxt();
        }
        public void GetSentences()
        {
            Sentences = parser.ParseToSentences(Text);
            Console.WriteLine("Opetation parse to sentences is done");
        }
        public void GetWords()
        {
            Words = parser.ParseToWords(Text);
            Console.WriteLine("Opetation parse to words is done");
        }
        public void GetPhrases()
        {
            Phrases = parser.ParseByPunctuationMarks(Text);
            Console.WriteLine("Opetation parse by punctuation marks is done");
        }
        public string GetLongestSentenceBySymbols()
        {
            if (Sentences.Count == 0)
            {
                GetSentences();
            }
            Sentences.Sort((a, b) => b.Length - a.Length);
            return Sentences[0].ToString();
        }
        public string GetShortestSentencebyWords()
        {
            if (Sentences.Count == 0)
            {
                GetSentences();
            }
            return Sentences.OrderBy(q => q.Split(' ').Length).FirstOrDefault();
        }
        public void WriteToFileResult()
        {           
            string shortSentence = GetShortestSentencebyWords();
            string longestSentence = GetLongestSentenceBySymbols();
            string mostCommonLetter = GetMostPopularLetter();

            using (StreamWriter sw = new StreamWriter(pathInfo, false, Encoding.UTF8))
            {
                sw.WriteLine($"The longest sentence by count symbols: {Environment.NewLine}{longestSentence}");
                sw.WriteLine($"{Environment.NewLine}Shortest sentence by words: {Environment.NewLine}{shortSentence}");
                sw.WriteLine(mostCommonLetter);
            }
        }
        public void GetListOfWordsWithCountOfRepeat()
        {
            Dictionary<string, int> repeats = new Dictionary<string, int>();  
            if (Words.Count == 0)
            {
                GetWords();
            }
            foreach (var word in Words)
            {
                if (repeats.ContainsKey(word))
                {
                    repeats[word] += 1;
                }
                else
                {
                    repeats[word] = 1;
                }
            }                    
            using (StreamWriter sw = new StreamWriter(pathCount, false, Encoding.UTF8))
            {
                sw.WriteLine($"Different words count: {Words.Distinct().Count()}");
                foreach (var item in repeats.OrderBy(k=>k.Key))
                {
                    sw.WriteLine($"Word: {item.Key} - is found {item.Value} times");
                }               
            }
            Console.WriteLine("File recorded!");
        }  
        public string GetMostPopularLetter()
        {
            Dictionary<char, int> letters = new Dictionary<char, int>();
            foreach (var letter in Text)
            {
                if (letter != ' ')
                {
                    string buf = letter.ToString();
                    buf = buf.ToUpper();
                    if (letters.ContainsKey(Convert.ToChar(buf)))
                    {
                        letters[Convert.ToChar(buf)] += 1;
                    }
                    else
                    {
                        letters[Convert.ToChar(buf)] = 1;
                    }
                }
            }
            string _letter = letters.OrderByDescending(k => k.Value).Select(k => k.Key).FirstOrDefault().ToString();
            string count = letters.OrderByDescending(k => k.Value).Select(k => k.Value).FirstOrDefault().ToString();           
            return $"The most common letter is - {_letter}, was found {count} times";
        }
    }
}
