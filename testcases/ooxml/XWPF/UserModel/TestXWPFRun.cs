/* ====================================================================
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for Additional information regarding copyright ownership.
   The ASF licenses this file to You under the Apache License, Version 2.0
   (the "License"); you may not use this file except in compliance with
   the License.  You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
==================================================================== */
namespace NPOI.XWPF.UserModel
{
    using System.Collections.Generic;
    using System.IO;
    using NUnit.Framework;
    using NPOI.OpenXmlFormats.Wordprocessing;
    using NPOI.XWPF;
    using NPOI.XWPF.Model;

    /**
     * Tests for XWPF Run
     */
    [TestFixture]
    public class TestXWPFRun
    {

        public CT_R ctRun;
        public XWPFParagraph p;
        [SetUp]
        public void SetUp()
        {
            XWPFDocument doc = new XWPFDocument();
            p = doc.CreateParagraph();

            this.ctRun = new CT_R();

        }

        [Test]
        public void TestSetGetText()
        {
            ctRun.AddNewT().Value = ("TEST STRING");
            ctRun.AddNewT().Value = ("TEST2 STRING");
            ctRun.AddNewT().Value = ("TEST3 STRING");

            Assert.AreEqual(3, ctRun.SizeOfTArray());
            XWPFRun run = new XWPFRun(ctRun, p);

            Assert.AreEqual("TEST2 STRING", run.GetText(1));

            run.SetText("NEW STRING", 0);
            Assert.AreEqual("NEW STRING", run.GetText(0));

            //Run.Text=("xxx",14);
            //Assert.Fail("Position wrong");
        }

        [Test]
        public void TestSetGetBold()
        {
            CT_RPr rpr = ctRun.AddNewRPr();
            rpr.AddNewB().val = (ST_OnOff.True);

            XWPFRun run = new XWPFRun(ctRun, p);
            Assert.AreEqual(true, run.IsBold());

            run.SetBold(false);
            Assert.AreEqual(false, run.IsBold());
            Assert.AreEqual(ST_OnOff.False, rpr.b.val);
        }

        [Test]
        public void TestSetGetItalic()
        {
            CT_RPr rpr = ctRun.AddNewRPr();
            rpr.AddNewI().val = (ST_OnOff.True);

            XWPFRun run = new XWPFRun(ctRun, p);
            Assert.AreEqual(true, run.IsItalic());

            run.SetItalic(false);
            Assert.AreEqual(ST_OnOff.False, rpr.i.val);
        }

        [Test]
        public void TestSetGetStrike()
        {
            CT_RPr rpr = ctRun.AddNewRPr();
            rpr.AddNewStrike().val = (ST_OnOff.True);

            XWPFRun run = new XWPFRun(ctRun, p);
            Assert.AreEqual(true, run.IsStrike());

            run.SetStrike(false);
            Assert.AreEqual(ST_OnOff.False, rpr.strike.val);
        }

        [Test]
        public void TestSetGetUnderline()
        {
            CT_RPr rpr = ctRun.AddNewRPr();
            rpr.AddNewU().val = (ST_Underline.dash);

            XWPFRun run = new XWPFRun(ctRun, p);
            Assert.AreEqual(UnderlinePatterns.Dash, run.GetUnderline());

            run.SetUnderline(UnderlinePatterns.None);
            Assert.AreEqual(ST_Underline.none, rpr.u.val);
        }


        [Test]
        public void TestSetGetVAlign()
        {
            CT_RPr rpr = ctRun.AddNewRPr();
            rpr.AddNewVertAlign().val = (ST_VerticalAlignRun.subscript);

            XWPFRun run = new XWPFRun(ctRun, p);
            Assert.AreEqual(VerticalAlign.SUBSCRIPT, run.GetSubscript());

            run.SetSubscript(VerticalAlign.BASELINE);
            Assert.AreEqual(ST_VerticalAlignRun.baseline, rpr.vertAlign.val);
        }


        [Test]
        public void TestSetGetFontFamily()
        {
            CT_RPr rpr = ctRun.AddNewRPr();
            rpr.AddNewRFonts().ascii = ("Times New Roman");

            XWPFRun run = new XWPFRun(ctRun, p);
            Assert.AreEqual("Times New Roman", run.GetFontFamily());

            run.SetFontFamily("Verdana");
            Assert.AreEqual("Verdana", rpr.rFonts.ascii);
        }


        [Test]
        public void TestSetGetFontSize()
        {
            CT_RPr rpr = ctRun.AddNewRPr();
            rpr.AddNewSz().val = 14;

            XWPFRun run = new XWPFRun(ctRun, p);
            Assert.AreEqual(7, run.GetFontSize());

            run.SetFontSize(24);
            Assert.AreEqual(48, (int)rpr.sz.val);
        }


        [Test]
        public void TestSetGetTextForegroundBackground()
        {
            CT_RPr rpr = ctRun.AddNewRPr();
            rpr.AddNewPosition().val = "4000";

            XWPFRun run = new XWPFRun(ctRun, p);
            Assert.AreEqual(4000, run.GetTextPosition());

            run.SetTextPosition(2400);
            Assert.AreEqual(2400, int.Parse(rpr.position.val));
        }

        [Test]
        public void TestAddCarriageReturn()
        {

            ctRun.AddNewT().Value = ("TEST STRING");
            ctRun.AddNewCr();
            ctRun.AddNewT().Value = ("TEST2 STRING");
            ctRun.AddNewCr();
            ctRun.AddNewT().Value = ("TEST3 STRING");
            Assert.AreEqual(2, ctRun.SizeOfCrArray());

            XWPFRun run = new XWPFRun(new CT_R(), p);
            run.SetText("T1");
            run.AddCarriageReturn();
            run.AddCarriageReturn();
            run.SetText("T2");
            run.AddCarriageReturn();
            Assert.AreEqual(3, run.GetCTR().GetCrList().Count);

        }

        [Test]
        public void TestAddPageBreak()
        {
            ctRun.AddNewT().Value = ("TEST STRING");
            ctRun.AddNewBr();
            ctRun.AddNewT().Value = ("TEST2 STRING");
            CT_Br breac = ctRun.AddNewBr();
            breac.clear = (ST_BrClear.left);
            ctRun.AddNewT().Value = ("TEST3 STRING");
            Assert.AreEqual(2, ctRun.SizeOfBrArray());

            XWPFRun run = new XWPFRun(new CT_R(), p);
            run.SetText("TEXT1");
            run.AddBreak();
            run.SetText("TEXT2");
            run.AddBreak(BreakType.TEXTWRAPPING);
            Assert.AreEqual(2, run.GetCTR().SizeOfBrArray());
        }

        /**
         * Test that on an existing document, we do the
         *  right thing with it
         * @throws IOException 
         */
        [Test]
        public void TestExisting()
        {
            XWPFDocument doc = XWPFTestDataSamples.OpenSampleDocument("TestDocument.docx");
            XWPFParagraph p;
            XWPFRun run;


            // First paragraph is simple
            p = doc.GetParagraphArray(0);
            Assert.AreEqual("This is a test document.", p.GetText());
            Assert.AreEqual(2, p.GetRuns().Count);

            run = p.GetRuns()[0];
            Assert.AreEqual("This is a test document", run.ToString());
            Assert.AreEqual(false, run.IsBold());
            Assert.AreEqual(false, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());
            Assert.AreEqual(null, run.GetCTR().rPr);

            run = p.GetRuns()[1];
            Assert.AreEqual(".", run.ToString());
            Assert.AreEqual(false, run.IsBold());
            Assert.AreEqual(false, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());
            Assert.AreEqual(null, run.GetCTR().rPr);


            // Next paragraph is all in one style, but a different one
            p = doc.GetParagraphArray(1);
            Assert.AreEqual("This bit is in bold and italic", p.GetText());
            Assert.AreEqual(1, p.GetRuns().Count);

            run = p.GetRuns()[0];
            Assert.AreEqual("This bit is in bold and italic", run.ToString());
            Assert.AreEqual(true, run.IsBold());
            Assert.AreEqual(true, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());
            Assert.AreEqual(true, run.GetCTR().rPr.IsSetB());
            Assert.AreEqual(false, run.GetCTR().rPr.b.IsSetVal());


            // Back to normal
            p = doc.GetParagraphArray(2);
            Assert.AreEqual("Back to normal", p.GetText());
            Assert.AreEqual(1, p.GetRuns().Count);

            run = p.GetRuns()[(0)];
            Assert.AreEqual("Back to normal", run.ToString());
            Assert.AreEqual(false, run.IsBold());
            Assert.AreEqual(false, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());
            Assert.AreEqual(null, run.GetCTR().rPr);


            // Different styles in one paragraph
            p = doc.GetParagraphArray(3);
            Assert.AreEqual("This contains BOLD, ITALIC and BOTH, as well as RED and YELLOW text.", p.GetText());
            Assert.AreEqual(11, p.GetRuns().Count);

            run = p.GetRuns()[(0)];
            Assert.AreEqual("This contains ", run.ToString());
            Assert.AreEqual(false, run.IsBold());
            Assert.AreEqual(false, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());
            Assert.AreEqual(null, run.GetCTR().rPr);

            run = p.GetRuns()[(1)];
            Assert.AreEqual("BOLD", run.ToString());
            Assert.AreEqual(true, run.IsBold());
            Assert.AreEqual(false, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());

            run = p.GetRuns()[2];
            Assert.AreEqual(", ", run.ToString());
            Assert.AreEqual(false, run.IsBold());
            Assert.AreEqual(false, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());
            Assert.AreEqual(null, run.GetCTR().rPr);

            run = p.GetRuns()[(3)];
            Assert.AreEqual("ITALIC", run.ToString());
            Assert.AreEqual(false, run.IsBold());
            Assert.AreEqual(true, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());

            run = p.GetRuns()[(4)];
            Assert.AreEqual(" and ", run.ToString());
            Assert.AreEqual(false, run.IsBold());
            Assert.AreEqual(false, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());
            Assert.AreEqual(null, run.GetCTR().rPr);

            run = p.GetRuns()[(5)];
            Assert.AreEqual("BOTH", run.ToString());
            Assert.AreEqual(true, run.IsBold());
            Assert.AreEqual(true, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());

            run = p.GetRuns()[(6)];
            Assert.AreEqual(", as well as ", run.ToString());
            Assert.AreEqual(false, run.IsBold());
            Assert.AreEqual(false, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());
            Assert.AreEqual(null, run.GetCTR().rPr);

            run = p.GetRuns()[(7)];
            Assert.AreEqual("RED", run.ToString());
            Assert.AreEqual(false, run.IsBold());
            Assert.AreEqual(false, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());

            run = p.GetRuns()[(8)];
            Assert.AreEqual(" and ", run.ToString());
            Assert.AreEqual(false, run.IsBold());
            Assert.AreEqual(false, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());
            Assert.AreEqual(null, run.GetCTR().rPr);

            run = p.GetRuns()[(9)];
            Assert.AreEqual("YELLOW", run.ToString());
            Assert.AreEqual(false, run.IsBold());
            Assert.AreEqual(false, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());

            run = p.GetRuns()[(10)];
            Assert.AreEqual(" text.", run.ToString());
            Assert.AreEqual(false, run.IsBold());
            Assert.AreEqual(false, run.IsItalic());
            Assert.AreEqual(false, run.IsStrike());
            Assert.AreEqual(null, run.GetCTR().rPr);
        }

        [Test]
        public void TestPictureInHeader()
        {
            XWPFDocument sampleDoc = XWPFTestDataSamples.OpenSampleDocument("headerPic.docx");
            XWPFHeaderFooterPolicy policy = sampleDoc.GetHeaderFooterPolicy();

            XWPFHeader header = policy.GetDefaultHeader();

            int count = 0;

            foreach (XWPFParagraph p in header.Paragraphs)
            {
                foreach (XWPFRun r in p.GetRuns())
                {
                    List<XWPFPicture> pictures = r.GetEmbeddedPictures();

                    foreach (XWPFPicture pic in pictures)
                    {
                        Assert.IsNotNull(pic.GetPictureData());
                        Assert.AreEqual("DOZOR", pic.GetDescription());
                    }

                    count += pictures.Count;
                }
            }

            Assert.AreEqual(1, count);
        }

        [Test]
        public void TestAddPicture()
        {
            XWPFDocument doc = XWPFTestDataSamples.OpenSampleDocument("TestDocument.docx");
            XWPFParagraph p = doc.GetParagraphArray(2);
            XWPFRun r = p.GetRuns()[0];

            Assert.AreEqual(0, doc.AllPictures.Count);
            Assert.AreEqual(0, r.GetEmbeddedPictures().Count);

            r.AddPicture(new MemoryStream(new byte[0]), (int)PictureType.JPEG, "test.jpg", 21, 32);

            Assert.AreEqual(1, doc.AllPictures.Count);
            Assert.AreEqual(1, r.GetEmbeddedPictures().Count);
        }
    }
}