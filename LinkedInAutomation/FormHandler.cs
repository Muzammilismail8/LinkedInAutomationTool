using OpenQA.Selenium;

namespace LinkedInAutomation;

public static class FormHandler
{
    public static void FillForm(IWebDriver driver)
    {
        try
        {
            var formData = new Dictionary<string, string>
        {
            { "email", "oliviagates16@gmail.com" },
            { "phone", "3347479687" },
            { "countryCode", "+92" },
            { "Address", "Lahore, Pakistan" },
            { "Summary", "Full Stack .Net Devloper with 3 year of experience." }
        };

            var phoneField = FindEmptyInputFieldByStrings(driver, ["phone", "Phone", "tel"]);
            if (phoneField != null) phoneField.SendKeys(formData["phone"]);

            var addressField = FindEmptyInputFieldByStrings(driver, ["Address", "address", "ADDRESS"]);
            if (phoneField != null) phoneField.SendKeys(formData["Address"]);

            var summeryField = FindEmptyInputFieldByStrings(driver, ["Summary", "summary", "SUMMARY"]);
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
}
