/*
* Licensed to the Apache Software Foundation (ASF) under one or more
* contributor license agreements.  See the NOTICE file distributed with
* this work for Additional information regarding copyright ownership.
* The ASF licenses this file to You under the Apache License, Version 2.0
* (the "License"); you may not use this file except in compliance with
* the License.  You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
namespace TestCases.HSSF.UserModel
{
    using NPOI.HSSF.UserModel;
    using NUnit.Framework;

    using NPOI.SS.UserModel;
    using TestCases.SS.UserModel;
    using NPOI.Util;
    using System.Collections.Generic;
    using System.Collections;

    /**
     * Test <c>HSSFPicture</c>.
     *
     * @author Yegor Kozlov (yegor at apache.org)
     */
    [TestFixture]
    public class TestHSSFPicture : BaseTestPicture
    {
        public TestHSSFPicture()
            : base(HSSFITestDataProvider.Instance)
        {

        }

        [Test]
        public void TestResize()
        {
            BaseTestResize(new HSSFClientAnchor(0, 0, 848, 240, (short)0, 0, (short)1, 9));
        }

        /**
         * Bug # 45829 reported ArithmeticException (/ by zero) when resizing png with zero DPI.
         */
        [Test]
        public void Test45829()
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sh1 = wb.CreateSheet();
            IDrawing p1 = sh1.CreateDrawingPatriarch();

            byte[] pictureData = HSSFTestDataSamples.GetTestDataFileContent("45829.png");
            int idx1 = wb.AddPicture(pictureData, PictureType.PNG);
            IPicture pic = p1.CreatePicture(new HSSFClientAnchor(), idx1);
            pic.Resize();
        }
        [Test]
        public void TestAddPictures()
        {
            IWorkbook wb = new HSSFWorkbook();

            ISheet sh = wb.CreateSheet("Pictures");
            IDrawing dr = sh.CreateDrawingPatriarch();
            Assert.AreEqual(0, ((HSSFPatriarch)dr).Children.Count);
            IClientAnchor anchor = wb.GetCreationHelper().CreateClientAnchor();

            //register a picture
            byte[] data1 = new byte[] { 1, 2, 3 };
            int idx1 = wb.AddPicture(data1, PictureType.JPEG);
            Assert.AreEqual(1, idx1);
            IPicture p1 = dr.CreatePicture(anchor, idx1);
            Assert.IsTrue(Arrays.Equals(data1, ((HSSFPicture)p1).PictureData.Data));

            // register another one
            byte[] data2 = new byte[] { 4, 5, 6 };
            int idx2 = wb.AddPicture(data2, PictureType.JPEG);
            Assert.AreEqual(2, idx2);
            IPicture p2 = dr.CreatePicture(anchor, idx2);
            Assert.AreEqual(2, ((HSSFPatriarch)dr).Children.Count);
            Assert.IsTrue(Arrays.Equals(data2, ((HSSFPicture)p2).PictureData.Data));

            // confirm that HSSFPatriarch.Children returns two picture shapes 
            Assert.IsTrue(Arrays.Equals(data1, ((HSSFPicture)((HSSFPatriarch)dr).Children[(0)]).PictureData.Data));
            Assert.IsTrue(Arrays.Equals(data2, ((HSSFPicture)((HSSFPatriarch)dr).Children[(1)]).PictureData.Data));

            // Write, read back and verify that our pictures are there
            wb = HSSFTestDataSamples.WriteOutAndReadBack((HSSFWorkbook)wb);
            IList lst2 = wb.GetAllPictures();
            Assert.AreEqual(2, lst2.Count);
            Assert.IsTrue(Arrays.Equals(data1, (lst2[(0)] as HSSFPictureData).Data));
            Assert.IsTrue(Arrays.Equals(data2, (lst2[(1)] as HSSFPictureData).Data));

            // confirm that the pictures are in the Sheet's Drawing
            sh = wb.GetSheet("Pictures");
            dr = sh.CreateDrawingPatriarch();
            Assert.AreEqual(2, ((HSSFPatriarch)dr).Children.Count);
            Assert.IsTrue(Arrays.Equals(data1, ((HSSFPicture)((HSSFPatriarch)dr).Children[(0)]).PictureData.Data));
            Assert.IsTrue(Arrays.Equals(data2, ((HSSFPicture)((HSSFPatriarch)dr).Children[(1)]).PictureData.Data));

            // add a third picture
            byte[] data3 = new byte[] { 7, 8, 9 };
            // picture index must increment across Write-read
            int idx3 = wb.AddPicture(data3, PictureType.JPEG);
            Assert.AreEqual(3, idx3);
            IPicture p3 = dr.CreatePicture(anchor, idx3);
            Assert.IsTrue(Arrays.Equals(data3, ((HSSFPicture)p3).PictureData.Data));
            Assert.AreEqual(3, ((HSSFPatriarch)dr).Children.Count);
            Assert.IsTrue(Arrays.Equals(data1, ((HSSFPicture)((HSSFPatriarch)dr).Children[(0)]).PictureData.Data));
            Assert.IsTrue(Arrays.Equals(data2, ((HSSFPicture)((HSSFPatriarch)dr).Children[(1)]).PictureData.Data));
            Assert.IsTrue(Arrays.Equals(data3, ((HSSFPicture)((HSSFPatriarch)dr).Children[(2)]).PictureData.Data));

            // write and read again
            wb = HSSFTestDataSamples.WriteOutAndReadBack((HSSFWorkbook)wb);
            IList lst3 = wb.GetAllPictures();
            // all three should be there
            Assert.AreEqual(3, lst3.Count);
            Assert.IsTrue(Arrays.Equals(data1, (lst3[(0)] as HSSFPictureData).Data));
            Assert.IsTrue(Arrays.Equals(data2, (lst3[(1)] as HSSFPictureData).Data));
            Assert.IsTrue(Arrays.Equals(data3, (lst3[(2)] as HSSFPictureData).Data));

            sh = wb.GetSheet("Pictures");
            dr = sh.CreateDrawingPatriarch();
            Assert.AreEqual(3, ((HSSFPatriarch)dr).Children.Count);

            // forth picture
            byte[] data4 = new byte[] { 10, 11, 12 };
            int idx4 = wb.AddPicture(data4, PictureType.JPEG);
            Assert.AreEqual(4, idx4);
            dr.CreatePicture(anchor, idx4);
            Assert.AreEqual(4, ((HSSFPatriarch)dr).Children.Count);
            Assert.IsTrue(Arrays.Equals(data1, ((HSSFPicture)((HSSFPatriarch)dr).Children[(0)]).PictureData.Data));
            Assert.IsTrue(Arrays.Equals(data2, ((HSSFPicture)((HSSFPatriarch)dr).Children[(1)]).PictureData.Data));
            Assert.IsTrue(Arrays.Equals(data3, ((HSSFPicture)((HSSFPatriarch)dr).Children[(2)]).PictureData.Data));
            Assert.IsTrue(Arrays.Equals(data4, ((HSSFPicture)((HSSFPatriarch)dr).Children[(3)]).PictureData.Data));

            wb = HSSFTestDataSamples.WriteOutAndReadBack((HSSFWorkbook)wb);
            IList lst4 = wb.GetAllPictures();
            Assert.AreEqual(4, lst4.Count);
            Assert.IsTrue(Arrays.Equals(data1, (lst4[(0)] as HSSFPictureData).Data));
            Assert.IsTrue(Arrays.Equals(data2, (lst4[(1)] as HSSFPictureData).Data));
            Assert.IsTrue(Arrays.Equals(data3, (lst4[(2)] as HSSFPictureData).Data));
            Assert.IsTrue(Arrays.Equals(data4, (lst4[(3)] as HSSFPictureData).Data));
            sh = wb.GetSheet("Pictures");
            dr = sh.CreateDrawingPatriarch();
            Assert.AreEqual(4, ((HSSFPatriarch)dr).Children.Count);
            Assert.IsTrue(Arrays.Equals(data1, ((HSSFPicture)((HSSFPatriarch)dr).Children[(0)]).PictureData.Data));
            Assert.IsTrue(Arrays.Equals(data2, ((HSSFPicture)((HSSFPatriarch)dr).Children[(1)]).PictureData.Data));
            Assert.IsTrue(Arrays.Equals(data3, ((HSSFPicture)((HSSFPatriarch)dr).Children[(2)]).PictureData.Data));
            Assert.IsTrue(Arrays.Equals(data4, ((HSSFPicture)((HSSFPatriarch)dr).Children[(3)]).PictureData.Data));
        }
    }
}