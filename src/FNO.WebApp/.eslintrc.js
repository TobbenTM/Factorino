module.exports = {
  root: true,
  parserOptions: {
    parser: "babel-eslint",
    sourceType: "module"
  },
  settings: {
    'import/resolver': 'webpack',
  },
  // https://github.com/airbnb/javascript
  extends: ["airbnb-base", "plugin:vue/essential"],
  plugins: [
    'vue',
    'html',
  ],
  rules: {
    // allow paren-less arrow functions
    "arrow-parens": 0,
    // allow async-await
    "generator-star-spacing": 0,
    // allow debugger during development
    "no-debugger": process.env.NODE_ENV === "production" ? 2 : 0,
    // allow CRLF
    "linebreak-style": 0
  }
};
