using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace FileParser
{
    class Parser
    {
        string Path { get; set; }
        object locker = new object();
        string[] massymbol = new[] { ">", "<", "\"", "^", "&", "(", ")", "[", "]", "{", "}", "»", "«", "_", "|", "+", "-", "#", "$", "%", "*", "/", "=", "~" };
        public Parser(string _path)
        {
            Path = _path;
        }
        public string TakeTxt()
        {
            string text;
            lock (locker)
            {
                using (StreamReader sr = new StreamReader(Path, Encoding.UTF8))
                {
                    text = sr.ReadToEnd();
                }
            }
            string bufer = text;
            for (int i = 0; i < massymbol.Length; i++)
            {
                if (text.Contains(massymbol[i]))
                {
                    bufer = bufer.Replace(massymbol[i], "");
                    text = bufer;
                }
            }
            text = text.Replace("\n", " ");
            text = text.Replace("\t", " ");

            return text;
        }
        public string TakeTxt(string _Path)
        {
            string text;
            lock (locker)
            {
                using (StreamReader sr = new StreamReader(_Path, Encoding.UTF8))
                {
                    text = sr.ReadToEnd();
                }
            }
            string bufer = text;
            for (int i = 0; i < massymbol.Length; i++)
            {
                if (text.Contains(massymbol[i]))
                {
                    bufer = bufer.Replace(massymbol[i], "");
                    text = bufer;
                }
            }
            text = text.Replace("\n", " ");
            text = text.Replace("\t", " ");

            return text;
        }
        public List<string> ParseToSentences(string Text)
        {
            string[] mas;           
            List<string> Sentences = new List<string>();
            mas = Text.Split(new string[] { ".", "?", "!", "?!", "..", "...", "!..", "?.." }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var sentence in mas)
            {
                Sentences.Add(sentence.Trim());
            }
            return Sentences;
        }

        public List<string> ParseToWords(string Text)
        {
            List<string> Words = new List<string>();
            List<string> Sentences = ParseToSentences(Text);
            string[] mas;           
            foreach (var sentence in Sentences)
            {
                mas = sentence.Split(new string[] { " ", ",", "—", ":", ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in mas)
                {
                    string _word = "";
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (!Char.IsNumber(word[i]))
                        {
                            _word += word[i];
                        }
                    }
                    _word = _word.ToLower();
                    if(_word!="")
                    {
                        if (_word != "'" && _word[0] != '\'')
                            Words.Add(_word);
                        else
                        {
                            _word = _word.Replace("'", "");
                            if(_word != "")
                            Words.Add(_word);
                        }
                    }                                                            
                }
            }           
            return Words;
        }
        public List<string> ParseByPunctuationMarks(string Text)
        {
            List<string> Phrases = new List<string>();            
            string[] mas = Text.Split(new string[] { ",", "—", ":", ";", ".", "?", "!", "?!", "..", "...", "!..", "?.." }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var phrase in mas)
            {               
                Phrases.Add(phrase.Trim());               
            }
            return Phrases;
        }

    }
}
