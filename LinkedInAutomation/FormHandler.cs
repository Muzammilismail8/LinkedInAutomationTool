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
                { "Summary", ".NET developer with 3 years of experience in ASP.NET Core MVC, Web API, Angular, and SQL databases. Experienced in ERP, customer portals, and CRM projects." },
                { "Cover letter", "With 3 years of experience in .NET development, including expertise in ASP.NET Core MVC, Web API, Angular, and SQL databases, I am excited about the opportunity to contribute to your team. My background includes developing robust applications and working on ERP systems, customer portals, and CRM solutions. I am eager to bring my technical skills and passion for software development to a dynamic team and help achieve your company’s goals." },
                { "What's your preferred tech stack and why do you love it? Don't worry, no judgement if it's not ours.", "My preferred tech stack is ASP.NET Core, Angular, and SQL Server. I appreciate this stack for its flexibility, performance, and seamless integration. ASP.NET Core offers a robust backend framework, Angular enables dynamic and responsive front-ends, and SQL Server provides reliable data management and advanced features. This combination helps me build efficient and scalable applications." },
                { "Phone", "03347479687" },
                { "Address", "Lahore, Pakistan" },
                { "years of work experience do you have with Software Engineering Practices", "4" },
                { "Current Salary", "160000" },
                { "Expected Salary", "230000" },
                { "years of work experience do you have with SQL Server", "3" },
                { "your notice period", "30" },
                { "How many years of work experience do you have with T-SQL Stored Procedures", "0" },
                { "How many years of work experience do you have with HTML/CSS Validation", "3" },
                { "How many years of work experience do you have with SQL Database Administration", "1" },
                { "Zip or Postal Code", "5400" },
                { "How many years of work experience do you have with WebRTC", "0" },
                { "How many years of work experience do you have with Azure DevOps Services", "0" },
                { "How many years of work experience do you have with Continuous Integration and Continuous Delivery (CI/CD)", "0" },
                { "How many years of work experience do you have with Redis", "0" },
                { "How many years of work experience do you have with Azure Cosmos DB", "0" },
                { "How does our mission resonate with you", "Because I have experience working in the same domain as you." },
                { "How many years of work experience do you have with Unified Modeling Language (UML)", "3" },
                { "How many years of work experience do you have with Sybase Products", "3" },
                { "How many years of work experience do you have with UML Tools", "3" },
                { "How many years of work experience do you have with Microsoft Power Pages", "0" },
                { "How many years of work experience do you have with SQL", "3" },
                { "How many years of work experience do you have with Microsoft Power Apps", "0" },
                { "How many years of work experience do you have with Spring Framework", "0" },
                { "City", "Lahore, Pakistan" },
                { "Desired salary", "230000" },
                { "How many years of work experience do you have with Software as a Service (SaaS)", "3" },
                { "How many years of work experience do you have with Java", "0" },
                { "How many years of experience do you have using automated testing tools (such as Cypress or Selenium), professionally", "1" },
                { "How many years of work experience do you have with JavaScript", "1" },
                { "How many years of experience do you have using Python, professionally", "0" },
                { "How many years of experience do you have using Django, professionally", "0" },
                { "How many years of experience do you have using React, professionally", "0" },
                { "How many years of experience do you have using Vue, professionally", "0" },
                { "How many years of experience do you have using SQL, professionally", "3" },
                { "How many years of experience do you have using PostgreSQL, professionally", "2" },
                { "How many years of work experience do you have with WPF Development", "0" },
                { "How many years of work experience do you have with PyTorch", "0" },
                { "How many years of work experience do you have with VBScript", "0" },
                { "How many years of experience do you have in back end development in C# in .NET", "3" },
                { "How many years of work experience do you have with Financial Services", "2" },
                { "How many years of work experience do you have with API Development", "3" },
                { "How many years of work experience do you have with Kotlin", "0" },
                { "How many years of work experience do you have with GitHub Actions", "0" },
                { "How many years of work experience do you have with Design Systems", "3" },
                { "Do you have JavaScript experience (Vue.js or React.js)", "0" },
                { "What are your salary expectations", "230000" },
                { "How many years of work experience do you have with RDBMS", "3" },
                { "How many years of work experience do you have with gRPC", "0" },
                { "How many years of Oil and Gas experience do you currently have", "0" },
                { "How many years of work experience do you have with .NET?", "3" },
                { "How many years of work experience do you have with .NET Core", "3" },
                { "How many years of work experience do you have with Angular", "1" },
                { "How many years of work experience do you have with HTML5", "4" }
            };


            // Loop through formData to fill the form dynamically
            foreach (var entry in formData)
            {
                var labelKeywords = new[] { entry.Key };
                IWebElement? inputField = null;
                
                try
                {
                inputField = FindInputFieldByLabelContains(driver, labelKeywords);
                }
                catch (Exception)
                {
                    inputField = null;
                }

                if (inputField != null)
                {
                    inputField.Clear();
                    inputField.SendKeys(entry.Value);
                }
            }
        }
        catch (Exception ex) { }
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
                    catch (NoSuchElementException) {
                        return null;
                    }
                }
            }
            catch (NoSuchElementException) {
                return null;
            }
        }

        return null;
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
