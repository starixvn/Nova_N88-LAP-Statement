var path = require('path');
var merge = require('webpack-merge');

var pathConfig = require('../config/path');
var alias = require('../config/alias');
var rules = require('../config/rule');
var pluginGenerator = require('../config/plugin');
var exposeLoader = require('expose-loader');
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");

function generateConfig(environment) {
    var jsPath = `${pathConfig.scriptPaths.build}/${environment}`;
    var cssPath = `${pathConfig.cssPaths.build}/${environment}`;

    var commonConfigs = {
        resolve: {
            alias: alias,
            modules: [
                path.join(__dirname, "www"),
                "node_modules"
           ]
        },
        entry: {

            [`${jsPath}/language`]: pathConfig.scriptPaths.language,
            [`${jsPath}/language-zh-CN`]: pathConfig.scriptPaths.languageCN,
            [`${jsPath}/language-zh-TW`]: pathConfig.scriptPaths.languageTW,
            [`${jsPath}/language-zh-Hans`]: pathConfig.scriptPaths.languageHans,
            [`${jsPath}/pages/betlist/refno`]: [pathConfig.scriptPaths.betlist_refno],

            [`${cssPath}/main`]: pathConfig.cssPaths.main,
            [`${cssPath}/pages/utility/error`]: pathConfig.cssPaths.error,
            [`${cssPath}/um`]: pathConfig.cssPaths.um,
            [`${cssPath}/pages/betlist/betlistLayout`]: pathConfig.cssPaths.betlistLayout,
            [`${cssPath}/pages/betlist/betlistRefno`]: pathConfig.cssPaths.betlistRefno,
            [`${cssPath}/pages/betlist/systemparlaydetail`]: pathConfig.cssPaths.systemparlaydetail,
            [`${cssPath}/pages/betlist/cashouthistory`]: pathConfig.cssPaths.cashouthistory,
            [`${cssPath}/pages/betlist/betlistResult`]: pathConfig.cssPaths.betlistResult,
            [`${cssPath}/pages/betlist/colossusbetdetail`]: pathConfig.cssPaths.colossusbetdetail
        },
        output: {
            path: pathConfig.projectRoot,
            filename: "[name].chunk.js"
        },
        module: {
            rules: rules.commonRules
        },
        plugins: pluginGenerator(environment),
        stats: {
            assets: false,
            colors: true,
            version: false,
            hash: false,
            timings: true,
            chunks: false,
            chunkModules: false,
            children: false
        },
        optimization: {
            minimizer: [
             new UglifyJsPlugin({
                    sourceMap: true
                })
            ],
            splitChunks: {
                chunks: 'all',
                minSize: 30000,
                maxSize: 0,
                minChunks: 2,
                maxAsyncRequests: 5,
                maxInitialRequests: 3,
                automaticNameDelimiter: '~',
                name: true,
                cacheGroups: {
                    // disable default cache group
                    default: false,
                    vendors: false,
                    common: {
                       name: `${jsPath}/pages/lib/common`,
                       test: /(node_modules|www[\\/]js[\\/]source[\\/]vendors|www[\\/]js[\\/]source[\\/]common)/,
                       minChunks: 1,
                       priority: -10
                    }
                   
                }
            }
        }
    };

    return commonConfigs;
}

module.exports = generateConfig;