var writeFiles = require('./writeFiles');
var generateComponents = require('./generate/componentFile');
var generateSuites = require('./generate/fileStructure');
var copyStatic = require('./copyStatic');

function generate() {

  // Basic folder layout:
  var fileStructure = [
    { path: ["common"] },
    { path: ["components"] },
    { path: ["suites"] }
  ];

  // Build virtual file structure
  generateSuites(fileStructure, this);
  generateComponents(fileStructure, this);

  // Write to disc
  writeFiles(fileStructure, this);
  copyStatic(this);

  this.onComplete();

};

module.exports = {
  generate: generate,
  styles: ["nunit", "xunit"]
};
