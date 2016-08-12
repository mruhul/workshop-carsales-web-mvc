var wpStrip = require('strip-loader');
var devConfig = require('./webpack.config.js');

var stripLoader = {
    test: [/\.js$/,/\.es6$/],
    exclude: /node_modules/,
    loader: wpStrip.loader('console.log')
}

devConfig.module.loaders.push(stripLoader);

module.exports = devConfig;