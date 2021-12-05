using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestFixture]
    public class WebShopTests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver();
            baseURL = "https://demo.opencart.com/";
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void TheCorrectLoginTest()
        {
            driver.Navigate().GoToUrl(baseURL);
            driver.FindElement(By.XPath("//div[@id='top-links']/ul/li[2]/a/span")).Click();
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("input-email")).Click();
            driver.FindElement(By.Id("input-email")).Clear();
            driver.FindElement(By.Id("input-email")).SendKeys("dominik.zivko@student.ferit.hr");
            driver.FindElement(By.Id("input-password")).Clear();
            driver.FindElement(By.Id("input-password")).SendKeys("ruap");
            driver.FindElement(By.XPath("//input[@value='Login']")).Click();
            Assert.AreEqual(driver.Url, "https://demo.opencart.com/index.php?route=account/account");
        }

        [Test]
        public void TheEditAccountTest()
        {
            driver.Navigate().GoToUrl(baseURL);
            driver.FindElement(By.XPath("//div[@id='top-links']/ul/li[2]/a/span")).Click();
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("input-email")).Click();
            driver.FindElement(By.Id("input-email")).Clear();
            driver.FindElement(By.Id("input-email")).SendKeys("dominik.zivko@student.ferit.hr");
            driver.FindElement(By.Id("input-password")).Clear();
            driver.FindElement(By.Id("input-password")).SendKeys("ruap");
            driver.FindElement(By.XPath("//input[@value='Login']")).Click();
            driver.FindElement(By.LinkText("Edit your account information")).Click();
            driver.FindElement(By.Id("input-firstname")).Click();
            driver.FindElement(By.Id("input-firstname")).Clear();
            driver.FindElement(By.Id("input-firstname")).SendKeys("NewName");
            driver.FindElement(By.Id("input-lastname")).Clear();
            driver.FindElement(By.Id("input-lastname")).SendKeys("NewLastName");
            driver.FindElement(By.XPath("//input[@value='Continue']")).Click();
            Assert.AreEqual("Success: Your account has been successfully updated.", driver.FindElement(By.XPath("//div[@id='account-account']/div")).Text);
        }

        [Test]
        public void TheAddToCartTest()
        {
            driver.Navigate().GoToUrl("https://demo.opencart.com/");
            driver.FindElement(By.XPath("//div[@id='top-links']/ul/li[2]/a/span")).Click();
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("input-email")).Click();
            driver.FindElement(By.Id("input-email")).Clear();
            driver.FindElement(By.Id("input-email")).SendKeys("dominik.zivko@student.ferit.hr");
            driver.FindElement(By.Id("input-password")).Click();
            driver.FindElement(By.Id("input-password")).Clear();
            driver.FindElement(By.Id("input-password")).SendKeys("ruap");
            driver.FindElement(By.XPath("//input[@value='Login']")).Click();
            driver.FindElement(By.LinkText("Your Store")).Click();
            driver.FindElement(By.XPath("//div[@id='content']/div[2]/div/div/div[3]/button/span")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//div[@id='top-links']/ul/li[4]/a/span")).Click();
            Thread.Sleep(500);
            Assert.IsTrue(IsElementPresent(By.XPath("//div[@id='content']/form/div/table/tbody/tr/td[2]")));
        }

        [Test]
        public void TheWriteAReviewTest()
        {
            driver.Navigate().GoToUrl("https://demo.opencart.com/");
            driver.FindElement(By.XPath("//div[@id='top-links']/ul/li[2]/a/span")).Click();
            driver.FindElement(By.LinkText("Login")).Click();
            driver.FindElement(By.Id("input-email")).Click();
            driver.FindElement(By.Id("input-email")).Clear();
            driver.FindElement(By.Id("input-email")).SendKeys("dominik.zivko@student.ferit.hr");
            driver.FindElement(By.Id("input-password")).Click();
            driver.FindElement(By.Id("input-password")).Clear();
            driver.FindElement(By.Id("input-password")).SendKeys("ruap");
            driver.FindElement(By.XPath("//input[@value='Login']")).Click();
            driver.Navigate().GoToUrl("https://demo.opencart.com/index.php?route=product/category&path=20_27");
            driver.FindElement(By.LinkText("iMac")).Click();
            driver.FindElement(By.LinkText("Write a review")).Click();
            driver.FindElement(By.Id("input-review")).Click();
            driver.FindElement(By.Id("input-review")).Clear();
            driver.FindElement(By.Id("input-review")).SendKeys("Sufficiently long review text");
            driver.FindElement(By.XPath("//form[@id='form-review']/div[4]/div/input[3]")).Click();
            driver.FindElement(By.Id("button-review")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Thank you for your review. It has been submitted to the webmaster for approval.", driver.FindElement(By.XPath("//form[@id='form-review']/div[2]")).Text);
        }

        [Test]
        public void TheSearchTest()
        {
            driver.Navigate().GoToUrl("https://demo.opencart.com/");
            driver.FindElement(By.Name("search")).Click();
            driver.FindElement(By.Name("search")).Clear();
            driver.FindElement(By.Name("search")).SendKeys("iPod");
            driver.FindElement(By.XPath("//div[@id='search']/span/button/i")).Click();
            Assert.IsTrue(IsElementPresent(By.LinkText("iPod Classic")));
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}
