/* *************************************
  Generate Action Block
**************************************** */

const indentString = require('indent-string');
var _ = require('lodash');
var util = require('../utils/util');

module.exports = function(test, meta, indent) {

  var actionBlock = "";

  test.actions.forEach((action, idx) => {
    var newLine = generateLine(action, meta, indent, idx);
    if (newLine) actionBlock += newLine;
    if (idx < test.actions.length - 1 && newLine) actionBlock += "\n";
  });

  return actionBlock;

};

var actions = {
  "FULL_PAGELOAD": {
    render: (action, description) => `
      LoadPage(${buildValueString(action.value)}, ${buildBooleanString(action.complete)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "PAUSE": {
    render: (action, description) => `
      Pause(${action.value}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "INPUT": {
    render: (action, description) => `
      SetInputValue(${genSelector(action)}, ${buildValueString(action.value)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "EL_PRESENT_ASSERT": {
    render: (action, description) => `
      AssertElementPresent(${genSelector(action)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "EL_NOT_PRESENT_ASSERT": {
    render: (action, description) => `
      AssertElementNotPresent(${genSelector(action)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "TEXT_ASSERT": {
    render: (action, description) => `
      AssertText(${genSelector(action)}, ${buildValueString(action.value)}, ${buildBooleanString(action.regex)}, ${buildBooleanString(action.continueOnFail)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "VALUE_ASSERT": {
    render: (action, description) => `
      AssertValue(${genSelector(action)}, ${buildValueString(action.value)}, ${buildBooleanString(action.regex)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});  
    `
  },
  "PATH_ASSERT": {
    render: (action, description) => `
      AssertPath(${buildValueString(action.value)}, ${buildBooleanString(action.regex)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "STYLE_ASSERT": {
    render: (action, description) => `
      AssertStyle(${genSelector(action)}, ${buildValueString(action.style)},  ${buildValueString(action.value)}, ${buildBooleanString(action.regex)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "MOUSEDOWN": {
    render: (action, description) => `
      MouseClick(${genSelector(action)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "MOUSEOVER": {
    render: (action, description) => `
      MouseOver(${genSelector(action)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "DOUBLECLICK": {
    render: (action, description) => `
      DoubleClick(${genSelector(action)}, ${buildValueString(action.selector)}, ${buildValueString(action.selectorType)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "DIALOG": {
    render: (action, description) => `
      SetDialogResponses(${buildBooleanString(action.alert)}, ${action.confirm ? `"accept"` : `"reject"`}, ${action.prompt ? `"${action.promptResponse}"` : "null"}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "SUBMIT": {
    render: (action, description) => `
      SubmitForm(${genSelector(action)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "CLEAR_COOKIES": {
    render: (action, description) => `
      ClearCookies(${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "CLEAR_CACHES": {
    render: (action, description) => `
      ClearCaches(${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "FOCUS": {
    render: (action, description) => `
      Focus(${genSelector(action)}, ${buildValueString(action.selector)}, ${buildValueString(action.selectorType)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "BLUR": {
    render: (action, description) => `
      Blur(${genSelector(action)}, ${buildValueString(action.selector)}, ${buildValueString(action.selectorType)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "KEYDOWN": {
    render: (action, description) => `
      KeyDown(${genSelector(action)}, ${buildValueString(action.selector)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "EXECUTE_SCRIPT": {
    render: (action, description) => `
      ExecuteScript(${buildScriptString(action.script)}, ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "REFRESH": {
    render: (action, description) => `
      Navigate("refresh", ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "BACK": {
    render: (action, description) => `
      Navigate("back", ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "FORWARD": {
    render: (action, description) => `
      Navigate("forward", ${buildDescription(description)}${buildOptionalTimeout(action.timeout)});
    `
  },
  "COMPONENT": {
    render: (action, description, meta) => {
      var component = _.find(meta.components, {id: action.componentId});
      return `
       components.${component.name}(${buildComponentActionParams(action, component)});
      `
    }
  }
};



function genSelector(action) {
  switch (action.selectorType) {
    case "CSS":
      return `By.CssSelector(${buildValueString(action.selector)})`;
    case "XPATH":
      return `By.XPath(${buildValueString(action.selector)})`;
      case "ID":
      return `By.Id(${buildValueString(action.selector)})`;
    case "ATTR":
      return `By.CssSelector("[\'${action.selector}\']")`;
    case "NAME":
      return `By.Name(${buildValueString(action.selector)})`;
    case "TEXT":
      return `By.XPath(${buildValueString(`//*[contains(text(), '${action.selector}')]`) })`;
  }
}

function generateLine(action, meta, indent) {

  var exclude = ["URL_CHANGE_INDICATOR", "PUSHSTATE"];

  if (actions[action.type]) {
    var description = action.description || util.buildActionDescription(action);
    var line = actions[action.type].render(action, description, meta).trim();
    line = line.replace(/^ +/gm, '');
    line = indentString(line, indent);
    return line;
  }
  else if (exclude.indexOf(action.type) === -1)
    return indentString(`// No support for: Action ${action.type}`, indent);

}

function buildScriptString(script) {
  var escapedScriptString = script.replace(/"/g, "\"\"");
  return `@"${escapedScriptString}"`
}

function buildValueString(string) {
  if (!string) return string;
  return `$"${string.replace(/\$/g, "")}"`
}

function buildDescription(string) {
  if (!string) return string;
  return `$"${string.replace(/"/g, "\\\"").replace(/\$/g, "")}"`;
}

function buildOptionalTimeout(timeout) {
  if (timeout) return `, ${timeout}`;
  else return "";
}

function buildBooleanString(value) {
  return value ? "true" : "false";
}

function buildComponentActionParams(action, component) {

  var params = [];

  component.variables.forEach((variable, idx) => {

    var variableInAction = _.find(action.variables, {id: variable.id});

    if (variableInAction) {
      params.push(`$"${variableInAction.value}"`);
    } else {
      params.push(`$"${variable.defaultValue}"`);
    }

  });

  return params.join(", ");

}