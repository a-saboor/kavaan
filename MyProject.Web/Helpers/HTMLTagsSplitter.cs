using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using MyProject.Web.ViewModels;

namespace MyProject.Web.Helpers
{
    public class HTMLTagsSplitter
    {
        public static List<HTMLTagsSplitResultView> ToSplitList(string remark)
        {
            List<HTMLTagsSplitResultView> ReturnList = new List<HTMLTagsSplitResultView>();

            if (!string.IsNullOrEmpty(remark))
            {
                if(remark.Contains("<"))
				{

                    string[] seperator = { "<hr />", "<br />" };
                    List<string> List = remark.Split(seperator, StringSplitOptions.RemoveEmptyEntries).ToList();

                    for (int i = 0; List.Count() % 2 != 0 ? i < List.Count() - 1 : i < List.Count(); i += 2)
                    {
                        ReturnList.Add(new HTMLTagsSplitResultView
                        {
                            Comment = Regex.Replace(List[i], "<.*?>", String.Empty),
                            Date = Regex.Replace(List[i + 1], "<.*?>", String.Empty)
                        });
                    }
                }
				else
				{
                    ReturnList.Add(new HTMLTagsSplitResultView
                    {
                        Date = "",
                        Comment = remark
                    });
                }
            }

            return ReturnList;
        }
    }
}