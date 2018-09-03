using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System.Text.RegularExpressions;

public abstract class SnapActionDriver : IDisposable  {

  public IWebDriver driver;
  public WebDriverWait wait;
  public SnapConfig snapConfig;

  public String random1;
  public String random2;
  public String random3;

  public String random {
    get {
      Random rnd = new Random();
      return $"{rnd.Next(1000000)}";
    }
  }

  public void Dispose() {
    driver.Quit();
  }

  public void Initialize(IWebDriver d, WebDriverWait w, SnapConfig s) {
		wait = w;
		driver = d;
		snapConfig = s;
		buildVariables();
  }

  public void buildVariables() {
    Random rnd = new Random();
    random1 = $"{rnd.Next(1000000)}";
    random2 = $"{rnd.Next(1000000)}";
    random3 = $"{rnd.Next(1000000)}";
  }

	/* **** Utility methods **** */

  public void EnsurePageScripts() {

		String pageScriptType = ((IJavaScriptExecutor) driver).ExecuteScript("return typeof snptGetElement").ToString();

    if (pageScriptType != "function") {
	    String injectedScript = @"(function() {var w = window, d = w.document;function xp(x) { var r = d.evaluate(x, d.children[0], null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null); return r.snapshotItem(0) };return w[""snptGetElement""] = function(s, t) {try {return t === ""XPATH"" ? xp(s) :t === ""ID"" ? d.querySelector(""#"" + s) :t === ""ATTR"" ? d.querySelector(""["" + s + ""]"") :t === ""NAME"" ? d.querySelector(""[name=\"""" + s + ""\""]"") :t === ""TEXT"" ? xp(""/[contains(text(), '"" + s + ""')]""): d.querySelector(s); }catch(e) {return null}}})();";
	    ((IJavaScriptExecutor) driver).ExecuteScript(injectedScript);
		}

  }

  public WebDriverWait getWait(int customTimeout) {
    if (customTimeout > 0) {
      WebDriverWait customWait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(customTimeout));
      return customWait;
    } else {
      return wait;
    }
  }

	public void StartSuite(String suiteName) {
    snapConfig.StartSuite(suiteName);
  }

  public void EndSuite(String suiteName) {
    snapConfig.EndSuite(suiteName);
  }

  public void StartTest(String testName) {
    snapConfig.StartTest(testName);
  }

  public void EndTest(String testName) {
    snapConfig.EndTest(testName);
  }

  public void reportAction(String description) {
    snapConfig.reportAction(description);
  }

  /*  **** Actions: **** */

	public void LoadPage(String url, Boolean requireComplete, String description = "No description.", int timeout = 0) {

		reportAction(description);

    driver.Url = url;

    if (requireComplete) {
      getWait(timeout).Until(d => ((IJavaScriptExecutor) d).ExecuteScript("return document.readyState").Equals("complete"));
    }

	}

	public void Pause(int milliseconds, String description = "No description.") {

    reportAction(description);

    Thread.Sleep(milliseconds);

  }

  public void SetInputValue(By selector, String value, String description = "No description.", int timeout = 0) {

    var element = driver.FindElement(selector);

    reportAction(description);
    getWait(timeout).Until(d => element);

    if (element.TagName == "select") {
      var selectElement = new SelectElement(element);
      selectElement.SelectByValue(value);
    } else {
      element.Clear();
      element.SendKeys(value);
    }

  }

	public void MouseClick(By selector, String value, String description = "No description.", int timeout = 0) {

    reportAction(description);

    getWait(timeout).Until(d => driver.FindElement(selector));
    driver.FindElement(selector).Click();

  }

	public void MouseOver(By selector, String description = "No description.", int timeout = 0) {

    reportAction(description);

    getWait(timeout).Until(d => driver.FindElement(selector));
    IWebElement element = driver.FindElement(selector);
    Actions action = new Actions(driver);
    action.MoveToElement(element).Build().Perform();

  }

  public void DoubleClick(By selector, String Selector, String SelectorType, String description = "No description.", int timeout = 0) {

    reportAction(description);

    EnsurePageScripts();
    getWait(timeout).Until(d => driver.FindElement(selector));

    ((IJavaScriptExecutor) driver).ExecuteScript(
      $@"
        var clickEvent = document.createEvent('MouseEvents');
        clickEvent.initEvent(""dblclick"", true, true);
        snptGetElement(""{Selector}"", ""{SelectorType}"").dispatchEvent(clickEvent);
      "
    );

  }

  public void SetDialogResponses(Boolean isAlertSkipped = false, String confirmResponse = "none", String promptResponse = "", String description = "No description.") {

    reportAction(description);

    String execute = "";

    if (isAlertSkipped != null) execute += "window.alert = function() {};";
    if (confirmResponse != "none") {
      var confirmReturnValue = confirmResponse == "accept" ? "true" : "false";
      execute += $"window.confirm = function() {{ return {confirmReturnValue} }};";
    }
    if (promptResponse != "") execute += $"window.prompt = function() {{ return \"{promptResponse}\" }};";

    if (execute != "") {
      ((IJavaScriptExecutor) driver).ExecuteScript(execute);
    }

  }

	public void SubmitForm(By selector, String value, String description = "No description.", int timeout = 0) {

    reportAction(description);

    getWait(timeout).Until(d => driver.FindElement(selector));
    driver.FindElement(selector).Submit();

  }

	public void ClearCookies(String description = "No description.") {

		reportAction(description);

		driver.Manage().Cookies.DeleteAllCookies();

	}

	public void ClearCaches(String description = "No description.") {

		reportAction(description);

		driver.Manage().Cookies.DeleteAllCookies();

	}

	public void Focus(By selector, String Selector, String SelectorType, String description = "No description.", int timeout = 0) {

		reportAction(description);

		getWait(timeout).Until(d => driver.FindElement(selector));
		EnsurePageScripts();

		((IJavaScriptExecutor) driver).ExecuteScript(
			$@"
	      var event = new FocusEvent(""focus"");
	      snptGetElement(""{Selector}"", ""{SelectorType}"").dispatchEvent(event);
	    "
    );

	}

	public void Blur(By selector, String Selector, String SelectorType, String description = "No description.", int timeout = 0) {

    reportAction(description);

    getWait(timeout).Until(d => driver.FindElement(selector));
    EnsurePageScripts();

		((IJavaScriptExecutor) driver).ExecuteScript(
			$@"
	      var event = new FocusEvent(""blur"");
	      snptGetElement(""{Selector}"", ""{SelectorType}"").dispatchEvent(event);
	    "
    );

  }

  public void KeyDown(By selector, String keyCode, String description = "No description.", int timeout = 0) {

    reportAction(description);

    getWait(timeout).Until(d => driver.FindElement(selector));
    // TODO: Make this.

  }

	public void ExecuteScript(String script, String description = "No description.", int timeout = 0) {

		reportAction(description);

		EnsurePageScripts();

		((IJavaScriptExecutor) driver).ExecuteScript(
			$@"
	      {script}
	    "
    );

  }

  public void Navigate(String action, String description = "No description.") {

    reportAction(description);

		if (action == "refresh") driver.Navigate().Refresh();
		if (action == "forward") driver.Navigate().Forward();
		if (action == "back") driver.Navigate().Back();

  }

	/* *** Assertions **** */

	public void AssertElementPresent(By selector, String description = "No description.", int timeout = 0) {

    reportAction(description);

    getWait(timeout).Until(d => driver.FindElement(selector));
    Assert.True(driver.FindElements(selector).Count > 0, description);

  }

  public void AssertElementNotPresent(By selector, String description = "No description.", int timeout = 0) {

    reportAction(description);

    getWait(timeout).Until(d => driver.FindElement(selector));
    Assert.True(driver.FindElements(selector).Count == 0, description);

  }

  public void AssertText(By selector, String expected, Boolean regex = false, Boolean continueOnFail = false, String description = "No description.", int timeout = 0) {

    reportAction(description);

		try {
			getWait(timeout).Until(d => driver.FindElement(selector));
		} catch (WebDriverTimeoutException e) {}

		Boolean passed = false;

		try {
			if (regex) {
				passed = (new Regex(expected)).IsMatch( driver.FindElement(selector).Text );
			} else {
				passed = (new Regex(expected)).IsMatch( driver.FindElement(selector).Text );
			}
		} catch (NoSuchElementException e) {}

    if (!continueOnFail) Assert.True(passed, description);

  }

  public void AssertValue(By selector, String expected, Boolean regex = false, String description = "No description.", int timeout = 0) {

    reportAction(description);

    getWait(timeout).Until(d => driver.FindElement(selector));
    IWebElement element = driver.FindElement(selector);
    String inputType = element.GetAttribute("type");
    String inputValue = element.GetAttribute((inputType == "checkbox" || inputType == "radio") ? "checked" : "value");

    if (regex) {
      Assert.True((new Regex(expected)).IsMatch(inputValue), description);
    } else {
      Assert.True(inputValue.Equals(expected), description);
    }

  }

  public void AssertPath(String expected, Boolean regex = false, String description = "No description.", int timeout = 0) {

    reportAction(description);

    if (regex) {
      getWait(timeout).Until(d => (new Regex(expected)).IsMatch(((IJavaScriptExecutor) driver).ExecuteScript("return window.location.pathname").ToString()));
    } else {
      getWait(timeout).Until(d => ((IJavaScriptExecutor) d).ExecuteScript("return window.location.pathname").Equals(expected));
    }

  }

  public void AssertStyle(By selector, String style, String expected, Boolean regex = false, String description = "No description.", int timeout = 0) {

    reportAction(description);

		getWait(timeout).Until(d => driver.FindElement(selector));

    IWebElement element = driver.FindElement(selector);
    Assert.True(element.GetCssValue(style).Equals(expected));

  }

}
