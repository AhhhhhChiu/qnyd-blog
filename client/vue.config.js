// eslint-disable-next-line @typescript-eslint/no-var-requires
const path = require('path');

function resolve(dir) {
  return path.join(__dirname, dir);
}
module.exports = {
  devServer: {
    proxy: {
      '/api/v1': {
        changeOrigin: true,
        target: 'http://192.168.1.5:5000/',
      },
    },
  },
  chainWebpack: (config) => config.resolve.alias.set('@', resolve('src')),
  css: {
    loaderOptions: {
      sass: {
        additionalData: '@import "@/assets/css/global.scss";',
      },
    },
  },
};
