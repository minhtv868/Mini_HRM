using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace IC.Application.Common.Helpers
{
	public class HtmlDocumentHelper
	{
        public static HtmlDocument GetHtmlDoc(string source)
        {
            try
            {
                HtmlDocument htmlDoc = new HtmlDocument
                {
                    OptionFixNestedTags = true,
                    OptionAutoCloseOnEnd = true,
                    OptionDefaultStreamEncoding = Encoding.UTF8
                };
                if (string.IsNullOrEmpty(source))
                {
                    return htmlDoc;
                }
                if (!source.Contains("<") || !source.Contains(">"))
                {
                    return htmlDoc;
                }

                source = Regex.Replace(source, "<!–(.|\\s)*?–>", "");
                source = Regex.Replace(source, "<!-(.|\\s)*?->", "");
                // Remove Doctype
                source = Regex.Replace(source, "<!(.|\\s)*?>", "");
                source = source.Replace("\r\n\r\n", "");
                
                htmlDoc.LoadHtml(source);
               
                return htmlDoc;

            }
            catch
            {
                return null;
            }
        }
        public static string GetTextOnlyFromHtml(string source)
		{
			try
			{
				if (string.IsNullOrEmpty(source))
				{
					return source;
				}
				if (!source.Contains("<") || !source.Contains(">"))
				{
					return source;
				}
				
				HtmlDocument htmlDoc = GetHtmlDoc(source);
				string RetVal = "";
				foreach (HtmlNode htmlNode in htmlDoc.DocumentNode.ChildNodes)
				{
					if (htmlNode.Name.Contains("style"))
					{
						continue;
					}
					if (htmlNode.Name.Contains(":"))
					{
						continue;
					}
					if (isInlineTag(htmlNode.Name))
					{
						RetVal += getInnerText(htmlNode);
					}
					else
					{
						RetVal += getInnerText(htmlNode) + Environment.NewLine;
					}
				}
				RetVal = System.Net.WebUtility.HtmlDecode(RetVal);
				RetVal = RetVal.Replace((char)160, (char)32);
				for (int index = 0; index < 3; index++)
				{
					while (RetVal.Contains("  "))
					{
						RetVal = RetVal.Replace("  ", " ");
					}
					while (RetVal.Contains(Environment.NewLine + " "))
					{
						RetVal = RetVal.Replace(Environment.NewLine + " ", Environment.NewLine);
					}
					while (RetVal.Contains(Environment.NewLine + "&nbsp;"))
					{
						RetVal = RetVal.Replace(Environment.NewLine + "&nbsp;", Environment.NewLine);
					}
					while (RetVal.Contains(" " + Environment.NewLine))
					{
						RetVal = RetVal.Replace(" " + Environment.NewLine, Environment.NewLine);
					}
					while (RetVal.Contains("&nbsp;" + Environment.NewLine))
					{
						RetVal = RetVal.Replace("&nbsp;" + Environment.NewLine, Environment.NewLine);
					}
					while (RetVal.Contains("\t\t"))
					{
						RetVal = RetVal.Replace("\t\t", "\t");
					}
					while (RetVal.Contains("\t" + Environment.NewLine))
					{
						RetVal = RetVal.Replace("\t" + Environment.NewLine, Environment.NewLine);
					}
					while (RetVal.Contains(Environment.NewLine + "\t"))
					{
						RetVal = RetVal.Replace(Environment.NewLine + "\t", Environment.NewLine);
					}
					while (RetVal.Contains(Environment.NewLine + Environment.NewLine))
					{
						RetVal = RetVal.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
					}
				}

				if (RetVal.StartsWith("\r\n"))
				{
					RetVal = RetVal.Substring(2);
				}
				return RetVal;

			}
			catch
			{
				return source;
			}
		}
		
		public static string getInnerText(HtmlNode htmlNode)
		{
			string InnerText = "";
			if (htmlNode.ChildNodes.Count > 0)
			{
				foreach (HtmlNode m_ChildNode in htmlNode.ChildNodes)
				{
					if (isInlineTag(m_ChildNode.Name))
					{
						InnerText += getInnerText(m_ChildNode);
					}
					else
					{
						InnerText += Environment.NewLine + getInnerText(m_ChildNode);
					}
				}
			}
			else
			{
				InnerText = htmlNode.InnerText.Replace(Environment.NewLine, " ");
			}
			return InnerText;
		}
		public static bool isInlineTag(string tagName)
		{
			string tagInline = "span,label,i,a,b,em,small,strong,sub,sup";
			if (tagName.Contains("text"))
			{
				return true;
			}
			foreach (string tagNameInline in tagInline.Split(','))
			{
				if (tagName == tagNameInline)
				{
					return true;
				}
			}
			return false;
		}
        public static HtmlDocument ClearHtmlDoc(HtmlDocument doc)
        {
            HtmlDocument result = doc;
            removeUnknowTag(result);
            int IndexParagraph = 0;
            string ParagraphText = "";
            List<HtmlNode> nodeDelete = new List<HtmlNode>();
            HtmlNodeCollection l_NodeP = result.DocumentNode.SelectNodes("//p");
            if (l_NodeP != null)
            {
                for (int indexNode = 0; indexNode < l_NodeP.Count; indexNode++)
                {
                    HtmlNode htmlNode = l_NodeP[indexNode];
                    removeStyle(htmlNode);
                    try
                    {


                        ParagraphText = System.Net.WebUtility.HtmlDecode(htmlNode.InnerText);
                        ParagraphText = ParagraphText.Replace(Environment.NewLine, " ");
                        if (String.IsNullOrEmpty(ParagraphText.Trim()))
                            continue;
                        htmlNode.InnerHtml = decodeGreaterAndLitter(System.Net.WebUtility.HtmlDecode(encodeGreaterAndLitter(htmlNode.InnerHtml)));
                        if (htmlNode.HasChildNodes)
                        {
                            string innerHtmlClear = "";

                            foreach (var childNode in htmlNode.ChildNodes)
                            {
                                innerHtmlClear += childNode.OuterHtml.Trim();
                                innerHtmlClear = innerHtmlClear.Replace(Environment.NewLine, " ");
                                innerHtmlClear = innerHtmlClear.Replace("\r", " ");
                                innerHtmlClear = innerHtmlClear.Replace("\n", " ");
                                while (innerHtmlClear.Contains("  "))
                                {
                                    innerHtmlClear = innerHtmlClear.Replace("  ", " ");
                                }
                            }
                            htmlNode.InnerHtml = innerHtmlClear.Trim();
                        }
                    }
                    catch
                    {

                        IndexParagraph++;
                        continue;
                    }

                }
            }

            if (result.DocumentNode.SelectNodes("//table") != null)
            {
                foreach (HtmlNode htmlNode in result.DocumentNode.SelectNodes("//table"))
                {
                    removeStyle(htmlNode);
                    string tableStyle = "";
                    if (htmlNode.Attributes["style"] != null)
                        tableStyle = htmlNode.Attributes["style"].Value;
                    if (string.IsNullOrEmpty(tableStyle))
                    {
                        tableStyle = "border-collapse:collapse;width: 100%;";
                    }
                    else
                    {
                        if (!tableStyle.Contains("border-collapse:"))
                        {
                            tableStyle += ";border-collapse:collapse";
                        }
                        if (!tableStyle.Contains("width:"))
                        {
                            tableStyle += ";width: 100%;";
                        }
                    }

                    string tableText = System.Web.HttpUtility.HtmlDecode(htmlNode.InnerText).ToLower().Replace("\r\n", " ").Replace("  ", " ");
                    while (tableText.Contains("  "))
                    {
                        tableText = tableText.Replace("  ", " ");
                    }


                    if (!string.IsNullOrEmpty(tableStyle))
                    {
                        htmlNode.SetAttributeValue("style", tableStyle);
                    }
                    var trnotes = htmlNode.SelectNodes("//tr");
                    if (trnotes != null)
                    {
                        foreach (var trnote in trnotes)
                        {
                            if (trnote.Attributes["style"] != null)
                            {
                                removeStyle(trnote);
                            }

                        }

                    }
                    var tdnotes = htmlNode.SelectNodes("//td");
                    if (tdnotes != null)
                    {
                        foreach (var tdnote in tdnotes)
                        {
                            if (tdnote.Attributes["style"] != null)
                            {
                                removeStyle(tdnote);
                            }

                        }

                    }
                    var spannotes = htmlNode.SelectNodes("//span");
                    if (spannotes != null)
                    {
                        foreach (var spannote in spannotes)
                        {
                            if (spannote.Attributes["style"] != null)
                            {
                                removeStyle(spannote);
                            }

                        }

                    }
                    var pnotes = htmlNode.SelectNodes("//p");
                    if (pnotes != null)
                    {
                        foreach (var pnote in pnotes)
                        {
                            if (pnote.Attributes["style"] != null)
                            {
                                removeStyle(pnote);
                            }

                        }

                    }

                }
            }
            
            if (result.DocumentNode.SelectNodes("//span") != null)
            {
                foreach (HtmlNode htmlNode in result.DocumentNode.SelectNodes("//span"))
                {
                    removeStyle(htmlNode);
                }
            }
            if (result.DocumentNode.SelectNodes("//a") != null)
            {
                foreach (HtmlNode htmlNode in result.DocumentNode.SelectNodes("//a"))
                {
                    string innerTexta = getInnerText(htmlNode);
                    if (string.IsNullOrEmpty(innerTexta))
                    {
                        nodeDelete.Add(htmlNode);
                    }
                    else
                    {
                        removeStyle(htmlNode);
                        htmlNode.Name = "span";
                    }
                }
            }
            foreach (var node in nodeDelete)
            {
                node.Remove();
            }
            return result;
        }
        public static void removeUnknowTag(HtmlDocument doc)
        {
            var allNode = doc.DocumentNode.Descendants();
            List<HtmlNode> nodeDelete = new List<HtmlNode>();
            string removePrivite = ",div,a,span,p,img";
            string removeTag = ",meta,link,style,title,";
            if (allNode != null)
            {
                foreach (var node in allNode)
                {
                    if (node.NodeType == HtmlNodeType.Comment)
                    {
                        nodeDelete.Add(node);
                    }
                    if (!removePrivite.Contains("," + node.Name + ","))
                    {
                        removeStyle(node);
                    }

                    if (node.Name.Contains(":"))
                    {

                        if (string.IsNullOrEmpty(node.InnerHtml))
                        {
                            nodeDelete.Add(node);
                        }
                        else
                        {
                            node.Name = "span";
                        }

                    }
                    else if (node.Name.ToLower().StartsWith("h") && node.Name.Length == 2)
                    {

                        if (string.IsNullOrEmpty(node.InnerHtml))
                        {
                            nodeDelete.Add(node);
                        }
                        else
                        {
                            node.Name = "p";
                        }

                    }
                    else
                    {
                        if (removeTag.Contains("," + node.Name + ","))
                        {
                            nodeDelete.Add(node);
                        }
                    }
                }
            }
            foreach (var node in nodeDelete)
            {
                node.Remove();
            }

        }
        public static string encodeGreaterAndLitter(string text)
        {
            return text.Replace("&lt;", "[LITTERENCODE]").Replace("&gt;", "[GRETERENCODE]").Replace("&nbsp;", "[SPACECODE]");

        }
        public static string decodeGreaterAndLitter(string text)
        {
            return text.Replace("[LITTERENCODE]", "&lt;").Replace("[GRETERENCODE]", "&gt;").Replace("[SPACECODE]", "&nbsp;");

        }
        public static void removeStyle(HtmlNode htmlNode)
        {
            string styleToKeep = "";
            string[] whiteListStyle = { "text-align", "text-indent", "color", "font-weight", "font-style",
                "width", "height", "border", "border-left", "border-right", "border-top", "border-bottom","font-family" };
            if (htmlNode != null)
            {
                //decode
                htmlNode.InnerHtml = decodeGreaterAndLitter(System.Net.WebUtility.HtmlDecode(encodeGreaterAndLitter(htmlNode.InnerHtml)));

                //end decode
                if (htmlNode.Attributes["style"] != null)
                {
                    foreach (string styleName in whiteListStyle)
                    {
                        string styleValue = getStyle(htmlNode, styleName);
                        if (!string.IsNullOrEmpty(styleValue))
                        {

                            if (styleName == "text-indent")
                            {
                                float valueFloat = 0;
                                string styleValueString = styleValue.Replace("px", "").Replace("in", "").Replace("pt", "").Trim();
                                var isFloat = float.TryParse(styleValueString, out valueFloat);
                                if (valueFloat <= 0 && isFloat)
                                {
                                    string unitStyle = styleValue.Replace(styleValueString, "").Trim();
                                    string styleValueMargin = getStyle(htmlNode, "margin-left");
                                    float valueFloatMargin = 0;

                                    string styleValueStringMargin = styleValueMargin.Replace("px", "").Replace("in", "").Replace("pt", "").Trim();
                                    var isFloatMargin = float.TryParse(styleValueStringMargin, out valueFloatMargin);
                                    if (isFloatMargin)
                                    {
                                        valueFloat += valueFloatMargin;
                                    }
                                    if (valueFloat > 0)
                                    {
                                        styleValue = valueFloat.ToString() + unitStyle;
                                    }
                                }
                                if (valueFloat <= 0 && isFloat)
                                    continue;
                            }
                            if (styleName == "font-weight")
                            {
                                if (styleValue.Contains("normal"))
                                    continue;
                            }
                            if (styleName == "font-style")
                            {
                                if (styleValue.Contains("normal"))
                                    continue;
                            }
                            if (styleName == "color")
                            {
                                if (styleValue.Contains("blue") || styleValue.Contains("black"))
                                    continue;
                            }
                            if (styleName == "font-family")
                            {
                                if (!styleValue.ToLower().Contains("symbol") && !styleValue.ToLower().Contains("wingdings"))
                                    continue;
                            }
                            //đổi width của table thành 100%
                            if (htmlNode.Name.ToLower().Contains("table"))
                            {
                                if (styleName.ToLower() == "width")
                                {
                                    if (string.IsNullOrEmpty(styleToKeep))
                                    {
                                        styleToKeep = styleName + ":100%";
                                    }
                                    else
                                    {
                                        styleToKeep += ";" + styleName + ":100%";
                                    }
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(styleToKeep))
                                    {
                                        styleToKeep = styleName + ":" + styleValue;
                                    }
                                    else
                                    {
                                        styleToKeep += ";" + styleName + ":" + styleValue;
                                    }
                                }

                            }
                            else if (htmlNode.Name.ToLower().Contains("span"))
                            {
                                if (styleName.ToLower() == "width")
                                {
                                    string valueDisplay = getStyle(htmlNode, "display");
                                    if (valueDisplay.Contains("inline"))
                                    {
                                        htmlNode.InnerHtml += " ";
                                    }
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(styleToKeep))
                                    {
                                        styleToKeep = styleName + ":" + styleValue;
                                    }
                                    else
                                    {
                                        styleToKeep += ";" + styleName + ":" + styleValue;
                                    }
                                }

                            }
                            else
                            {
                                if (string.IsNullOrEmpty(styleToKeep))
                                {
                                    styleToKeep = styleName + ":" + styleValue;
                                }
                                else
                                {
                                    styleToKeep += ";" + styleName + ":" + styleValue;
                                }
                            }
                        }

                    }
                }
                if (!string.IsNullOrEmpty(styleToKeep))
                {
                    htmlNode.Attributes.Remove("style");
                    htmlNode.Attributes.Add("style", styleToKeep);
                }
                else if (htmlNode.Attributes["style"] != null)
                {
                    htmlNode.Attributes.Remove("style");
                }
                //remove word attribute
                if (htmlNode.Attributes["class"] != null)
                {
                    var classes = htmlNode.Attributes["class"].Value;
                    if (!classes.Contains("demuc"))
                    {
                        htmlNode.Attributes.Remove("class");
                    }

                }
                if (htmlNode.Attributes["lang"] != null)
                {
                    htmlNode.Attributes.Remove("lang");
                }
                if (htmlNode.Attributes["dir"] != null)
                {
                    htmlNode.Attributes.Remove("dir");
                }
                if (htmlNode.Attributes["xml:space"] != null)
                {
                    htmlNode.Attributes.Remove("xml:space");
                }
                if (htmlNode.Attributes["align"] != null)
                {
                    string align = htmlNode.Attributes["align"].Value;
                    if (htmlNode.Name.ToLower() == "td" || htmlNode.Name.ToLower() == "table")
                    {
                        //keep att
                    }
                    else if (htmlNode.Name.ToLower() == "p")
                    {
                        if (htmlNode.Attributes["style"] != null)
                        {
                            htmlNode.Attributes["style"].Value = htmlNode.Attributes["style"].Value + ";text-align:" + align;
                        }
                        else
                        {
                            htmlNode.Attributes.Append("style", "text-align:" + align);
                        }
                    }
                    else
                    {
                        htmlNode.Attributes.Remove("align");
                    }

                }
            }

        }
        public static string getStyle(HtmlNode htmlNode, string styleName)
        {
            string styleValue = "";
            if (htmlNode != null && !string.IsNullOrEmpty(styleName))
            {
                if (htmlNode.Attributes["style"] != null)
                {
                    string[] styleValues = htmlNode.Attributes["style"].Value.Split(';');
                    foreach (string value in styleValues)
                    {
                        if ((value.Contains(styleName + ":") || value.Contains(styleName + " :")) && !value.Contains("-" + styleName))
                        {
                            styleValue = value.Replace(styleName + ":", "").Replace(styleName + " :", "");
                            break;
                        }
                    }
                }
            }
            return styleValue;
        }
    }
}
