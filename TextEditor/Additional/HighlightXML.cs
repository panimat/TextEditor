using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace TextEditor.Additional
{

    public static class HighlightXML
    {
        public static Color HC_NODE = Color.FromRgb(255, 0, 243);
        public static Color HC_STRING = Color.FromRgb(6, 17, 255);
        public static Color HC_ATTRIBUTE = Color.FromRgb(255, 0, 0);
        public static Color HC_COMMENT = Color.FromRgb(35, 255, 0);
        public static Color HC_INNERTEXT = Color.FromRgb(0, 0, 0);


        public static void HighlighttextXML(RichTextBox rtb)
        {
            int k = 0;

            TextRange tr = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);

            string str = tr.Text;

            int st, en;
            int lasten = -1;
            while (k < str.Length)
            {
                st = str.IndexOf('<', k);

                if (st < 0)
                    break;

                if (lasten > 0)
                {
                    TextPointer myTextPointer1 = rtb.Document.ContentStart.GetPositionAtOffset(lasten + 1);
                    TextPointer myTextPointer2 = rtb.Document.ContentEnd.GetPositionAtOffset(st);

                    tr.Select(myTextPointer1, myTextPointer2);
                    tr.ApplyPropertyValue(Inline.ForegroundProperty, new SolidColorBrush(HighlightXML.HC_INNERTEXT));
                }

                en = str.IndexOf('>', st + 1);
                if (en < 0)
                    break;

                k = en + 1;
                lasten = en;

                if (str[st + 1] == '!')
                {
                    TextPointer myTextPointer1 = rtb.Document.ContentStart.GetPositionAtOffset(st + 1);
                    TextPointer myTextPointer2 = rtb.Document.ContentStart.GetPositionAtOffset(en - st - 1);

                    rtb.Selection.Select(myTextPointer1, myTextPointer2);
                    rtb.Selection.ApplyPropertyValue(Inline.ForegroundProperty, new SolidColorBrush(HighlightXML.HC_COMMENT));

                    //rtb.Selection.Select(st + 1, en - st - 1);
                    //rtb.SelectionColor = HighlightXML.HC_COMMENT;
                    continue;

                }
                String nodeText = str.Substring(st + 1, en - st - 1);


                bool inString = false;

                int lastSt = -1;
                int state = 0;
                /* 0 = before node name
                 * 1 = in node name
                   2 = after node name
                   3 = in attribute
                   4 = in string
                   */
                int startNodeName = 0, startAtt = 0;
                for (int i = 0; i < nodeText.Length; ++i)
                {
                    if (nodeText[i] == '"')
                        inString = !inString;

                    if (inString && nodeText[i] == '"')
                        lastSt = i;
                    else
                        if (nodeText[i] == '"')
                    {
                        TextPointer myTextPointer1 = rtb.Document.ContentStart.GetPositionAtOffset(lastSt + st + 2);
                        TextPointer myTextPointer2 = rtb.Document.ContentStart.GetPositionAtOffset(i - lastSt - 1);

                        rtb.Selection.Select(myTextPointer1, myTextPointer2);
                        rtb.Selection.ApplyPropertyValue(Inline.ForegroundProperty, new SolidColorBrush(HighlightXML.HC_STRING));

                        //rtb.Select(lastSt + st + 2, i - lastSt - 1);
                        //rtb.SelectionColor = HighlightXML.HC_STRING;
                    }

                    switch (state)
                    {
                        case 0:
                            if (!Char.IsWhiteSpace(nodeText, i))
                            {
                                startNodeName = i;
                                state = 1;
                            }
                            break;
                        case 1:
                            if (Char.IsWhiteSpace(nodeText, i))
                            {
                                TextPointer myTextPointer1 = rtb.Document.ContentStart.GetPositionAtOffset(startNodeName + st);
                                TextPointer myTextPointer2 = rtb.Document.ContentStart.GetPositionAtOffset(i - startNodeName + 1);

                                rtb.Selection.Select(myTextPointer1, myTextPointer2);
                                rtb.Selection.ApplyPropertyValue(Inline.ForegroundProperty, new SolidColorBrush(HighlightXML.HC_NODE));

                                //rtb.Select(startNodeName + st, i - startNodeName + 1);
                                //rtb.SelectionColor = HighlightXML.HC_NODE;

                                state = 2;
                            }
                            break;
                        case 2:
                            if (!Char.IsWhiteSpace(nodeText, i))
                            {
                                startAtt = i;
                                state = 3;
                            }
                            break;

                        case 3:
                            if (Char.IsWhiteSpace(nodeText, i) || nodeText[i] == '=')
                            {
                                TextPointer myTextPointer1 = rtb.Document.ContentStart.GetPositionAtOffset(startAtt + st);
                                TextPointer myTextPointer2 = rtb.Document.ContentStart.GetPositionAtOffset(i - startAtt + 1);

                                rtb.Selection.Select(myTextPointer1, myTextPointer2);
                                rtb.Selection.ApplyPropertyValue(Inline.ForegroundProperty, new SolidColorBrush(HighlightXML.HC_NODE));
                                                               
                                //rtb.Select(startAtt + st, i - startAtt + 1);
                                //rtb.SelectionColor = HighlightXML.HC_ATTRIBUTE;

                                state = 4;
                            }
                            break;
                        case 4:
                            if (nodeText[i] == '"' && !inString)
                                state = 2;
                            break;


                    }

                }
                if (state == 1)
                {
                    TextPointer myTextPointer1 = rtb.Document.ContentStart.GetPositionAtOffset(st + 1);
                    TextPointer myTextPointer2 = rtb.Document.ContentStart.GetPositionAtOffset(nodeText.Length);

                    rtb.Selection.Select(myTextPointer1, myTextPointer2);
                    rtb.Selection.ApplyPropertyValue(Inline.ForegroundProperty, new SolidColorBrush(HighlightXML.HC_NODE));

                    //rtb.Select(st + 1, nodeText.Length);
                    //rtb.SelectionColor = HighlightXML.HC_NODE;
                }
            }
        }
    }
}
