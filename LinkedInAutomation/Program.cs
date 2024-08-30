using LinkedInAutomation;
using OpenQA.Selenium;

Console.WriteLine("Starting LinkedIn Automation");
IWebDriver driver = EnviornmentHandler.ConfigureAndRunCrome()!;

driver.Navigate().GoToUrl("https://www.linkedin.com/jobs/collections/easy-apply");
Thread.Sleep(3000);

while (true)
{
    JobHandler.ApplyOnCurrentJob(driver);
}


//driver.Dispose();