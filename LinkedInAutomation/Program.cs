using LinkedInAutomation;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

Console.WriteLine("Starting LinkedIn Automation");

IWebDriver? driver = ConfigureAndRunCrome();
if(driver == null) { throw new NotFoundException(); }

driver.Navigate().GoToUrl("https://www.linkedin.com/jobs/collections/easy-apply");
Thread.Sleep(3000);

while (true)
{
    JobHandler.ApplyOnCurrentJob(driver);
}

static IWebDriver? ConfigureAndRunCrome()
{
    try
    {
        foreach (var process in Process.GetProcessesByName("chrome"))
        {
            process.Kill();
            process.WaitForExit();
        }

        new DriverManager().SetUpDriver(new ChromeConfig());
        var options = new ChromeOptions();

        string windowUserName = "Reacon";

        options.AddArgument(@$"user-data-dir=C:\Users\{windowUserName}\AppData\Local\Google\Chrome\User Data");
        options.AddArgument(@"profile-directory=Default");

        return new ChromeDriver(options);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    return null;
}

//driver.Dispose();