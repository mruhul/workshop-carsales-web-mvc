'use strict';

var glob = require('glob'),
    path = require('path'),
    webpack = require('webpack'),
    CleanWebpackPlugin = require('clean-webpack-plugin'),
    ExtractTextPlugin  = require('extract-text-webpack-plugin'),
    AssetsPlugin = require('assets-webpack-plugin'),
    CopyWebpackPlugin = require('copy-webpack-plugin'),
    AssetsPlugin = require('assets-webpack-plugin'),
    assetsPluginInstance = new AssetsPlugin();

var isProd = (process.env.NODE_ENV === 'prod');

var config = {
    entryPointMatch: './features/**/*-page.{js,ts}', // anything ends with -page.js
    outputPath: path.join(__dirname, 'dist'),
    publicPath: isProd ? './' :  '/dist/'
}

function getEntryFiles(){
    let entries = {};

    let matchedFiles = glob.sync(config.entryPointMatch);

    let length = matchedFiles.length;

    for(let i = 0; i < length; i++){
        let filePath = matchedFiles[i];
        let ext = path.extname(filePath);
        let filename = path.basename(filePath, ext);
        entries[filename] = filePath;
    }

    entries['common'] = ['jquery','./Features/Shared/Assets/common.js'];

    return entries;
}


module.exports = {
    entry: getEntryFiles(),
    output: {
        path: config.outputPath,
        filename: isProd ? '[name]-[chunkhash].js' : '[name].js',
        publicPath: config.publicPath
    },
    //externals: {
    //    "jquery": "jQuery"
    //},
    module: {
        preLoaders:[
            {
                test: [/\.js$/,/\.es6$/],
                exclude: /node_modules/,
                loader: 'eslint-loader'
            }
        ],
        loaders: [
            {
                test: /\.ts$/,
                loaders: ['ts-loader']
            },
            { 
                test: [/\.js$/,/\.es6$/],
                exclude: /node_modules/,
                loader: 'babel-loader'
            },
            { 
                test: [/\.css$/],
                exclude: /node_modules/,
                loader: ExtractTextPlugin.extract('style-loader','css-loader!autoprefixer-loader')
            },
            { 
                test: /\.scss$/,
                exclude: /node_modules/,
                loader: ExtractTextPlugin.extract('style-loader','css-loader!autoprefixer-loader!sass-loader')
            },
            {
                test: /.*\.(gif|png|jpe?g|svg)$/i,
                exclude: /node_modules/,
                loaders: [
                    'file?hash=sha512&digest=hex&name=img-[hash].[ext]',
                    'image-webpack'
                ]
            }
        ]
    },
    imageWebpackLoader: {
        pngquant:{
            quality: "65-90",
            speed: 4
        },
        svgo:{
            plugins: [
                { removeViewBox: false },
                { removeEmptyAttrs: false }
            ]
        }
    },
    resolve: {
        extensions: ['','.js','.ts','.es6','.scss']
    },
    plugins: [
        // new CopyWebpackPlugin(
        //     [
        //         {from:'./features/**/*.+(png|jpeg|jpg|svg)', 
        //             to:'./', 
        //             flatten: true}
        //     ]
        // ),
        new AssetsPlugin({
            filename: 'webpack.assets.json',
            path: __dirname,
            prettyPrint: true
        }),
        new CleanWebpackPlugin(['dist', 'build'], {
            root: __dirname,
            verbose: true, 
            dry: false
        }),
        new ExtractTextPlugin(isProd ? '[name]-[chunkhash].css' : '[name].css'),
        new webpack.optimize.CommonsChunkPlugin({
            name: "common",
            filename: isProd ? "common-[chunkhash].js" : "common.js",
            minChunks: Infinity
        })
    ],
    devtool:"source-map"
};