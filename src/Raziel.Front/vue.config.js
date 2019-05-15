const CopyPlugin = require('copy-webpack-plugin');

module.exports = {
  configureWebpack: {
    plugins: [
      new CopyPlugin([{
        from: './src/assets/js/argon2-asm.min.js',
        to: './js/argon2-asm.min.js'
      }, ])
    ]
  },
  lintOnSave: false
}
