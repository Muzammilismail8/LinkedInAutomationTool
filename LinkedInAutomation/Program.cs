using LinkedInAutomation;
using OpenQA.Selenium;

Console.WriteLine("Starting LinkedIn Automation");
IWebDriver driver = EnviornmentHandler.ConfigureAndRunCrome()!;

driver.Navigate().GoToUrl("https://www.linkedin.com/jobs/search/?f_AL=true&f_E=4&f_TPR=r2592000&f_WT=2&geoId=101022442&keywords=.net&origin=JOB_SEARCH_PAGE_JOB_FILTER&refresh=true&spellCorrectionEnabled=true");
Thread.Sleep(3000);

JobHandler.FindNextJobtoApply(driver);

while (true)
{
    try{JobHandler.ApplyOnCurrentJob(driver);}
    catch (Exception) { }
}

//driver.Dispose();