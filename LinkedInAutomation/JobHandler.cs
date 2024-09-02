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
                    IWebElement? appliedStatus = currentJob.FindElement(By.XPath($".//li[contains(@class, 'job-card-container__footer-item')]"));

                    IWebElement? jobTitleElement = currentJob.FindElement(By.XPath(".//a[contains(@class, 'job-card-list__title')]//strong"));

                    var jobTitle = jobTitleElement.Text;

                    if (appliedStatus != null)
                    {
                        var text = appliedStatus.Text;

                        if (appliedStatus.Text.Contains("Applied"))
                        {
                            Console.WriteLine("Already applied to this job. Skipping...");
                        }
                        else
                        {
                            bool isJobSuitable = IsJobSuitable(driver, [".net", "c#", "asp.net"], ["react", "nestjs", "nextjs"]);

                            if (isJobSuitable)
                            {
                                currentJob.Click();
                                break;
                            }
                        }

                        if (i == jobListings.Count - 1)
                        {
                            var nextPageButton = ComponentHandler.FindButtonByStrings(driver, new[] { "View next page" });
                            if (nextPageButton != null) nextPageButton.Click();
                        }

                        JobClose(currentJob);
                        Thread.Sleep(3000);

                        continue;
                    }

                }
                catch (NoSuchElementException ex)
                {
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(10000);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Thread.Sleep(10000);

            return false;
        }

        return true;
    }

    public static void JobClose(IWebElement? currentJob)
    {
        try
        {
            var colseButton = currentJob!.FindElement(By.XPath($".//button[contains(@aria-label, 'Dismiss')]"));
            if (colseButton != null) colseButton.Click();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public static bool IsJobSuitable(IWebDriver driver, List<string> requiredSkills, List<string> notRequiredSkills)
    {
        try
        {
            var jobDescriptionSection = driver.FindElement(By.XPath("//*[contains(@class, 'jobs-description-content')]"));
            string jobDescription = jobDescriptionSection.Text.ToLower();

            int totalRequiredSkills = requiredSkills.Count;
            int foundRequiredSkills = 0;

            foreach (var skill in requiredSkills)
            {
                if (jobDescription.Contains(skill, StringComparison.OrdinalIgnoreCase))
                {
                    foundRequiredSkills++;
                }
            }

            double requiredSkillsPercentage = (double)foundRequiredSkills / totalRequiredSkills;

            bool containsNotRequiredSkills = notRequiredSkills.Any(skill => jobDescription.Contains(skill, StringComparison.OrdinalIgnoreCase));

            if (requiredSkillsPercentage >= 0.5 && !containsNotRequiredSkills)
            {
                return true;
            }
            else
            {
                if (requiredSkillsPercentage < 0.5)
                {
                    Console.WriteLine("The job description does not contain at least 70% of the required skills.");
                }

                if (containsNotRequiredSkills)
                {
                    Console.WriteLine("The job description contains one or more not required skills.");
                }

                return false;
            }
        }
        catch (NoSuchElementException ex)
        {
            Thread.Sleep(1000000);
            Console.WriteLine($"Element not found: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
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
