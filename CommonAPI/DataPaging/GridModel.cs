using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Common.DataPaging
{
    public class GridModel<T>
    {
        public int FirstPage { get; set; }
        public int LastPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRecord { get; set; }
        public int TotalPage { get; set; }
        public int SizePage { get; set; }

        public List<T> Data { get; set; }

        public StringBuilder GeneralPaging(string classCurrent, string classClick)
        {
            ConfigPage();

            var stringHtml = new StringBuilder();

            if (TotalPage > 1)
            {
                var txtFirstPage = FirstPage == 0 ? "" : "...";
                var txtLastPage = LastPage == TotalPage ? "" : ".../" + TotalPage.ToString();

                stringHtml.AppendLine(string.Format("<div class=\"container text-center allPage\">"));
                stringHtml.AppendLine(string.Format("<div class=\"row\">"));
                stringHtml.AppendLine(string.Format("<ul class=\"pagination\">"));
                stringHtml.AppendLine(string.Format("<li><a class=\"{0}\" href=\"#\" data-value=\"0\"> {1} &laquo;&laquo;</a></li>", classClick, txtFirstPage));
                stringHtml.AppendLine(string.Format("<li><a class=\"{0}\" href=\"#\" data-value=\"{1}\" style=\"{2}\"> &laquo;</a></li>", classClick, (CurrentPage - 1).ToString(), (String.IsNullOrEmpty(txtFirstPage) ? "display:none" : "display:block")));

                for (var i = FirstPage; i < LastPage; i++)
                {
                    stringHtml.AppendLine(string.Format("<li><a href=\"#\" class=\"{0}\" data-value=\"{1}\">{2}</a></li>", CurrentPage == i ? classCurrent : classClick, i.ToString(), (i + 1).ToString()));
                }

                stringHtml.AppendLine(string.Format("<li><a class=\"{0}\" href=\"#\" data-value=\"{1}\" style=\"{2}\">&raquo;</a></li>", classClick, (CurrentPage + 1).ToString(), (String.IsNullOrEmpty(txtLastPage) ? "display:none" : "display:block")));
                stringHtml.AppendLine(string.Format("<li><a class=\"{0}\" data-value=\"{1}\" href=\"#\">&raquo;&raquo; {2}</a></li>", classClick, (TotalPage - 1).ToString(), txtLastPage));
                stringHtml.AppendLine(string.Format("</ul>"));
                stringHtml.AppendLine(string.Format("</div>"));
                stringHtml.AppendLine(string.Format("</div>"));
            }

            return stringHtml;
        }

        public void ConfigPage()
        {
            SizePage = SizePage == 0 ? 10 : SizePage;

            LastPage = (((int)(CurrentPage / SizePage) + 1) * SizePage);
            FirstPage = LastPage - SizePage;

            if (CurrentPage == LastPage && CurrentPage < TotalPage - 1)
            {
                FirstPage = ++CurrentPage;

                LastPage = (((int)(CurrentPage / SizePage) + 1) * SizePage);
            }
            else
                if (!Data.Any() && CurrentPage == TotalPage && CurrentPage != 0)
                {
                    LastPage = ++TotalPage;
                }

            LastPage++;
            LastPage = LastPage >= TotalPage ? TotalPage : LastPage;
        }
    }
}
