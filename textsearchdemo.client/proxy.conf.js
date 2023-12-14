const PROXY_CONFIG = [
  {
    context: [
      "/search"
    ],
    target: "http://localhost:5000",
    secure: false,
    changeOrigin: true
  }
]

module.exports = PROXY_CONFIG;
