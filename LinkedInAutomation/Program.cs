// See https://aka.ms/new-console-template for more information
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

Console.WriteLine("Starting LinkedIn Automation");

new DriverManager().SetUpDriver(new ChromeConfig());
using IWebDriver driver = new ChromeDriver();

try
{
    // Navigate to LinkedIn login page
    driver.Navigate().GoToUrl("https://www.linkedin.com/login");

    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

    // Login
    wait.Until(d => d.FindElement(By.Id("username"))).SendKeys("oliviagates16@gmail.com");
    wait.Until(d => d.FindElement(By.Id("password"))).SendKeys("fakeman039");
    wait.Until(d => d.FindElement(By.XPath("//button[@type='submit']"))).Click();

    // Navigate to the jobs page
    driver.Navigate().GoToUrl("https://www.linkedin.com/jobs/collections/easy-apply");

    // Wait for the Easy Apply button
    IWebElement easyApplyButton = wait.Until(d =>
    {
        var buttons = d.FindElements(By.XPath("//button[contains(@class, 'jobs-apply-button--top-card')]"));
        if (buttons.Count > 0) return buttons[0];

        buttons = d.FindElements(By.XPath("//button[contains(@class, 'jobs-apply-button')]"));
        return buttons.Count > 0 ? buttons[0] : null;
    });

    if (easyApplyButton != null)
    {
        easyApplyButton.Click();
        Console.WriteLine("Clicked Easy Apply button.");
    }
    else
    {
        Console.WriteLine("No Easy Apply button found.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
finally
{
    driver.Quit();
}