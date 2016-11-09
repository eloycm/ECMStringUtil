using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECMStringUtil.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace ECMStringUtil.Extensions.Tests
{
    [TestClass()]
    public class CoreExtensionsTests
    {
        /// <summary>
        ///A test for EncryptString
        ///</summary>
        [TestMethod()]
        public void EncryptStringTest()
        {
            string plainText = "test";
            string expected = "ZOVtsFv5YFVpQ3wJMB6nug=="; // TODO: Initialize to an appropriate value
            string actual;
            actual = CoreExtensions.EncryptString(plainText);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for DecryptString
        ///</summary>
        [TestMethod()]
        public void DecryptStringTest()
        {
            string cipherText = "ZOVtsFv5YFVpQ3wJMB6nug==";
            string expected = "test";
            string actual;
            actual = CoreExtensions.DecryptString(cipherText);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for VerticalResponseformCleanUp
        ///</summary>
        [TestMethod()]
        public void VerticalResponseformCleanUpTest()
        {
            StringBuilder sb = new StringBuilder(1646);
            sb.AppendLine(@"<form method=""post"" action=""http://oi.vresp.com?fid=28aa4138db"" target=""vr_optin_popup"" onsubmit=""window.open( 'http://www.verticalresponse.com', 'vr_optin_popup', 'scrollbars=yes,width=600,height=450' ); return true;"" >");
            sb.AppendLine(@"");
            sb.AppendLine(@"  <div style=""font-family: verdana; font-size: 11px; width: 160px; padding: 10px; border: 1px solid #000000; background: #dddddd;"">");
            sb.AppendLine(@"    <strong><span style=""color: #333333;"">Sign Up Today!</span></strong>");
            sb.AppendLine(@"    <p style=""text-align: right; margin-top: 10px; margin-bottom: 10px;""><span style=""color: #f00;"">* </span><span style=""color: #333333"">required</span></p>");
            sb.AppendLine(@"    <label style=""color: #333333;"">Email Address:</label>    <span style=""color: #f00"">* </span>");
            sb.AppendLine(@"    <br/>");
            sb.AppendLine(@"    <input name=""email_address"" size=""15"" style=""margin-top: 5px; margin-bottom: 5px; border: 1px solid #999; padding: 3px;""/>");
            sb.AppendLine(@"    <br/>");
            sb.AppendLine(@"    <label style=""color: #333333;"">First Name:</label>    <span style=""color: #f00"">* </span>");
            sb.AppendLine(@"    <br/>");
            sb.AppendLine(@"    <input name=""first_name"" size=""15"" style=""margin-top: 5px; margin-bottom: 5px; border: 1px solid #999; padding: 3px;""/>");
            sb.AppendLine(@"    <br/>");
            sb.AppendLine(@"    <label style=""color: #333333;"">Last Name:</label>    <span style=""color: #f00"">* </span>");
            sb.AppendLine(@"    <br/>");
            sb.AppendLine(@"    <input name=""last_name"" size=""15"" style=""margin-top: 5px; margin-bottom: 5px; border: 1px solid #999; padding: 3px;""/>");
            sb.AppendLine(@"    <br/>");
            sb.AppendLine(@"    <input type=""submit"" value=""Join Now"" style=""margin-top: 5px; border: 1px solid #999; padding: 3px;""/><br/>");
            sb.AppendLine(@"    <br/><span style=""color: #333333""><a title=""Social and Email Marketing by VerticalResponse"" href=""http://www.verticalresponse.com"">Social and Email Marketing by VerticalResponse</a></span>");
            sb.AppendLine(@"  </div>");
            sb.AppendLine(@"</form>");

            string sToclean = sb.ToString(); // 

            string actual = CoreExtensions.VerticalResponseformCleanUp(sToclean);
            Assert.AreEqual(actual.IndexOf("<form"), -1);

        }

        /// <summary>
        ///A test for IsInitialSubstringRepeated
        ///</summary>
        [TestMethod()]
        public void IsInitialSubstringRepeatedTest()
        {
            string source = "http://local.chsl.com/http://local.chsl.com/~/media/files/chsl/sports/boys/lacrosse/honors/2002.ashx";
            string subString = "http://"; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual = CoreExtensions.IsInitialSubstringRepeated(source, subString);
            Assert.AreEqual(expected, actual);

            expected = false;
            source = string.Empty;
            actual = CoreExtensions.IsInitialSubstringRepeated(source, subString);
            Assert.AreEqual(expected, actual);

            subString = "very long string dfsrqewrewqfa;sdlfkjasd;fldkjs asd;lfkjakljrqewoiewjfasd;lfkjasd;flkqewpoiruqewrqwe;lkjdasfoijuqwerqwoeriquweroueir";
            actual = CoreExtensions.IsInitialSubstringRepeated(source, subString);
            Assert.AreEqual(expected, actual);

            subString = string.Empty;
            actual = CoreExtensions.IsInitialSubstringRepeated(source, subString);
            Assert.AreEqual(expected, actual);

            source = null;
            subString = null;
            actual = CoreExtensions.IsInitialSubstringRepeated(source, subString);
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        ///A test for CleanUpMediaUrl
        ///</summary>
        [TestMethod()]
        public void CleanUpMediaUrlTest()
        {
            string mediaUrl = "http://local.chsl.com/http://local.chsl.com/~/media/files/chsl/sports/boys/lacrosse/honors/2002.ashx";
            string expected = "http://local.chsl.com/~/media/files/chsl/sports/boys/lacrosse/honors/2002.ashx";
            string actual;
            actual = CoreExtensions.CleanUpMediaUrl(mediaUrl);
            Assert.AreEqual(expected, actual);

        }
        ///</summary>
        [TestMethod()]
        public void LeftTrimCleanUpMediaUrlTest()
        {
            string mediaUrl = "local.chsl.com/http://local.chsl.com/~/media/files/chsl/sports/boys/lacrosse/honors/2002.ashx";
            string expected = "http://local.chsl.com/~/media/files/chsl/sports/boys/lacrosse/honors/2002.ashx";
            string actual;
            actual = CoreExtensions.CleanUpMediaUrl(mediaUrl);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for IsPoBox
        ///</summary>
        [TestMethod()]
        public void IsPoBoxTest()
        {
            string addr = "555 rc drive";
            bool expected = false;
            bool actual = CoreExtensions.IsPoBox(addr);
            Assert.AreEqual(expected, actual);

            addr = "P.O. Box 222";
            expected = true;

            actual = CoreExtensions.IsPoBox(addr);
            Assert.AreEqual(expected, actual);

            addr = "pobox 333";
            expected = true;

            actual = CoreExtensions.IsPoBox(addr);
            Assert.AreEqual(expected, actual);

            addr = null;
            expected = false;

            actual = CoreExtensions.IsPoBox(addr);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CleanUpMediaUrl
        ///</summary>
        [TestMethod()]
        public void CleanUpMediaUrlTest1()
        {
            string mediaUrl = "https://my.aod.org:443/~/media/extranet/bookkeepers/financial%20report%20forms/2012-13%20financial%20reports%20and%20forms%20current%20year/non-parish%20school%202012-13%20financial%20report%20worksheets.ashx";
            bool isSecureConnection = true; // TODO: Initialize to an appropriate value
            string expected = "https://my.aod.org:443/~/media/extranet/bookkeepers/financial%20report%20forms/2012-13%20financial%20reports%20and%20forms%20current%20year/non-parish%20school%202012-13%20financial%20report%20worksheets.ashx";

            string actual;
            actual = CoreExtensions.CleanUpMediaUrl(mediaUrl, isSecureConnection);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for CleanPortFromUrl
        ///</summary>
        [TestMethod()]
        public void CleanPortFromUrlTest()
        {
            string url = "http://www.example.com:80/dir/?query=test";
            string expected = "http://www.example.com/dir/?query=test";
            string actual;
            actual = CoreExtensions.CleanPortFromUrl(url);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for CleanupAbsoluteUrl
        ///</summary>
        [TestMethod()]
        public void CleanupAbsoluteUrlTest()
        {
            string url = "https://my.aod.org:443/~/media/extranet/bookkeepers/financial%20report%20forms/";
            string expected = "/~/media/extranet/bookkeepers/financial report forms/";
            string actual;
            actual = CoreExtensions.CleanupAbsoluteUrl(url);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CleanStringBefore
        ///</summary>
        [TestMethod()]
        public void CleanStringBeforeTest()
        {
            string s = "asdf;lkjre/http://www.yahoo.com";
            string tk = "http:";
            string expected = "http://www.yahoo.com";
            string actual;
            actual = CoreExtensions.CleanStringBefore(s, tk);
            Assert.AreEqual(expected, actual);

            s = "http://www.yahoo.com";
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for IsGoogleArray
        ///</summary>
        [TestMethod()]
        public void IsGoogleArrayTest()
        {
            string s = "Poor\t\tFair\t\tDon't Know\t\tGood\t\tExcellent\t\n5396\t\t12056\t\t32425\t\t30753\t\t38158\t\n";
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = CoreExtensions.IsGoogleArray(s);
            Assert.AreEqual(expected, actual);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"[");
            sb.AppendLine(@"['Priority', 'Poor', 'Fair', 'Don\'t Know', 'Good', 'Excellent'],");
            sb.AppendLine(@"['Catholic Schools', 2467, 3642, 7773, 5026, 2182],");
            sb.AppendLine(@"['Christian service and outreach', 2222, 5922, 9256, 9228, 3974],");
            sb.AppendLine(@"['Evangelization and catechesis', 2482, 6159, 9712, 8474, 3641],");
            sb.AppendLine(@"['Lay leadership involvement', 2628, 5848, 10134, 8282, 3606],");
            sb.AppendLine(@"['Stewardship and administration', 2595, 5600, 10342, 8137, 3695],");
            sb.AppendLine(@"['Vocations', 2727, 6098, 10536, 7698, 3320],");
            sb.AppendLine(@"['Youth and young adult ministry', 2999, 5623, 10748, 6999, 3490]");
            sb.AppendLine(@"]");
            s = sb.ToString();

            actual = CoreExtensions.IsGoogleArray(s);
            expected = true;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsTabDelimited
        ///</summary>
        [TestMethod()]
        public void IsTabDelimitedTest()
        {
            string s = "Poor\t\tFair\t\tDon't Know\t\tGood\t\tExcellent\t\n5396\t\t12056\t\t32425\t\t30753\t\t38158\t\n";
            bool expected = true;
            bool actual;
            actual = CoreExtensions.IsTabDelimited(s);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for toGoggleArray
        ///</summary>
        [TestMethod()]
        public void toGoggleArrayTest()
        {
            string s = "Poor\t\tFair\t\tDon't Know\t\tGood\t\tExcellent\t\n5396\t\t12056\t\t32425\t\t30753\t\t38158\t\n";

            string actual;
            actual = CoreExtensions.toGogleArray(s);
            Assert.IsTrue(actual.IsGoogleArray());

        }
        /// <summary>
        ///A test for toGoggleArray with bigger data
        ///</summary>
        [TestMethod()]
        public void toGoggleArrayTestBigTable()
        {
            string s = "\tGood\tExcellent\tTotal\nProviding regular meetings for teens \t5847\t8695\t20964\nProviding special events for teens (e.g. social events, retreats, service experiences)\t7869\t12280\t29617\nEngaging youth in parish life and liturgies\t8160\t11422\t29590\nProviding regular meetings for young adults\t6906\t9770\t29505\nHosting special events for young adults \t6954\t9298\t29520\nEngaging young adults in parish ministries and outreaches\t7069\t9432\t29478\n";

            string actual;
            actual = CoreExtensions.toGogleArray(s);
            Assert.AreNotEqual("[]", actual);
            Assert.IsTrue(actual.IsGoogleArray());

        }

        /// <summary>
        ///A test for InBetween
        ///</summary>
        [TestMethod()]
        public void InBetweenTest()
        {
            string @this = "a - to keep x more stuff"; // TODO: Initialize to an appropriate value
            string from = string.Empty; // TODO: Initialize to an appropriate value
            string until = string.Empty; // TODO: Initialize to an appropriate value

            string expected = " to keep ";
            string actual;
            actual = CoreExtensions.InBetween(@this, from: "-", until: "x");
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for RecodeHtml
        ///</summary>
        [TestMethod()]
        public void RecodeHtmlTest()
        {
            string toRecode = "this is a [strong]test[/strong]";
            string[] tags = new string[] { "strong", "li", "ul" };
            string lTag = "[";
            string rTag = "]";
            string expected = "this is a <strong>test</strong>";
            string actual = CoreExtensions.RecodeHtml(toRecode, tags, lTag, rTag);
            Assert.AreEqual(expected, actual);
            // Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for RecodeHtml
        ///</summary>
        [TestMethod()]
        public void RecodeHtmlBrTest()
        {
            string toRecode = "this is a [strong]test[/strong][br/]";
            string[] tags = new string[] { "strong", "li", "ul", "br" };
            string lTag = "[";
            string rTag = "]";
            string expected = "this is a <strong>test</strong><br />";
            string actual = CoreExtensions.RecodeHtml(toRecode, tags, lTag, rTag);
            Assert.AreEqual(expected, actual);
            // Assert.Inconclusive("Verify the correctness of this test method.");
        }
        [TestMethod()]
        public void SanitizeHtmlTablesTest()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(@"<table>");
            sb.AppendLine(@"  <tr>");
            sb.AppendLine(@"    <td>Jill</td>");
            sb.AppendLine(@"    <td>Smith</td>");
            sb.AppendLine(@"    <td>50</td>");
            sb.AppendLine(@"  </tr>");
            sb.AppendLine(@"  <tr>");
            sb.AppendLine(@"    <td>Eve</td>");
            sb.AppendLine(@"    <td>Jackson</td>");
            sb.AppendLine(@"    <td>94</td>");
            sb.AppendLine(@"  </tr>");
            sb.AppendLine(@"</table> ");

            var test = sb.ToString();

            var sb2 = new System.Text.StringBuilder(61);
            sb.AppendLine(@"Jill");
            sb.AppendLine(@"    Smith");
            sb.AppendLine(@"    50");
            sb.AppendLine(@"  ");
            sb.AppendLine(@"  ");
            sb.AppendLine(@"    Eve");
            sb.AppendLine(@"    Jackson");
            sb.AppendLine(@"    94");

            var expected = sb2.ToString();


            string actual = CoreExtensions.SanitizeHtmlTables(test);
            Assert.AreEqual(-1, actual.IndexOf("<table>"));
            // Assert.Inconclusive("Verify the correctness of this test method.");
        }


        /// <summary>
        ///A test for TrimAfter
        ///</summary>
        [TestMethod()]
        public void TrimAfterTest()
        {
            string source = "5/11/2009 12:00:00 AM ";
            string searchString = " ";
            string expected = "5/11/2009";
            string actual;
            actual = CoreExtensions.TrimAfter(source, searchString);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsHtmlValid
        ///</summary>
        [TestMethod()]
        public void IsHtmlValidTest()
        {
            string s = "hello <strong>world</strong>";
            bool expected = true;
            bool actual;
            actual = CoreExtensions.IsHtmlValid(s);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        [TestMethod()]
        public void IsHtmlValidTest2()
        {
            string s = "hello <strong>world<strong>";
            bool expected = false;
            bool actual;
            actual = CoreExtensions.IsHtmlValid(s);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        [TestMethod()]
        public void IsHtmlValidTestComplex()
        {
            StringBuilder sb = new StringBuilder(374);
            sb.AppendLine(@"""<p>Suppose we have a rule which requires data in one field to be a certain value given data in a completely different field. We have a couple of options:</p><ol>");
            sb.AppendLine(@"<li>Standard Validation  paired with helper methods to traverse controls on the page to find the control we’re looking for</li>");
            sb.AppendLine(@"<li>Form Verification Actions  access to all form fields</li>");
            sb.AppendLine(@"</ol>"";");

            string s = sb.ToString();
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = CoreExtensions.IsHtmlValid(s);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        [TestMethod()]
        public void IsHtmlValidTestComplexFalse()
        {
            StringBuilder sb = new StringBuilder(372);
            sb.AppendLine(@"""<p>Suppose we have a rule which requires data in one field to be a certain value given data in a completely different field. We have a couple of options:</p><ol>");
            sb.AppendLine(@"<li>Standard Validation paired with helper methods to traverse controls on the page to find the control we’re looking for</li>");
            sb.AppendLine(@"<l>Form Verification Actions  access to all form fields</li>");
            sb.AppendLine(@"<ol>"";");

            string s = sb.ToString();
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = CoreExtensions.IsHtmlValid(s);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for StripTagsCharArray
        ///</summary>
        [TestMethod()]
        public void StripTagsCharArrayTest()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"""<p>Suppose we have a rule which requires data in one field to be a certain value given data in a completely different field. We have a couple of options:</p><ol>");
            sb.AppendLine(@"<li>Standard Validation  paired with helper methods to traverse controls on the page to find the control we’re looking for</li>");
            sb.AppendLine(@"<li>Form Verification Actions  access to all form fields</li>");
            sb.AppendLine(@"</ol>");

            StringBuilder sbexpected = new StringBuilder();
            sbexpected.AppendLine(@"""Suppose we have a rule which requires data in one field to be a certain value given data in a completely different field. We have a couple of options:");
            sbexpected.AppendLine(@"Standard Validation  paired with helper methods to traverse controls on the page to find the control we’re looking for");
            sbexpected.AppendLine(@"Form Verification Actions  access to all form fields");

            string source = sb.ToString();
            string expected = sbexpected.ToString().Trim();
            string actual;
            actual = CoreExtensions.StripTagsCharArray(source).Trim();
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        [TestMethod()]
        public void ToDateTimeTest()
        {
            string d = "10-5-2015";
            string h = "15:10";
            DateTime expected = new DateTime(2015, 10, 5, 15, 10, 00);
            DateTime actual;
            actual = CoreExtensions.ToDateTime(d, h);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        [TestMethod()]
        public void IsMilitaryTimeTest()
        {
            var actual = "08:00".IsMilitaryTime();
            var expected = true;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void IsMilitaryTimeTest1()
        {
            var actual = "8:00".IsMilitaryTime();
            var expected = false;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void IsMilitaryTimeTest2()
        {
            var actual = "8:00 PM".IsMilitaryTime();
            var expected = false;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void FixMilitaryTimeTest()
        {
            var input = "8:00".FixMilitaryTime();
            var actual = input.IsMilitaryTime();
            var expected = true;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void FixMilitaryTimeTest2()
        {
            var input = "32:00".FixMilitaryTime();
            Assert.AreEqual("00:00", input);

            var actual = input.IsMilitaryTime();
            var expected = true;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void IsAMPMTest()
        {
            var actual = "08:00 AM".IsAMPMTime();
            var expected = true;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void IsAMPMTest1()
        {
            var actual = "8:00".IsAMPMTime();
            var expected = false;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void ToStandardTimeTest()
        {
            string input = "8:00";
            var actual = CoreExtensions.ToStandardTime(input);
            string expected = "8:00 AM";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void ToStandardTimeTest1()
        {
            string input = "20:00";
            var actual = CoreExtensions.ToStandardTime(input);
            string expected = "8:00 PM";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void ToStandardTimeTest2()
        {
            string input = "8:00 AM";
            var actual = CoreExtensions.ToStandardTime(input);
            string expected = "8:00 AM";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void TrimLeadingZeroTest()
        {
            var actual = "08:00".TrimLeadingZero();
            var expected = "8:00";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void TrimLeadingZeroTest1()
        {
            var actual = " 08:00".TrimLeadingZero();
            var expected = "8:00";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void RemoveBracketsTest()
        {
            string test = "{asd;flkjl}";
            string expected = "asd;flkjl";
            string actual = CoreExtensions.RemoveBrackets(test);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void AddressOnStringTest()
        {
            string expected = "2600 Harvard Rd Berkley, MI 48072";
            string actual = CoreExtensions.AddressOnString("Our Lady of La Salette Church 2600 Harvard Rd Berkley, MI 48072");
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void AddressOnStringTest2()
        {
            string expected = "2000 event1";
            string actual = CoreExtensions.AddressOnString("aaa 2000 event1");
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void GetCoordinatesTest()
        {
            string test = "Our Lady of La Salette Church 2600 Harvard Rd Berkley, MI 48072";
            string[] expected = new string[] { "42.4923395", "-83.1849986" };
            var actual = CoreExtensions.GetCoordinates(test);
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
        }
        [TestMethod()]
        public void LastWordTest()
        {
            string test = "Our Lady of La Salette Church 2600 Harvard Rd Berkley, MI 48072";
            string expected = "48072";
            var actual = CoreExtensions.LastWord(test);
            Assert.AreEqual(expected, actual);
        }
        public void LastWordTest2()
        {
            string test = string.Empty;
            string expected = string.Empty;
            var actual = CoreExtensions.LastWord(test);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void StripLastWordTest()
        {
            string test = "Our Lady of La Salette Church 2600 Harvard Rd Berkley, MI 48072";
            string expected = "Our Lady of La Salette Church 2600 Harvard Rd Berkley, MI";
            var actual = CoreExtensions.StripLastWord(test);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void StripLastWordTest2()
        {
            string test = "Our Lady of La Salette Church 2600 Harvard Rd Berkley, MI 48072";
            string expected = "Our Lady of La Salette Church 2600 Harvard Rd Berkley, MI 48072";
            var actual = CoreExtensions.StripLastWord(test, "aaa");
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void StripLastWordTest3()
        {
            string test = "Our Lady of La Salette Church 2600 Harvard Rd Berkley, MI 48072";
            string expected = "Our Lady of La Salette Church 2600 Harvard Rd Berkley, MI";
            var actual = CoreExtensions.StripLastWord(test, "48072");

        }
        [TestMethod()]
        public void StripLastWordTest4()
        {
            string test = "Our Lady of La Salette Church 2600 Harvard Rd Berkley, MI 48072";
            string expected = "Our Lady of La Salette Church 2600 Harvard Rd Berkley, MI test";
            var actual = CoreExtensions.StripLastWord(test, "48072", "test");
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void StripLastWordTest5()
        {
            string test = string.Empty;
            string expected = string.Empty;
            var actual = CoreExtensions.StripLastWord(test, "48072", "test");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ReplaceMultipleTest()
        {
            string test = "mult|iple ch{ars{)}*";
            string expected = "multiple chars";
            string actual = test.ReplaceMultiple(string.Empty, "{", "}", "|", "*", ")");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void BytesToStringTest()
        {
            string test = "35674";
            var actual = test.BytesToString();
            Assert.AreEqual("34.8KB", actual);
        }

        [TestMethod()]
        public void DecodeUserPWdBasicTest()
        {
            string test = "Basic YWhtL3VhdF91c2VyOkFITTEwMjA=";
            string actual = test.DecodeUserPWdBasic();

            Assert.AreEqual("ahm/uat_user:AHM1020", actual);
        }
    }
}