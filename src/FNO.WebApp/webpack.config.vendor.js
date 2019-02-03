const path = require('path');
const webpack = require('webpack');
const MiniCSSExtractPlugin = require('mini-css-extract-plugin');
const OptimizeCSSPlugin = require('optimize-css-assets-webpack-plugin');

module.exports = (env, argv) => {
  const isDevBuild = !(
    process.env.NODE_ENV && process.env.NODE_ENV === 'production'
  );

  let mode;

  if (argv && argv.mode) {
    mode = argv.mode;
  } else {
    mode = isDevBuild ? 'development' : 'production';
  }

  return [
    {
      // Redundant, but from Dotnet the mode cannot be set at command line.
      mode,
      stats: { modules: false },
      resolve: {
        extensions: ['.js'],
      },
      module: {
        rules: [
          {
            test: /\.(png|woff|woff2|eot|ttf|svg)(\?|$)/,
            use: 'url-loader?limit=100000',
          },
          {
            test: /\.css(\?|$)/,
            use: [MiniCSSExtractPlugin.loader, 'css-loader'],
          },
        ],
      },
      entry: {
        vendor: [
          'event-source-polyfill',
          'vue',
          'vuex',
          'axios',
          'vue-router',
          'element-ui/lib/theme-chalk/index.css',
        ],
      },
      optimization: {
        minimize: !isDevBuild,
      },
      output: {
        path: path.join(__dirname, 'wwwroot', 'dist'),
        publicPath: '/dist/',
        filename: '[name].js',
        library: '[name]_[hash]',
      },
      plugins: [
        new MiniCSSExtractPlugin({
          filename: 'vendor.css',
        }),
        // Compress extracted CSS.
        new OptimizeCSSPlugin({
          cssProcessorOptions: {
            safe: true,
          },
        }),
        new webpack.DllPlugin({
          path: path.join(__dirname, 'wwwroot', 'dist', '[name]-manifest.json'),
          name: '[name]_[hash]',
        }),
      ],
    },
  ];
};
