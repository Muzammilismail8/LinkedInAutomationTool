using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Diagnostics;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;

namespace LinkedInAutomation;

public static class EnviornmentHandler
{
    public static IWebDriver? ConfigureAndRunCrome()
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

            IWebDriver? driver = new ChromeDriver(options);
            return driver;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

}
