const PROXY_CONFIG = [
  {
    context: [
      "/search"
    ],
    target: "https://localhost:7260",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
