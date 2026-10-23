using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace FrontendTest;

public class KindergartenTest
{
    [Fact]
    public void Should_CreateKindergarten_And_ReturnToIndex_WhenDataIsValid()
    {
        var driver = SetupAndNavigateIndex();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        var createInIndex = wait.Until(
            ExpectedConditions.ElementToBeClickable(
                By.Id("CreateInIndex"))
        );
        createInIndex.Click();

        InsertKindergartenData(driver);

        Thread.Sleep(3000);

        var indexKindergartenName = driver.FindElement(By.Id("IndexKindergartenName"));
        var kindergartenNameData = indexKindergartenName.Text;

        var indexGroupName = driver.FindElement(By.Id("IndexGroupName"));
        var groupNameData = indexGroupName.Text;

        var indexChildrenCount = driver.FindElement(By.Id("IndexChildrenCount"));
        var childrenCountData = indexChildrenCount.Text;

        Assert.Equal("Päikesejänku lasteaed", kindergartenNameData);
        Assert.Equal("Tibud", groupNameData);
        Assert.Equal("12", childrenCountData);

        driver.Quit();
    }

    [Fact]
    public void Should_NavigateToKindergartenDetails_WhenKindergartenExists()
    {
        var driver = SetupAndNavigateIndex();

        var table = driver.FindElement(By.Id("IndexTable"));
        var rows = table.FindElements(By.TagName("tr")).ToList();
        rows.RemoveAt(0);

        var found = false;

        foreach (var row in rows)
        {
            var locadedElement = row.FindElement(By.Id("IndexKindergartenName"));

            if (locadedElement.Text == "Päikesejänku lasteaed")
            {
                var actionElement = row.FindElement(By.Id("IndexActionKindergarten"));
                var detailsButton = actionElement.FindElement(By.Id("IndexKindergartenDetails"));

                detailsButton.Click();
                found = true;
                break;
            }
        }
        Assert.True(found, "Kindergarten was not found in the table.");

        Thread.Sleep(3000);

        IWebElement details_KindergartenId = driver.FindElement(
            By.Id("Details_KindergartenId"));
        var details_id = details_KindergartenId.Text;

        IWebElement details_KindergartenName = driver.FindElement(
            By.Id("Details_KindergartenName"));
        var details_kindergartenName = details_KindergartenName.Text;

        IWebElement details_KindergartenGroupName = driver.FindElement(
            By.Id("Details_KindergartenGroupName"));
        var details_groupName = details_KindergartenGroupName.Text;

        IWebElement details_KindergartenChildrenCount = driver.FindElement(
            By.Id("Details_KindergartenChildrenCount"));
        var details_childrenCount = details_KindergartenChildrenCount.Text;

        IWebElement details_KindergartenTeacherName = driver.FindElement(
            By.Id("Details_KindergartenTeacherName"));
        var details_teacherName = details_KindergartenTeacherName.Text;

        Assert.False(string.IsNullOrEmpty(details_id));
        Assert.Equal("Päikesejänku lasteaed", details_kindergartenName);
        Assert.Equal("Tibud", details_groupName);
        Assert.Equal("12", details_childrenCount);
        Assert.Equal("Kadri Kuusk", details_teacherName);

        driver.Quit();
    }


    private static IWebDriver SetupAndNavigateIndex()
    {
        var options = new FirefoxOptions();
        options.AddArgument("--headless");

        IWebDriver driver = new FirefoxDriver();
        driver.Url = "http://localhost:5277/";

        var navigateToSpaceship = driver.FindElement(By.LinkText("Kindergarten"));
        navigateToSpaceship.Click();

        return driver;
    }

    private void InsertKindergartenData(IWebDriver driver, bool isUpdate = false)
    {
        var kindergartenName = isUpdate ? "Võilille lasteaed" : "Päikesejänku lasteaed";
        var groupName = isUpdate ? "Liblikad" : "Tibud";
        var childrenCount = isUpdate ? "15" : "12";
        var teacherName = isUpdate ? "Mari Maasikas" : "Kadri Kuusk";
        var elementId = isUpdate ? "CU_UpdateKindergarten" : "CU_CreateKindergarten";

        SetInputValue(driver, By.Id("CU_KindergartenNameEntry"), kindergartenName);
        SetInputValue(driver, By.Id("CU_GroupNameEntry"), groupName);
        SetInputValue(driver, By.Id("CU_ChildrenCountEntry"), childrenCount);
        SetInputValue(driver, By.Id("CU_TeacherNameEntry"), teacherName);

        driver.FindElement(By.Id(elementId)).Click();
    }

    private void SetInputValue(IWebDriver driver, By locator, string value)
    {
        var element = driver.FindElement(locator);
        element.Clear();
        element.SendKeys(value);
    }
}