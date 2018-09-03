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

public class SnapComponents : SnapActionDriver
{

	public void Initialize(IWebDriver driver, WebDriverWait wait, SnapConfig snapConfig) {
    base.Initialize(driver, wait, snapConfig);
  }

	<% for(var i=0; i < components.length; i++ ) {%>
  public void <%= components[i].name %>(<%- components[i].params %>) {
<%- components[i].generateActionBlock(4, " ") %>
  }
  <% } %>

}