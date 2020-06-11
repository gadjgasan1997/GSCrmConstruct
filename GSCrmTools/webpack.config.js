const path = require('path')

module.exports = {
    entry: "./wwwroot/js/CommonEventHandlers.js",
    output: {
        filename: "Core-GSCrm.js",
        path: path.resolve(__dirname, './wwwroot/js/')
    },
    module: {
      rules: [
        {
          test: /\.css$/,
          use: [
            'style-loader',
            'css-loader'
          ]
        }
      ]
  }
}