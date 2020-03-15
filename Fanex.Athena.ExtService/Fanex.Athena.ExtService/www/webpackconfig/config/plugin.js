var webpack = require('webpack');
var pathConfig = require('../config/path');
var CleanWebpackPlugin = require('clean-webpack-plugin');
var WebpackBuildNotifierPlugin = require('webpack-build-notifier');
var Visualizer = require('webpack-visualizer-plugin');

var cssRootPath = pathConfig.cssPaths.build;

function generatePlugins(environment) {
    var cssBetlistPath = cssRootPath + "/" + environment + "/pages/betlist";
    var cssPagePath = cssRootPath + "/" + environment + "/pages";
    var commonPlugins = [
        new CleanWebpackPlugin([cssRootPath], {
            root: process.cwd(),
            exclude: ['fonts', 'images', 'dev', 'pro']
        }),
        new CleanWebpackPlugin([cssPagePath], {
            root: process.cwd(),
            exclude: ['betlist']
        }),
        new CleanWebpackPlugin([cssBetlistPath], {
            root: process.cwd(),
            exclude: ['images']
        }),
        new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery",
            jquery: 'jquery',
            'window.$': 'jquery',
            "window.jQuery": 'jquery',
            "root.jQuery": 'jquery'
        }),
         new WebpackBuildNotifierPlugin({
             title: "New UI Webpack Build",
             suppressSuccess: true
        }),
        new Visualizer({
            filename: './webpack-stats-newui.html'
        })
    ];

    return commonPlugins;
}

module.exports = generatePlugins;