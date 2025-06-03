const express = require('express');
const path = require('path');
const fs = require('fs');

const app = express();
const PORT = 3000;

// Serve static files from the React app build directory
app.use(express.static(path.join(__dirname, 'intelligence-center-frontend/build')));

// Serve static files from the public directory
app.use(express.static(path.join(__dirname, 'intelligence-center-frontend/public')));

// API proxy to avoid CORS issues
app.use('/api/news', (req, res) => {
  const url = `http://localhost:5137/api/news${req.url}`;
  console.log('Proxying to:', url);
  
  const http = require('http');
  const options = {
    hostname: 'localhost',
    port: 5137,
    path: `/api/news${req.url}`,
    method: req.method,
    headers: req.headers
  };

  const proxyReq = http.request(options, (proxyRes) => {
    res.writeHead(proxyRes.statusCode, proxyRes.headers);
    proxyRes.pipe(res, { end: true });
  });

  req.pipe(proxyReq, { end: true });
});

app.use('/api/deals', (req, res) => {
  const url = `http://localhost:5215/api/deals${req.url}`;
  console.log('Proxying to:', url);
  
  const http = require('http');
  const options = {
    hostname: 'localhost',
    port: 5215,
    path: `/api/deals${req.url}`,
    method: req.method,
    headers: req.headers
  };

  const proxyReq = http.request(options, (proxyRes) => {
    res.writeHead(proxyRes.statusCode, proxyRes.headers);
    proxyRes.pipe(res, { end: true });
  });

  req.pipe(proxyReq, { end: true });
});

// Catch all handler: send back React's index.html file
app.get('*', (req, res) => {
  const indexPath = path.join(__dirname, 'intelligence-center-frontend/build/index.html');
  
  if (fs.existsSync(indexPath)) {
    res.sendFile(indexPath);
  } else {
    // If build doesn't exist, serve a simple HTML page
    res.send(`
      <!DOCTYPE html>
      <html>
      <head>
        <title>GlobalData Intelligence Center</title>
        <style>
          body { font-family: Arial, sans-serif; text-align: center; padding: 50px; }
          .container { max-width: 600px; margin: 0 auto; }
          .error { color: #dc3545; background: #f8d7da; padding: 20px; border-radius: 5px; margin: 20px 0; }
          .btn { background: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; }
        </style>
      </head>
      <body>
        <div class="container">
          <h1>🌐 GlobalData Intelligence Center</h1>
          <div class="error">
            <h3>React App Build Not Found</h3>
            <p>The React application needs to be built first.</p>
            <p>Please run: <code>npm run build</code> in the intelligence-center-frontend directory</p>
          </div>
          <a href="/simple-dashboard.html" class="btn">Use Simple Dashboard Instead</a>
        </div>
      </body>
      </html>
    `);
  }
});

app.listen(PORT, () => {
  console.log(`🚀 Development server running on http://localhost:${PORT}`);
  console.log('📊 Proxying API requests to:');
  console.log('   - News API: http://localhost:5137');
  console.log('   - Deals API: http://localhost:5215');
});
