const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7233';

const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      "/api/Person",
      "/api/auth"
    ],
    target,
    secure: false
  }
]
console.log('Proxy configuration loaded:', PROXY_CONFIG);

module.exports = PROXY_CONFIG;
