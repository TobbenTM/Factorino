const path = require('path');
const webpack = require('webpack');
const VueLoaderPlugin = require('vue-loader/lib/plugin');
const MiniCSSExtractPlugin = require('mini-css-extract-plugin');
const OptimizeCSSPlugin = require('optimize-css-assets-webpack-plugin');

const bundleOutputDir = './wwwroot/dist';

module.exports = (env, argv) => {
  const isDevBuild = !(
    process.env.NODE_ENV && process.env.NODE_ENV === 'production'
  );

  let mode;

  if (argv && argv.mode) {
    [mode] = argv;
  } else {
    mode = isDevBuild ? 'development' : 'production';
  }

  return [
    {
      mode,
      entry: { main: './ClientApp/boot-app.js' },
      resolve: {
        extensions: ['.js', '.vue'],
        alias: {
          components: path.resolve(__dirname, './ClientApp/components'),
          vm: path.resolve(__dirname, './ClientApp/vm'),
          assets: path.resolve(__dirname, './ClientApp/assets'),
          utils: path.resolve(__dirname, './ClientApp/utils'),
          api: path.resolve(__dirname, './ClientApp/store/api'),
          app: path.resolve(__dirname, './ClientApp'),
        },
      },
      output: {
        path: path.join(__dirname, bundleOutputDir),
        filename: '[name].js',
        publicPath: '/dist/',
      },
      module: {
        rules: [
          { test: /\.vue$/, include: /ClientApp/, use: 'vue-loader' },
          { test: /\.js$/, include: /ClientApp/, use: 'babel-loader' },
          {
            test: /\.css$/,
            use: isDevBuild
              ? ['style-loader', 'css-loader']
              : [MiniCSSExtractPlugin.loader, 'css-loader'],
          },
          {
            test: /\.scss$/,
            use: [
              'vue-style-loader',
              'css-loader',
              'sass-loader',
            ],
          },
          { test: /\.(png|jpg|jpeg|gif|eot|ttf|woff|woff2|svg|svgz)(\?.+)?$/, use: 'url-loader?limit=25000' },
        ],
      },
      optimization: {
        minimize: !isDevBuild,
      },
      plugins: [
        new webpack.DllReferencePlugin({
          context: __dirname,
          manifest: require('./wwwroot/dist/vendor-manifest.json'),
        }),
        new VueLoaderPlugin(),
      ].concat(isDevBuild ? [] : [new MiniCSSExtractPlugin({
        filename: 'site.css',
      }),
      new OptimizeCSSPlugin({
        cssProcessorOptions: {
          safe: true,
        },
      })]),
      devtool: isDevBuild ? 'source-map' : false,
    },
  ];
};
