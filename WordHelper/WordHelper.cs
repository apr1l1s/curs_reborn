using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Windows;
using curs_reborn.Models;

namespace curs_reborn.WordHelper
{

    internal class WordHelper
    {
        //информация о файле
        private FileInfo _fileInfo;
        //конструктор (имя шаблона с полным путём)
        public WordHelper(string fileName) 
        {
            /*
             WordHelper.WordHelper helper = new WordHelper.WordHelper("Ведомость.docx");
                var keys = new Dictionary<string, string>
                {
                    {"<year>", comboYear.SelectedValue.ToString() },
                    {"<term>", comboTerm.SelectedValue.ToString() },
                    {"<group>", comboGroups.SelectedValue.ToString() }
                };
            helper.Process(keys);
            */
            //если файл существует
            if (File.Exists(fileName))
            {
                _fileInfo = new FileInfo(fileName);
            }
            else
            {
                throw new ArgumentException("Файл не найден");
            }
        }
        internal void FillTable(List<selectStatement_Result> st, Word.Application app)
        {
            Word.Range r = app.ActiveDocument.Tables[1].Range;
            object missingObj = Missing.Value;
            for (int i=0; i<st.Count; i++)
            {
                app.ActiveDocument.Tables[1].Rows.Add(ref missingObj);
                app.ActiveDocument.Tables[1].Cell(2+i, 1).Range.Text = st[i].surname.ToString();
                app.ActiveDocument.Tables[1].Cell(2 + i, 2).Range.Text = st[i].mark.ToString();
                app.ActiveDocument.Tables[1].Cell(2 + i, 3).Range.Text = st[i].grant.ToString();
                app.ActiveDocument.Tables[1].Cell(2 + i, 4).Range.Text = st[i].cost.ToString();
            }
        }
        internal void Process(Dictionary<string,string> items, List<selectStatement_Result> st)
        {
            Word.Application app = null;
            try
            {
                app = new Word.Application();
                //Обьектные параметры для word
                Object file = _fileInfo.FullName;
                Object missing = Type.Missing;
                //Открывается копия документа по шаблону указаному в file
                Word.Document doc = app.Documents.Open(file);
                foreach (var item in items)
                {
                    //перебираются все теги и заменяются на значения
                    Word.Find find = app.Selection.Find;
                    //тег
                    find.Text = item.Key;
                    //что вместо него встанет
                    find.Replacement.Text = item.Value;
                    //word принимает обьектные параметры
                    Object wrap = Word.WdFindWrap.wdFindContinue;
                    Object replace = Word.WdReplace.wdReplaceAll;

                    find.Execute(FindText: Type.Missing,
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
                FillTable(st, app);
                app.Visible = true;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\nПриложение закрыто");
                if (app != null)
                {
                    app.Quit();
                }
            }
            
        }
    }
}
