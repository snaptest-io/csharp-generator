using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

public class <%= suiteName %> : SnapTestBase
{

  public <%= suiteName %>()
  {
		SetupSuite();
		StartSuite("<%= suiteName %>");
  }
  <% for(var i=0; i < tests.length; i++ ) {%>
  [Fact]
  public void <%= tests[i].testName %>()
  {

		StartTest("<%= tests[i].testName %>");
<% for(var j=0; j < tests[i].variables.length; j++ ) {%>
    String <%= tests[i].variables[j].name %> = testData["<%= tests[i].variables[j].name %>"] != null ? testData["<%= tests[i].variables[j].name %>"] : "<%= tests[i].variables[j].defaultValue %>";
<% } %>
<%- tests[i].generateActionBlock(4, " ") %>

		EndTest("<%= tests[i].testName %>");

  }
  <% } %>

}