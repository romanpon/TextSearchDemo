const PROXY_CONFIG = [
  {
    context: [
      "/search"
    ],
    target: "https://localhost:5000",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
