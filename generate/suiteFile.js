var util = require('../utils/util');
var generateActionBlock = require('./actionBlock');
var _ = require("lodash");
var ejs = require('ejs');
var fs = require('fs-extra');


module.exports = function(suiteName, tests, meta) {

  var driverString = fs.readFileSync(`${__dirname}/../templates/${meta.style}/SnapSuite.cs`, 'utf8');

  var rendered = ejs.render(driverString, {
    tests: tests.map((test) => {
      return {
        generateActionBlock: (indent, indentChar) =>  generateActionBlock(test, meta, indent, indentChar),
        baseUrl: _.find(test.variables, {name: "baseUrl"}),
        variables: test.variables,
        testName: util.sanitizeForMethodName(test.name) + "Test"
      }
    }),
    suiteName: suiteName
  });

  return rendered;

};
