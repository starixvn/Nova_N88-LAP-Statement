var projectRoot = require('../config/path').projectRoot;

var alias = {
    // vendors
    'vendor-materialize': projectRoot + "/www/js/source/vendors/materialize/materialize.js",
    // common
    common: projectRoot + "/www/js/source/common",
    lib: projectRoot + "/www/js/source/lib",
    extensions: projectRoot + "/www/js/source/extensions",
    components: projectRoot + "/www/js/source/components",
    helpers: projectRoot + "/www/js/source/helpers",
    pages: projectRoot + "/www/js/source/pages"
   
};

module.exports = alias;