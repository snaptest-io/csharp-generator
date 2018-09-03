/* *************************************
  Generate Components
    - Generates Component files and contents
**************************************** */

var fs = require('fs');
var Path = require('path');

module.exports = function(meta) {

  fs.createReadStream(`${__dirname}/static/${meta.style}/SnapTestBase.cs`)
    .pipe(fs.createWriteStream(Path.normalize(meta.topDirPath + "/common/SnapTestBase.cs")));

  fs.createReadStream(`${__dirname}/static/${meta.style}/SnapActionDriver.cs`)
    .pipe(fs.createWriteStream(Path.normalize(meta.topDirPath + "/common/SnapActionDriver.cs")));

};
