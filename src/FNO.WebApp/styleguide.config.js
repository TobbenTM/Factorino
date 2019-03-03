const path = require('path');
const VueLoaderPlugin = require('vue-loader/lib/plugin');

const webpackConfig = {
  resolve: {
    extensions: ['.js', '.vue'],
    alias: {
      '@': path.resolve(__dirname, './ClientApp'),
    },
  },
  module: {
    rules: [
      { test: /\.vue$/, include: /ClientApp/, use: 'vue-loader' },
      { test: /\.js$/, include: /ClientApp/, use: 'babel-loader' },
      {
        test: /\.css$/,
        use: ['style-loader', 'css-loader'],
      },
      {
        test: /\.scss$/,
        use: ['vue-style-loader', 'css-loader', 'sass-loader'],
      },
      { test: /\.(png|jpg|jpeg|gif|eot|ttf|woff|woff2|svg|svgz)(\?.+)?$/, use: 'url-loader?limit=25000' },
    ],
  },
  plugins: [
    // add vue-loader plugin
    new VueLoaderPlugin(),
  ],
  cache: false,
};

module.exports = {
  components: 'ClientApp/components/*.vue',
  webpackConfig,
  require: [
    path.join(__dirname, 'ClientApp/css/site.scss'),
    path.join(__dirname, 'styleguide.requires.js'),
  ],
};
