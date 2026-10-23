using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace FrontendTest;

[TestCaseOrderer(
    ordererTypeName: "FrontendTest.PriorityOrderer",
    ordererAssemblyName: "FrontendTest")]
public class KindergartenTest
{
    [Fact]
    [Order(1)]
    /* Navigate from index to kindergarten creation page, insert valid data and submit. Confirm that the new
     item exists in index table after redirecting. */
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
    [Order(2)]
    /* Find a specific kindergarten item from index table, navigate to its details page, confirm the data on the page. */
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

    [Fact]
    [Order(3)]
    /* Find a predefined existing test kindergarten item in the index table, navigate to the update view of that
     specific item. Update existing item and verify changes in details view. */
    public void Should_UpdateKindergartenDetails_WhenDataIsValid()
    {
        IWebDriver driver = SetupAndNavigateIndex();

        var table = driver.FindElement(By.Id("IndexTable"));
        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
        rows.RemoveAt(0);

        foreach (var row in rows)
        {
            var locatedelement = row.FindElement(By.Id("IndexKindergartenName"));

            if (locatedelement.Text == "Päikesejänku lasteaed")
            {
                var rowElement = row.FindElement(By.Id("IndexActionKindergarten"));
                var button = rowElement.FindElement(By.Id("IndexKindergartenUpdate"));
                
                button.Click();
                Thread.Sleep(3000);
                break;
            }
        }
        
        InsertKindergartenData(driver, true);
        Thread.Sleep(3000);

        var detailsListContainer = driver.FindElement(By.Id("Details_List"));
        
        string updatedName = detailsListContainer.FindElement(By.Id("Details_KindergartenName")).Text;
        string updatedGroupName = detailsListContainer.FindElement(By.Id("Details_KindergartenGroupName")).Text;
        string updatedChildrenCount = detailsListContainer.FindElement(By.Id("Details_KindergartenChildrenCount")).Text;
        string updatedTeacherName = detailsListContainer.FindElement(By.Id("Details_KindergartenTeacherName")).Text;
        
        Assert.Equal("Võilille lasteaed", updatedName);
        Assert.Equal("Liblikad", updatedGroupName);
        Assert.Equal("15", updatedChildrenCount);
        Assert.Equal("Mari Maasikas", updatedTeacherName);
    
        driver.Quit();
    }

    [Fact]
    [Order(4)]
    /* Delete a specific kindergarten item and confirm its deletion in index view after redirection. */
    public void Should_DeleteKindergarten_WhenKindergartenExists()
    {
        IWebDriver driver = SetupAndNavigateIndex();

        var table = driver.FindElement(By.Id("IndexTable"));
        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
        rows.RemoveAt(0);
        
        bool found = false;

        foreach (var row in rows)
        {
            var locatedelement = row.FindElement(By.Id("IndexKindergartenName"));

            if (locatedelement.Text == "Võilille lasteaed")
            {
                var rowElement = row.FindElement(By.Id("IndexActionKindergarten"));
                var button = rowElement.FindElement(By.Id("IndexKindergartenDelete"));
                button.Click();

                found = true;
                Thread.Sleep(3000);
                break;
            }
        }
        Assert.True(found, "Kindergarten was not found in the index table.");

        var deleteButton = driver.FindElement(By.Id("Delete_Delete"));
        deleteButton.Click();
        
        Thread.Sleep(3000);

        var updatedTable = driver.FindElement(By.Id("IndexTable"));
        List<IWebElement> updatedRows = updatedTable.FindElements(By.TagName("tr")).ToList();
        updatedRows.RemoveAt(0);
        
        bool stillExists = false;
        
        foreach (var row in updatedRows)
        {
            var nameElement = row.FindElement(By.Id("IndexKindergartenName"));

            if (nameElement.Text == "Võilille lasteaed")
            {
                stillExists = true;
                break;
            }
        }
        
        Assert.False(stillExists, 
            "Kindergarten 'Võilille lasteaed' was successfully deleted, but still appears in the table.");
        
        driver.Quit();
    }

    [Fact]
    [Order(5)]
    /* Submit an empty kindergarten item and confirm its non-creation. */
    public void ShouldNot_CreateKindergarten_WhenRequiredFieldsAreEmpty()
    {
        var driver = SetupAndNavigateIndex();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        
        var createInIndex = wait.Until(
            ExpectedConditions.ElementToBeClickable(By.Id("CreateInIndex"))
        );
        createInIndex.Click();
        
        var createButton = driver.FindElement(By.Id("CU_CreateKindergarten"));
        createButton.Click();

        Thread.Sleep(2000);
        
        driver.Navigate().GoToUrl("http://localhost:5277/Kindergarten");
        
        var table = driver.FindElement(By.Id("IndexTable"));
        var rows = table.FindElements(By.TagName("tr")).ToList();
        
        Assert.True(rows.Count <= 2, "Validation failed: Empty kindergarten should not be created.");
        
        driver.Quit();
    }

    [Fact]
    [Order(6)]
    /* Navigate to create page and navigate back to index view without submitting anything. */
    public void Should_NavigateBackToIndex_WhenCancelOrBackIsClicked()
    {
        var driver = SetupAndNavigateIndex();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        var createInIndex = wait.Until(
            ExpectedConditions.ElementToBeClickable(By.Id("CreateInIndex")));
        createInIndex.Click();

        var backButton = wait.Until(
            ExpectedConditions.ElementToBeClickable(By.Id("CU_Back")));
        backButton.Click();
        
        Thread.Sleep(2000);
        
        Assert.Contains("/kindergarten", driver.Url);
        
        driver.Quit();
    }

    [Fact]
    [Order(7)]
    /* Create two kindergarten items and confirm they both appear in the index table. */
    public void Should_CreateMultipleKindergartens_AndDisplayAllInIndexTable()
    {
        var driver = SetupAndNavigateIndex();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        
        driver.FindElement(By.Id("CreateInIndex")).Click();
        
        SetInputValue(driver, By.Id("CU_KindergartenNameEntry"), "Esimene lasteaed");
        SetInputValue(driver, By.Id("CU_GroupNameEntry"), "Tõrukesed");
        SetInputValue(driver, By.Id("CU_ChildrenCountEntry"), "10");
        SetInputValue(driver, By.Id("CU_TeacherNameEntry"), "Anne Anni");
        
        driver.FindElement(By.Id("CU_CreateKindergarten")).Click();
        Thread.Sleep(2000);
        
        driver.FindElement(By.Id("CreateInIndex")).Click();
        
        SetInputValue(driver, By.Id("CU_KindergartenNameEntry"), "Teine lasteaed");
        SetInputValue(driver, By.Id("CU_GroupNameEntry"), "Mesilased");
        SetInputValue(driver, By.Id("CU_ChildrenCountEntry"), "14");
        SetInputValue(driver, By.Id("CU_TeacherNameEntry"), "Mai Tomingas");
        
        driver.FindElement(By.Id("CU_CreateKindergarten")).Click();
        Thread.Sleep(2000);
        
        var table = driver.FindElement(By.Id("IndexTable"));
        var pageSource = table.Text;

        Assert.Contains("Esimene lasteaed", pageSource);
        Assert.Contains("Teine lasteaed", pageSource);
    }

    [Fact]
    [Order(8)]
    /* Create a test kindergarten and navigate to its delete page. Cancel deletion and confirm the kindergarten still
     exists in index table. */
    public void Should_CancelDeletion_WhenDeleteConfirmationIsAborted()
    {
        var driver = SetupAndNavigateIndex();
        
        driver.FindElement(By.Id("CreateInIndex")).Click();
        InsertKindergartenData(driver, false);
        
        Thread.Sleep(2000);
        
        var table = driver.FindElement(By.Id("IndexTable"));
        var rows = table.FindElements(By.TagName("tr")).ToList();
        rows.RemoveAt(0);

        foreach (var row in rows)
        {
            if (row.FindElement(By.Id("IndexKindergartenName")).Text == "Päikesejänku lasteaed")
            {
                row.FindElement(By.Id("IndexActionKindergarten"))
                    .FindElement(By.Id("IndexKindergartenDelete"))
                    .Click();
                Thread.Sleep(2000);
                break;
            }
        }

        var cancelButton = driver.FindElement(By.Id("Delete_Back"));
        cancelButton.Click();
        Thread.Sleep(2000);

        var updatedTable = driver.FindElement(By.Id("IndexTable"));

        Assert.Contains("Päikesejänku lasteaed", updatedTable.Text);
        
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