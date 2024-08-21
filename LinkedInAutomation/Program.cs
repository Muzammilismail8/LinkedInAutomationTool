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

//apply on the current job
ApplyOnCurrentJob(driver, wait);


static void ApplyOnCurrentJob(IWebDriver driver, WebDriverWait wait)
{
    try
    {
        // Wait for the Easy Apply button
        IWebElement? easyApplyButton = wait.Until(d =>
        {
            var buttons = d.FindElements(By.XPath("//button[contains(@class, 'jobs-apply-button--top-card')]"));
            if (buttons.Count > 0) return buttons[0];

            while (buttons.Count == 0)
            {
                buttons = d.FindElements(By.XPath("//button[contains(@class, 'jobs-apply-button')]"));

                if (buttons.Count == 0)
                {
                    FindNextJobtoApply(driver);
                }

            }

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

    // Dictionary for form data
    var formData = new Dictionary<string, string>
        {
            { "email", "your_email@example.com" },
            { "phone", "1234567890" },
            { "countryCode", "+92" }
        };

    IWebElement? reviewButton;
    IWebElement? nextButton;

    reviewButton = FindButtonByStrings(driver, ["Review your application"]);
    nextButton = FindButtonByStrings(driver, ["Continue to next step"]);

    while (reviewButton == null && nextButton != null)
    {
        // Find and fill the phone number field
        var phoneField = FindEmptyInputFieldByStrings(driver, ["phone", "Phone", "tel"]);
        if (phoneField != null) phoneField.SendKeys(formData["phone"]);

        nextButton.Click();

        reviewButton = FindButtonByStrings(driver, ["Review your application"]);
    }

    if (reviewButton != null) reviewButton.Click();

    var submitButton = FindButtonByStrings(driver, ["Submit application"]);
    if (submitButton != null) submitButton.Click();
}

static IWebElement? FindButtonByStrings(IWebDriver driver, string[] partialAttributeNames)
{
    foreach (string partialName in partialAttributeNames)
    {
        try
        {
            IWebElement element = driver.FindElement(By.XPath($"//button[@aria-label='{partialName}']"));

            if (element != null)
            {
                return element;
            }
        }
        catch (NoSuchElementException) { }
    }
    return null;
}

static IWebElement? FindEmptyInputFieldByStrings(IWebDriver driver, string[] partialAttributeNames)
{
    foreach (string partialName in partialAttributeNames)
    {
        try
        {
            IWebElement element = driver.FindElement(By.XPath($"//input[contains(@name, '{partialName}') or contains(@id, '{partialName}')]"));

            if (element != null && string.IsNullOrEmpty(element.GetAttribute("value")))
            {
                return element;
            }
        }
        catch (NoSuchElementException) { }
    }

    return null;
}


static void FindNextJobtoApply(IWebDriver driver)
{
    // Find all the job listings
    var jobListings = driver.FindElements(By.CssSelector(".jobs-search-results__list-item"));

    foreach (var jobListing in jobListings)
    {
        try
        {
            // Check if the job has already been applied to (by locating the 'Applied' text)
            var appliedStatus = jobListing.FindElement(By.CssSelector(".job-card-container__footer-item.job-card-container__footer-job-state"));

            if (appliedStatus.Text.Contains("Applied"))
            {
                Console.WriteLine("Already applied to this job. Skipping...");
                continue;  // Skip this job and move to the next one
            }
        }
        catch (NoSuchElementException ex)
        {
            // 'Applied' text not found, meaning you haven't applied to this job yet.
            Console.WriteLine("Not applied to this job. Clicking...");
            jobListing.Click();  // Click the job to view/apply
            break;  // Stop once a non-applied job is found
        }
    }
}