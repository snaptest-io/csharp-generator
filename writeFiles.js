/* *************************************
  Generate Files
    - Takes the file structure and files and makes real fiels/folders on the FS
**************************************** */
var Path = require('path');
var mkdirp = require("mkdirp");
var fs = require('fs-extra');

module.exports = function(fileStructure, meta) {

  // first mkdirp all directories:
  fileStructure.filter((entity) => !entity.file).forEach((entity) => {
    mkdirp.sync(Path.normalize(meta.topDirPath + "/" + entity.path.join("/")));
  });

  // add the files into the proper folders
  fileStructure.filter((entity) => entity.file).forEach((entity) => {
    fs.writeFileSync(Path.normalize(meta.topDirPath + "/" + entity.path.join("/")), entity.file);
  });

};