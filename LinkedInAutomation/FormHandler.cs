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
            { "CoverLetter", "oliviagates16@gmail.com" },
            { "phone", "3347479687" },
            { "countryCode", "+92" },
            { "Address", "Lahore, Pakistan" },
            { "Summary", "Full Stack .Net Devloper with 3 year of experience." }
        };

            var phoneField = FindInputFieldByLabelContains(driver, ["phone", "Phone", "tel"]);
            if (phoneField != null) phoneField.SendKeys(formData["phone"]);

            var addressField = FindInputFieldByLabelContains(driver, ["Address", "address", "ADDRESS"]);
            if (addressField != null) addressField.SendKeys(formData["Address"]);

            var summeryField = FindInputFieldByLabelContains(driver, ["Summary", "summary", "SUMMARY"]);
            if (summeryField != null) summeryField.SendKeys(formData["Summary"]);

            var CoverLetterField = FindInputFieldByLabelContains(driver, ["Cover letter", "Cover letter", "COVER LETTER"]);
            if (CoverLetterField != null) CoverLetterField.SendKeys(formData["CoverLetter"]);
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

    static IWebElement? FindInputFieldByLabelContains(IWebDriver driver, string[] partialAttributeNames)
    {
        foreach (var partialAttributeName in partialAttributeNames)
        {
            try
            {
                IWebElement labelElement = driver.FindElement(By.XPath($"//label[contains(text(), '{partialAttributeName}')]"));

                if (labelElement != null)
                {
                    try
                    {
                        string inputId = labelElement.GetAttribute("for");

                        if (!string.IsNullOrEmpty(inputId))
                        {
                            IWebElement inputElement = driver.FindElement(By.Id(inputId));

                            if (inputElement != null && string.IsNullOrEmpty(inputElement.GetAttribute("value")))
                            {
                                return inputElement;
                            }
                        }

                    }
                    catch (NoSuchElementException)
                    {
                        Console.WriteLine($"Input attach with label {partialAttributeName} not found.");
                    }
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Label containing text '{partialAttributeName}' not found.");
            }
        }

        return null;
    }

}
