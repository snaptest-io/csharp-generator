/* *************************************
  Generate Virtual File Structure and Contents
**************************************** */
var treeUtils = require('../utils/tree');
var FSUtils = require('../utils/virtualFS');
var _ = require("lodash");
var sanitizeForFilename = require("sanitize-filename");
var generateSuite = require('./suiteFile');

module.exports = function(fileStructure, meta) {

  var testsFolder = FSUtils.fsFindByPath(fileStructure, ["suites"]);

  // make folders and the suites...
  treeUtils.walkThroughTreeNodes(meta.directory, (node, parentNode) => {

    var entityType = isFolder(node) ? "folder" : node.type;

    // If the root node, ignore it.
    if (node.root || (entityType !== "test" && entityType !== "folder")) return;

    var fsEntity;

    // ***** Generate Suite files: *****

    if (entityType === "folder") {

      // Folders without inner folders are suites
      var hasInnerFolder = false;
      var innerTests = [];

      treeUtils.walkThroughTreeNodes(node, (innerNode) => {
        if (node !== innerNode && isFolder(innerNode)) {
          hasInnerFolder = true;
        } else if (innerNode.type === "test") {
          innerTests.push(_.find(meta.tests, {id: innerNode.testId}));
        }
      });

      if (!hasInnerFolder) {
        fsEntity = {
          path: testsFolder.path.concat(generateNodePath(node, generateSuiteName(node, meta.tests) + ".cs")),
          file: generateSuite(generateSuiteName(node, meta.tests), innerTests, meta)
        }
      } else {
        fsEntity = {
          path: testsFolder.path.concat(generateNodePath(node, generateFolderName(node, meta.tests)))
        }
      }

    } else if (entityType === "test") {
      // Check for single test suites:

      var hasAdjacentFolder = false;

      if (isFolder(parentNode)) {
        parentNode.children.forEach((sibling) => {
          if (isFolder(sibling)) {
            hasAdjacentFolder = true;
          }
        })
      }

      if (hasAdjacentFolder) {
        fsEntity = {
          path: testsFolder.path.concat(generateNodePath(node, generateSuiteName(node, meta.tests) + ".cs")),
          file: generateSuite(generateSuiteName(node, meta.tests), [_.find(meta.tests, {id: node.testId})], meta)
        }
      } else {
        return; // Already processed when generating suite contents.
      }

    }

    fileStructure.push(fsEntity);

  });

};

isFolder = (node) => _.isArray(node.children);

generateSuiteName = (node, tests) => {
  var nodeName = isFolder(node) ? node.module : _.find(tests, {id: node.testId}).name;
  return sanitizeForFilename(nodeName).replace(/\W|_/g, "") + "Suite";
};

generateFolderName = (node, tests) => {
  var nodeName = isFolder(node) ? node.module : _.find(tests, {id: node.testId}).name;
  return sanitizeForFilename(nodeName).replace(/\W|_/g, "");
};

generateNodePath = (node, nodeName) => {

  var path = [nodeName];

  treeUtils.walkUpParents(node, (parent) => {
    if (!parent.root) path.unshift(generateFolderName(parent, parent.module));
  });

  return path;

};

getTestFromNode = (node, tests) => {
  return _.find(tests, {id: node.testId});
};