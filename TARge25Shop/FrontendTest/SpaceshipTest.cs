using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace FrontendTest;

[TestCaseOrderer(
    ordererTypeName: "FrontendTest.PriorityOrderer",
    ordererAssemblyName: "FrontendTest")]
public class SpaceshipTest
{
    [Fact]
    [Order(1)]
    public void Should_NavigateToCreate_AddSpaceship_WithCorrectData_ReturnToIndex()
    {
        // Setup
        IWebDriver driver = SetupAndNavigateIndex();

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        // Action
        IWebElement createInIndex = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("CreateInIndex")));
        createInIndex.Click();

        // Input data
        InsertSpaceshipData(driver);

        Thread.Sleep(3000);

        // Data collected after reload
        IWebElement indexNameSpaceship = driver.FindElement(By.Id("IndexNameSpaceship"));
        var spaceShipNameData = indexNameSpaceship.Text;

        IWebElement indexTypeSpaceship = driver.FindElement(By.Id("IndexTypeSpaceship"));
        var spaceShipTypeData = indexTypeSpaceship.Text;

        IWebElement indexCrewSpaceship = driver.FindElement(By.Id("IndexCrewSpaceship"));
        var spaceShipCrewData = indexCrewSpaceship.Text;

        // Verification
        Assert.Equal("Apollo 11", spaceShipNameData);
        Assert.Equal("Command Module", spaceShipTypeData);
        Assert.Equal("3", spaceShipCrewData);

        driver.Quit();
    }

    [Fact]
    [Order(2)]
    public void Should_NavigateToDetails_OfASpaceShip_WithPreviouslyCorrectData()
    {
        IWebDriver driver = SetupAndNavigateIndex();

        var table = driver.FindElement(By.Id("IndexTable"));
        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
        rows.RemoveAt(0);

        bool found = false;

        foreach (var row in rows)
        {
            var locadedElement = row.FindElement(By.Id("IndexNameSpaceship"));

            if (locadedElement.Text == "Apollo 11")
            {
                var actionElement = row.FindElement(By.Id("IndexActionsSpaceship"));
                var detailsButton = actionElement.FindElement(By.Id("IndexSpaceshipDetails"));
                detailsButton.Click();

                found = true;
                break;
            }
        }

        Assert.True(found, "Spaceship was not found in the table.");

        Thread.Sleep(3000);

        IWebElement details_SpaceshipId = driver.FindElement(By.Id("Details_SpaceshipId"));
        var details_id = details_SpaceshipId.Text;

        IWebElement details_SpaceshipName = driver.FindElement(By.Id("Details_SpaceshipName"));
        var details_name = details_SpaceshipName.Text;

        IWebElement details_SpaceshipType = driver.FindElement(By.Id("Details_SpaceshipType"));
        var details_type = details_SpaceshipType.Text;

        IWebElement details_SpaceshipCrew = driver.FindElement(By.Id("Details_SpaceshipCrew"));
        var details_crew = details_SpaceshipCrew.Text;

        IWebElement details_SpaceshipPower = driver.FindElement(By.Id("Details_SpaceshipPower"));
        var details_power = details_SpaceshipPower.Text;

        Assert.False(string.IsNullOrEmpty(details_id));
        Assert.Equal("Apollo 11", details_name);
        Assert.Equal("Command Module", details_type);
        Assert.Equal("3", details_crew);
        Assert.Equal("5000", details_power);

        driver.Quit();
    }

    [Fact]
    [Order(3)]
    public void Should_UpdateDetails_OfASpaceShip_WithNewData()
    {
        // TODO NB: controller now redirects to Details view, not Index. Test will fail.
        IWebDriver driver = SetupAndNavigateIndex();

        var table = driver.FindElement(By.Id("IndexTable"));
        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
        rows.RemoveAt(0);

        foreach (var row in rows)
        {
            var locatedelement = row.FindElement(By.Id("IndexNameSpaceship"));
            if (locatedelement.Text == "Apollo 11")
            {
                var rowelement = row.FindElement(By.Id("IndexActionsSpaceship"));
                var button = rowelement.FindElement(By.Id("IndexSpaceshipUpdate"));
                button.Click();
                Thread.Sleep(500);
                break;
            }
        }

        InsertSpaceshipData(driver, true);

        Thread.Sleep(3000);

        var updatedTable = driver.FindElement(By.Id("IndexTable"));
        List<IWebElement> updatedRows = updatedTable.FindElements(By.TagName("tr")).ToList();
        updatedRows.RemoveAt(0);

        bool updatedFound = false;
        string updatedName = "";
        string updatedType = "";
        string updatedCrew = "";

        foreach (var row in updatedRows)
        {
            var nameElement = row.FindElement(By.Id("IndexNameSpaceship"));
            if (nameElement.Text == "Enterprise-D")
            {
                updatedName = nameElement.Text;
                updatedType = row.FindElement(By.Id("IndexTypeSpaceship")).Text;
                updatedCrew = row.FindElement(By.Id("IndexCrewSpaceship")).Text;
                updatedFound = true;
                break;
            }
        }

        Assert.True(updatedFound, "Updated spaceship 'Enterprise-D' was not found in the index table.");
        Assert.Equal("Enterprise-D", updatedName);
        Assert.Equal("Explorer", updatedType);
        Assert.Equal("5", updatedCrew);

        driver.Quit();
    }

    [Fact]
    [Order(4)]
    public void Should_DeleteSpaceship_FromAppVIADeleteView()
    {
        IWebDriver driver = SetupAndNavigateIndex();

        var table = driver.FindElement(By.Id("IndexTable"));
        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
        rows.RemoveAt(0);

        bool found = false;

        foreach (var row in rows)
        {
            var locatedelement = row.FindElement(By.Id("IndexNameSpaceship"));

            if (locatedelement.Text == "Enterprise-D")
            {
                var rowelement = row.FindElement(By.Id("IndexActionsSpaceship"));
                var button = rowelement.FindElement(By.Id("IndexSpaceshipDelete"));
                button.Click();

                found = true;
                Thread.Sleep(500);
                break;
            }
        }

        Assert.True(found, "Spaceship to delete was not found in the table.");

        var clickthisbutton = driver.FindElement(By.Id("Delete_Delete"));
        clickthisbutton.Click();

        Thread.Sleep(3000);

        var updatedTable = driver.FindElement(By.Id("IndexTable"));
        List<IWebElement> updatedRows = updatedTable.FindElements(By.TagName("tr")).ToList();
        updatedRows.RemoveAt(0);

        bool stillExists = false;
        foreach (var row in updatedRows)
        {
            var nameElement = row.FindElement(By.Id("IndexNameSpaceship"));
            if (nameElement.Text == "Enterprise-D")
            {
                stillExists = true;
                break;
            }
        }

        Assert.False(stillExists, "Spaceship 'Enterprise-D' was successfully deleted, but still appears in the table.");

        driver.Quit();
    }

    private static IWebDriver SetupAndNavigateIndex()
    {
        FirefoxOptions options = new FirefoxOptions();
        options.AddArgument("--headless");

        IWebDriver driver = new FirefoxDriver();
        driver.Url = "http://localhost:5277/";

        IWebElement navigateToSpaceship = driver.FindElement(By.LinkText("Spaceship"));
        navigateToSpaceship.Click();

        return driver;
    }

    private void InsertSpaceshipData(IWebDriver driver, bool isUpdate = false)
    {
        string name = isUpdate ? "Enterprise-D" : "Apollo 11";
        string shipType = isUpdate ? "Explorer" : "Command Module";
        string crew = isUpdate ? "5" : "3";
        string enginePower = isUpdate ? "9000" : "5000";
        string elementId = isUpdate ? "CU_UpdateSpaceship" : "CU_CreateSpaceship";

        SetInputValue(driver, By.Id("CU_NameEntrySpaceship"), name);
        SetInputValue(driver, By.Id("CU_ShipTypeEntrySpaceship"), shipType);
        SetInputValue(driver, By.Id("CU_CrewEntrySpaceship"), crew);
        SetInputValue(driver, By.Id("CU_EnginePowerEntrySpaceship"), enginePower);


        driver.FindElement(By.Id(elementId)).Click();
    }

    private void SetInputValue(IWebDriver driver, By locator, string value)
    {
        IWebElement element = driver.FindElement(locator);
        element.Clear();
        element.SendKeys(value);
    }
}