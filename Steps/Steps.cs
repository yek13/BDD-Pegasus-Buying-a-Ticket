using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Pegasus.Base;
using Pegasus.Util;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace Pegasus
{
    [Binding]
    public sealed class Steps// steps kısmında bdd adımları tutulur yani senaryolar
    {

        public IWebDriver Driver { get; set; }
        public BasePage basePage;
        public ScreenShot screenshot;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Dictionary<string, string> saveEnv = new Dictionary<string, string>();
        private readonly ScenarioContext context;

        public Steps(ScenarioContext injectedContext)
        {
            context = injectedContext;
        }



        //Browser ayaga kalkar
        [BeforeScenario]//her senaryonun oncesınde calış.
        public void SetUp()
        {
            Logging.Logger();
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            options.AddArgument("disable-popup-blocking");
            options.AddArgument("disable-notifications");
            options.AddArgument("test-type");
            Driver = new ChromeDriver(options);

            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            Driver.Navigate().GoToUrl("https://www.flypgs.com/");
            log.Info("Driver ayağa kalktı...");
            basePage = new BasePage(Driver); //burdakı amac basepage deki metodları çağırmak ıcın olusturuldu.

        }


        [BeforeStep]//her adım öncesi çalışır.
        public void BeforeStep()
        {
            log.Info("Step :" + context.StepContext.StepInfo.Text);

        }

        //CssSelector ile yapmamızda ki amaç kapsayıcı oldugu ıcın hepsi kullanılması için ayrı ayrı method yazılmaması için kullanıldı. Ayrı ayrı id xpath classname kullanılmaması için.
        [Given("'(.*)' objesine tıklanır.")]
        public void LoginSteps(String obje)
        {

            log.Info("Parametre   :" + obje);
            basePage.ClickElement(By.CssSelector(obje));
            //new ScreenShot(Driver).TakeScreenShot(context.ScenarioInfo.Title);
 
        }
         
        [Given("'(.*)' alanına \"(.*)\" değeri yazılır.")]
        public void SentKeys(String obje, String text)
        {

            log.Info("Parametre   :" + obje);

            basePage.SendKeys(By.CssSelector(obje), text);
            //new ScreenShot(Driver).TakeScreenShot(context.ScenarioInfo.Title);

        }
        
        
        [Given("'(.*)' alanından \"(.*)\" tarihi seçilir.")]
        public void ControlDate(String obje, String value)
        {

            log.Info("Parametre   :" + obje);
            string[] reserved_tarih = value.Split(".");
            basePage.DateControl(obje, reserved_tarih);
            
            //new ScreenShot(Driver).TakeScreenShot(context.ScenarioInfo.Title);

        }
        
        [Given("'(.*)' saniye beklenir.")]
        public void TimeWait(int second)
        {

            log.Info(second + "saniye bekleniyor...");
            Thread.Sleep(TimeSpan.FromSeconds(second));
            //new ScreenShot(Driver).TakeScreenShot(context.ScenarioInfo.Title);

        }

        //browser kapatılır
        [AfterScenario]
        public void TearDown()
        {
            Driver.Quit();
        }

    }
}
