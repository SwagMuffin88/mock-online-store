using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace FrontendTest;

public class KindergartenTest
{
    
    
    private static IWebDriver SetupAndNavigateIndex()
    {
        FirefoxOptions options = new FirefoxOptions();
        options.AddArgument("--headless");

        IWebDriver driver = new FirefoxDriver();
        driver.Url = "http://localhost:5277/";

        IWebElement navigateToSpaceship = driver.FindElement(By.LinkText("Kindergarten"));
        navigateToSpaceship.Click();

        return driver;
    }
    
    private void InsertKindergartenData(IWebDriver driver, bool isUpdate = false)
    {
        string kindergartenName = isUpdate ? "Päikesejänku Lasteaed (Updated)" : "Päikesejänku Lasteaed";
        string groupName = isUpdate ? "Liblikad" : "Tibud";
        string childrenCount = isUpdate ? "15" : "12";
        string teacherName = isUpdate ? "Mari Maasikas" : "Kadri Kuusk";
        string elementId = isUpdate ? "CU_UpdateKindergarten" : "CU_CreateKindergarten";

        SetInputValue(driver, By.Id("CU_KindergartenNameEntry"), kindergartenName);
        SetInputValue(driver, By.Id("CU_GroupNameEntry"), groupName);
        SetInputValue(driver, By.Id("CU_ChildrenCountEntry"), childrenCount);
        SetInputValue(driver, By.Id("CU_TeacherNameEntry"), teacherName);

        driver.FindElement(By.Id(elementId)).Click();
    }
    
    private void SetInputValue(IWebDriver driver, By locator, string value)
    {
        IWebElement element = driver.FindElement(locator);
        element.Clear();
        element.SendKeys(value);
    }
}