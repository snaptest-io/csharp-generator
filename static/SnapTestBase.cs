using NUnit.Framework;
using NUnit;
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

public abstract class SnapTestBase : SnapActionDriver {

  public IWebDriver driver;
  public DesiredCapabilities capabilities;
  public IConfigurationRoot testData;
  public SnapConfig snapConfig;
  public WebDriverWait wait;
  public SnapComponents components;
  public int globalTimeout;

	/* **** Basic test setup **** */

  public void SetupSuite() {

    snapConfig = new SnapConfig();
		capabilities = snapConfig.getCapabilities();
		driver = snapConfig.getDriver(capabilities);
		testData = snapConfig.getTestData();
		globalTimeout = getGlobalTimeout();
		wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(globalTimeout));
		components = new SnapComponents();
		components.Initialize(driver, wait, snapConfig);

		base.Initialize(driver, wait, snapConfig);

  }

  public int getGlobalTimeout() {

    int timeout;

    if (testData["timeout"] != null) {
      if (Int32.TryParse(testData["timeout"], out timeout)) {
        return timeout;
      }
    }

    return 10000;

  }

	/* **** Test life-cycles hooks **** */

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

}
