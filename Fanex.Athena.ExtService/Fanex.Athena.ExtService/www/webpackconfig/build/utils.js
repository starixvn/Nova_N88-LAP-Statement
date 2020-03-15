const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

exports.setDefinePlugin = function(key, value) {
    const env = {};
    env[key] = JSON.stringify(value);

    return {
        plugins: [
            new webpack.DefinePlugin(env)
        ]
    };
};

exports.extractCSS = function(isProduction) {
    return {
        plugins: [
                new MiniCssExtractPlugin({ filename: "[name].css", chunkFilename: '[id].css'})
        ],
        module: {
            rules: [
                {
                    test: /\.(sc|c)ss$/,
                    use: [
                        MiniCssExtractPlugin.loader,
                          'css-loader?url=false',
                          'sass-loader',
                    ]
                }
            ]
        }
    };
};