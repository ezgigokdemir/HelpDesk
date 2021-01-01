var path = require('path');
var webpack = require('webpack');
var fs = require('fs');
const VueLoaderPlugin = require('vue-loader/lib/plugin');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');

var entry = {
    
};

var glob = require('glob');

function toObject(paths) {
    var ret = {};

    paths.forEach(function (path) {
        let name = path.split('/').slice(-3).join('/');
        name = name.substring(0, name.length - 3);
        ret[name] = path;
    });
    return ret;
}

module.exports = {
    entry: toObject(glob.sync('./wwwroot/js/src/**/**/*.js*', { dot: true, ignore: './wwwroot/js/src/static/**/*.js*' })),
    optimization: {
        splitChunks: {
            cacheGroups: {
                commons: {
                    test: /[\\/]node_modules[\\/]/,
                    name: 'vendors',
                    chunks: 'all'
                }
            }
        }
    },
    output: {
        path: path.resolve(__dirname, './wwwroot/js/dist'),
        filename: '[name].js'
    },
    watch: true,
    mode: 'development',
    watchOptions: {
        aggregateTimeout: 300,
        ignored: /node_modules/
    },
    devtool: 'cheap-eval-source-map',
    
    resolve: {
        alias: {
            'vue$': 'vue/dist/vue.esm.js'
        }
    },

    module: {
        rules: [
            {
                test: /\.vue$/,
                loader: 'vue-loader'
            },
            {
                test: /\.js$/,
                exclude: /(node_modules|bower_components)/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['@babel/preset-env']
                    }
                }
            },
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    'css-loader'
                ]
            },
            {
                test: /\.(png|svg|jpg|gif)$/,
                use: [
                    'file-loader'
                ]
            },
            {
                test: /\.(woff|woff2|eot|ttf|otf)$/,
                use: [
                    'file-loader'
                ]
            },
            {
                test: /\.(csv|tsv)$/,
                use: [
                    'csv-loader'
                ]
            },
            {
                test: /\.xml$/,
                use: [
                    'xml-loader'
                ]
            }
        ]
    },
    plugins: [
        new VueLoaderPlugin(),
        new CopyWebpackPlugin([
            { from: './wwwroot/js/src/static', to: 'static' },
        ]),
    ]
};
