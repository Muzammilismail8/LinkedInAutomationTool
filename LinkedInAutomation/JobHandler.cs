using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace LinkedInAutomation;

public static class JobHandler
{
    public static void ApplyOnCurrentJob(IWebDriver driver)
    {
        try
        {
            ComponentHandler.FindAndClickEasyApplyButton(driver);

            Thread.Sleep(2000);
            IWebElement? reviewButton;

            IWebElement? nextButton;
            IWebElement? IsErrorExist = null;

            reviewButton = ComponentHandler.FindButtonByStrings(driver, ["Review your application"]);
            nextButton = ComponentHandler.FindButtonByStrings(driver, ["Continue to next step"]);

            while ((reviewButton == null || IsErrorExist != null) && nextButton != null)
            {
                if (reviewButton == null) nextButton.Click();

                while (IsErrorExist != null)
                {
                    FormHandler.FillForm(driver);
                    IsErrorExist = ComponentHandler.GetDivByClass(driver, ["error", "preview__response--is-required"]);
                }

                reviewButton = ComponentHandler.FindButtonByStrings(driver, ["Review your application"]);
                if (reviewButton != null) reviewButton.Click();

                IsErrorExist = ComponentHandler.GetDivByClass(driver, ["error", "preview__response--is-required"]);
                Thread.Sleep(2000);
            }

            var submitButton = ComponentHandler.FindButtonByStrings(driver, ["Submit application"]);
            if (submitButton != null) submitButton.Click();

            Thread.Sleep(3000);
            ColsetheApplicationSentModal(driver);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public static bool FindNextJobtoApply(IWebDriver driver)
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
                            var nextPageButton = ComponentHandler.FindButtonByStrings(driver, new[] { "View next page" });
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
    public static void ColsetheApplicationSentModal(IWebDriver driver)
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
                IWebElement? dismissButton = ComponentHandler.FindButtonByStrings(driver, ["Dismiss"]);

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
}
