// See https://aka.ms/new-console-template for more information
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

Console.WriteLine("Starting LinkedIn Automation");

new DriverManager().SetUpDriver(new ChromeConfig());
IWebDriver driver = new ChromeDriver();

// Navigate to LinkedIn login page
driver.Navigate().GoToUrl("https://www.linkedin.com/login");

WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

// Login
wait.Until(d => d.FindElement(By.Id("username"))).SendKeys("oliviagates16@gmail.com");
wait.Until(d => d.FindElement(By.Id("password"))).SendKeys("fakeman039");
wait.Until(d => d.FindElement(By.XPath("//button[@type='submit']"))).Click();

// Navigate to the jobs page
driver.Navigate().GoToUrl("https://www.linkedin.com/jobs/collections/easy-apply");

try
{

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
    //driver.Quit();
}





// Dictionary for form data
var formData = new Dictionary<string, string>
        {
            { "email", "your_email@example.com" },
            { "phone", "1234567890" },
            { "countryCode", "+92" }
        };

IWebElement? reviewButton;

try{reviewButton = driver.FindElement(By.XPath("//button[@aria-label='Review your application']"));}
catch (Exception){reviewButton = null;}

while (reviewButton == null)
{
    // Find and fill the email field
    var emailField = FindElementByPartialAttribute(driver, new string[] { "email", "Email", "e-mail" });
    if (emailField != null) emailField.SendKeys(formData["email"]);

    // Find and fill the phone number field
    var phoneField = FindElementByPartialAttribute(driver, new string[] { "phone", "Phone", "tel" });
    if (phoneField != null) phoneField.SendKeys(formData["phone"]);

    // Find and fill the country code field
    var countryCodeField = FindElementByPartialAttribute(driver, new string[] { "country", "Country", "code" });
    if (countryCodeField != null) countryCodeField.SendKeys(formData["countryCode"]);

    // Click the "Next" button
    var nextButton = driver.FindElement(By.XPath("//button[@aria-label='Continue to next step']"));
    if (nextButton != null) nextButton.Click();

    reviewButton = driver.FindElement(By.XPath("//button[@aria-label='Review your application']"));
}

if (reviewButton != null) reviewButton.Click();

var submitButton = driver.FindElement(By.XPath("//button[@aria-label='Submit application']"));
if (submitButton != null) submitButton.Click();


static IWebElement FindElementByPartialAttribute(IWebDriver driver, string[] partialAttributeNames)
{
    foreach (string partialName in partialAttributeNames)
    {
        try
        {
            // Try finding element by name or id
            IWebElement element = driver.FindElement(By.XPath($"//input[contains(@name, '{partialName}') or contains(@id, '{partialName}')]"));
            if (element != null)
                return element;
        }
        catch (NoSuchElementException) { }
    }
    return null;
}