using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
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
    ApplyOnCurrentJob(driver);
}

static void ApplyOnCurrentJob(IWebDriver driver)
{
    try
    {
        FindAndClickEasyApplyButton(driver);

        Thread.Sleep(2000);
        IWebElement? reviewButton;

        IWebElement? nextButton;
        IWebElement? IsErrorExist = null;

        reviewButton = FindButtonByStrings(driver, ["Review your application"]);
        nextButton = FindButtonByStrings(driver, ["Continue to next step"]);

        while ((reviewButton == null || IsErrorExist != null) && nextButton != null)
        {
            if(reviewButton == null) nextButton.Click();

            while (IsErrorExist != null)
            {
                FillForm(driver);
                IsErrorExist = GetDivByClass(driver, ["error", "preview__response--is-required"]);
            }

            reviewButton = FindButtonByStrings(driver, ["Review your application"]);
            if (reviewButton != null) reviewButton.Click();

            IsErrorExist = GetDivByClass(driver, ["error", "preview__response--is-required"]);
            Thread.Sleep(2000);
        }

        var submitButton = FindButtonByStrings(driver, ["Submit application"]);
        if (submitButton != null) submitButton.Click();

        Thread.Sleep(3000);
        ColsetheApplicationSentModal(driver);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

static IWebElement? GetDivByClass(IWebDriver driver, string[] partialAttributeNames)
{
    try
    {
        foreach (string partialName in partialAttributeNames)
        {
            try
            {
                IWebElement element = driver.FindElement(By.XPath($"//div[contains(@class, '{partialName}')]"));

                if (element != null)
                {
                    return element;
                }
            }
            catch (NoSuchElementException ex)
            { }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    return null;
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

static void FindAndClickEasyApplyButton(IWebDriver driver)
{
    try
    {
        IWebElement? easyApplyButton = null;
        bool isJobListingExist = true;

        while (easyApplyButton == null && isJobListingExist)
        {
            easyApplyButton = FindButtonByStrings(driver, ["Easy Apply"]);

            if (easyApplyButton == null)
            {
                isJobListingExist = FindNextJobtoApply(driver);
            }
        }

        if (easyApplyButton != null) easyApplyButton.Click();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

static void ColsetheApplicationSentModal(IWebDriver driver)
{
    try
    {
        var waitForModal = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        waitForModal.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div.artdeco-modal")));

        IWebElement modal = driver.FindElement(By.CssSelector("div.artdeco-modal"));
        IWebElement modalContent = modal.FindElement(By.CssSelector("div.artdeco-modal__content"));

        if (modalContent.Text.Contains("Your application was sent"))
        {
            Console.WriteLine("Text found. Closing modal...");
            IWebElement? dismissButton = FindButtonByStrings(driver, ["Dismiss"]);

            if (dismissButton != null) dismissButton.Click();
        }
        else
        {
            Console.WriteLine("Colse button not found.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

static IWebElement? FindButtonByStrings(IWebDriver driver, string[] partialAttributeNames)
{
    try
    {
        foreach (string partialName in partialAttributeNames)
        {
            try
            {
                IWebElement element = driver.FindElement(By.XPath($"//button[contains(@aria-label, '{partialName}')]"));

                if (element != null)
                {
                    return element;
                }
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    return null;
}

static void FillForm(IWebDriver driver)
{
    try
    {
        var formData = new Dictionary<string, string>
        {
            { "email", "oliviagates16@gmail.com" },
            { "phone", "3347479687" },
            { "countryCode", "+92" }
        };

        var phoneField = FindEmptyInputFieldByStrings(driver, ["phone", "Phone", "tel"]);
        if (phoneField != null) phoneField.SendKeys(formData["phone"]);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

static IWebElement? FindEmptyInputFieldByStrings(IWebDriver driver, string[] partialAttributeNames)
{
    try
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
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        throw;
    }

    return null;
}

static bool FindNextJobtoApply(IWebDriver driver)
{
    try
    {
        var jobListings = driver.FindElements(By.CssSelector(".jobs-search-results__list-item"));

        for (int i = 0; i < jobListings.Count; i++)
        {
            var currentJob = jobListings[i];

            try
            {
                var appliedStatus = currentJob.FindElement(By.CssSelector(".job-card-container__footer-item.job-card-container__footer-job-state"));

                if (appliedStatus != null && appliedStatus.Text.Contains("Applied"))
                {
                    Console.WriteLine("Already applied to this job. Skipping...");
                    
                    if (i == jobListings.Count - 1)
                    {
                        var nextPageButton = FindButtonByStrings(driver, new[] { "View next page" });
                        if (nextPageButton != null) nextPageButton.Click();
                    }

                    continue;
                }
                else
                {
                    currentJob.Click();
                    break;
                }
               
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine(ex.Message);
                currentJob.Click();
                break;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return false;
    }

    return true;
}

//driver.Dispose();