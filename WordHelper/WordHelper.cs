using System;
using System.Collections.Generic;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using System.Windows;
using curs_reborn.Models;

namespace curs_reborn.WordHelper
{

    internal class WordHelper
    {
        private FileInfo _fileInfo;
        public WordHelper(string fileName) 
        {
            if (File.Exists(fileName))
            {
                _fileInfo = new FileInfo(fileName);
            }
            else
            {
                throw new ArgumentException("Файл не найден");
            }
        }
        internal void Process(Dictionary<string,string> items, List<selectStatement_Result> st)
        {
            Word.Application app = null;
            try
            {
                Object file = _fileInfo.FullName;
                Object missing = Type.Missing;
                Object wrap = Word.WdFindWrap.wdFindContinue;
                Object replace = Word.WdReplace.wdReplaceAll;
                app = new Word.Application();
                app.Documents.Add(file, missing, missing, missing);
                foreach (var item in items)
                {
                    Word.Find find = app.Selection.Find;
                    find.Text = item.Key;
                    find.Replacement.Text = item.Value;
                    find.Execute(FindText: missing,
                        MatchCase: false,
                        MatchWholeWord: false,
                        MatchWildcards: false,
                        MatchSoundsLike: missing,
                        MatchAllWordForms: false,
                        Forward: false,
                        Wrap: wrap,
                        Format: false,
                        ReplaceWith: missing,
                        Replace: replace);
                }
                for (int i = 0; i < st.Count; i++)
                {
                    app.ActiveDocument.Tables[1].Rows.Add(ref missing);
                    app.ActiveDocument.Tables[1].Cell(2 + i, 1).Range.Text = st[i].surname.ToString();
                    app.ActiveDocument.Tables[1].Cell(2 + i, 2).Range.Text = st[i].mark.ToString();
                    app.ActiveDocument.Tables[1].Cell(2 + i, 3).Range.Text = st[i].grant.ToString();
                    app.ActiveDocument.Tables[1].Cell(2 + i, 4).Range.Text = st[i].cost.ToString();
                }
                app.Visible = true;
            } catch(Exception ex)
            {
                MessageBox.Show($"{ex}");
                if (app != null)
                {
                    app.Quit();
                }
            }
            
        }
    }
}
