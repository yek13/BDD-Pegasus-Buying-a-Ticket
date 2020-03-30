using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Pegasus.Util;

namespace Pegasus.Base
{
    public class BasePage
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IWebDriver driver;
        WebDriverWait wait;

        IReadOnlyList<IWebElement> result;

        IJavaScriptExecutor scriptExecutor;
        public string goingRightClick = "#search-flight-datepicker-departure > div > div.ui-datepicker-group.ui-datepicker-group-last > div > a";
        public string returnRightClick = "#search-flight-datepicker-arrival> div > div.ui-datepicker-group.ui-datepicker-group-last > div > a";
        public string goingYear = "#search-flight-datepicker-departure .ui-datepicker-group-first .ui-datepicker-year";
        public string goingMonth = "#search-flight-datepicker-departure .ui-datepicker-group-first .ui-datepicker-month";
        public string goingDay = "#search-flight-datepicker-departure .ui-datepicker-group-first tbody a";
        public string returnYear = "#search-flight-datepicker-arrival .ui-datepicker-group-first .ui-datepicker-year";
        public string returnMonth = "#search-flight-datepicker-arrival .ui-datepicker-group-first .ui-datepicker-month";
        public string returnDay = "#search-flight-datepicker-arrival .ui-datepicker-group-first tbody a";


        public BasePage(IWebDriver driver)
        {


            this.driver = driver;

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
        }
        public IWebElement FindElement(By by)
        {


            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
            HightLightElement(by);
            return driver.FindElement(by);


        }

        public void ClickElement(By by)
        {
            FindElement(by).Click();
        }

        public SelectElement SelectOptions(IWebElement element)
        {
            return new SelectElement(element);
        }

        public void SendKeys(By by, String value)
        {
            FindElement(by).SendKeys(value);
            FindElement(by).SendKeys(Keys.Enter);
        }


        public void SelectElementByText(By by, String visibleText)
        {

            SelectOptions(FindElement(by)).SelectByText(visibleText);

        }


        public string GetText(By by)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
            string elementText = FindElement(by).Text;
            log.Info("Element Text :" + elementText);
            return elementText;
        }

        public void HightLightElement(By by)
        {
            scriptExecutor = (IJavaScriptExecutor)driver;
            scriptExecutor.ExecuteScript("arguments[0].setAttribute('style', 'background: yellow; border: 2px solid red;');", driver.FindElement(by));
            Thread.Sleep(TimeSpan.FromMilliseconds(700));
        }


        public void DateControl(string option, string[] date)
        {

            if (option.Equals("Gidiş Tarihi"))
            {
                while (true)
                {
                    if (date[2].Equals(GetText(By.CssSelector(goingYear))) && date[1].Equals(GetText(By.CssSelector(goingMonth))))
                    {
                        ControlDay(date[0], goingDay);
                        break;
                    }

                    else
                    {
                        ClickElement(By.CssSelector(goingRightClick));
                    }
                }
            }
            else if (option.Equals("Dönüş Tarihi"))
            {
                while (true)
                {
                    if (date[2].Equals(GetText(By.CssSelector(returnYear))) && date[1].Equals(GetText(By.CssSelector(returnMonth))))
                    {
                        ControlDay(date[0], returnDay);
                        break;
                    }
                    else
                    {
                        ClickElement(By.CssSelector(returnRightClick));
                    }
                }
            }

        }

        public void ControlDay(string day, string daySelector)
        {
            result = driver.FindElements(By.CssSelector(daySelector));
            foreach (var days in result)
            {
                if (days.Text.Equals(day))
                {
                    days.Click();
                    break;
                }
            }
        }
       
    }
}
