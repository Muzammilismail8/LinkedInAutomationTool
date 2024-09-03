using OpenQA.Selenium;

namespace LinkedInAutomation;

public static class ComponentHandler
{
    public static void FindAndClickEasyApplyButton(IWebDriver driver)
    {
        try
        {
            JobHandler.FindNextJobtoApply(driver);
            IWebElement? easyApplyButton = null;

            bool isJobListingExist = true;

            while (easyApplyButton == null && isJobListingExist)
            {
                var jobDetails = driver.FindElement(By.XPath($"//div[contains(@class, 'job-details')]"));

                try
                {
                    easyApplyButton = jobDetails.FindElement(By.XPath($".//button[contains(@aria-label, 'Easy Apply')]"));
                }
                catch (Exception)
                {
                    easyApplyButton = null;
                }

                //easyApplyButton = FindButtonByStrings(driver, ["Easy Apply"]);

                if (easyApplyButton == null)
                {
                    isJobListingExist = JobHandler.FindNextJobtoApply(driver);
                }
            }

            if (easyApplyButton != null) easyApplyButton.Click();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Easy Apply not fount...");
        }
    }

    public static IWebElement? FindButtonByStrings(IWebDriver driver, string[] partialAttributeNames)
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
    public static IWebElement? GetDivByClass(IWebDriver driver, string[] partialAttributeNames)
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

}
