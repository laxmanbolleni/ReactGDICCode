const http = require('http');
const fs = require('fs');
const path = require('path');
const url = require('url');

const PORT = 3000;

// MIME types
const mimeTypes = {
  '.html': 'text/html',
  '.js': 'text/javascript',
  '.css': 'text/css',
  '.json': 'application/json',
  '.png': 'image/png',
  '.jpg': 'image/jpg',
  '.gif': 'image/gif',
  '.svg': 'image/svg+xml',
  '.wav': 'audio/wav',
  '.mp4': 'video/mp4',
  '.woff': 'application/font-woff',
  '.ttf': 'application/font-ttf',
  '.eot': 'application/vnd.ms-fontobject',
  '.otf': 'application/font-otf',
  '.wasm': 'application/wasm'
};

// Proxy function for API calls
function proxyRequest(req, res, targetPort, targetPath) {
  const options = {
    hostname: 'localhost',
    port: targetPort,
    path: targetPath,
    method: req.method,
    headers: {
      ...req.headers,
      'host': `localhost:${targetPort}`
    }
  };

  const proxyReq = http.request(options, (proxyRes) => {
    // Add CORS headers
    res.setHeader('Access-Control-Allow-Origin', '*');
    res.setHeader('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE, OPTIONS');
    res.setHeader('Access-Control-Allow-Headers', 'Content-Type, Authorization');
    
    res.writeHead(proxyRes.statusCode, proxyRes.headers);
    proxyRes.pipe(res, { end: true });
  });

  proxyReq.on('error', (err) => {
    console.error('Proxy error:', err);
    res.writeHead(500, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({ error: 'API service unavailable' }));
  });

  req.pipe(proxyReq, { end: true });
}

const server = http.createServer((req, res) => {
  const parsedUrl = url.parse(req.url, true);
  const pathname = parsedUrl.pathname;

  // Handle CORS preflight requests
  if (req.method === 'OPTIONS') {
    res.setHeader('Access-Control-Allow-Origin', '*');
    res.setHeader('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE, OPTIONS');
    res.setHeader('Access-Control-Allow-Headers', 'Content-Type, Authorization');
    res.writeHead(200);
    res.end();
    return;
  }

  // Proxy API requests
  if (pathname.startsWith('/api/news')) {
    proxyRequest(req, res, 5137, pathname + (parsedUrl.search || ''));
    return;
  }

  if (pathname.startsWith('/api/deals')) {
    proxyRequest(req, res, 5215, pathname + (parsedUrl.search || ''));
    return;
  }

  // Serve static files
  let filePath = path.join(__dirname, 'intelligence-center-frontend/public', pathname === '/' ? 'index.html' : pathname);

  // Check if file exists
  fs.access(filePath, fs.constants.F_OK, (err) => {
    if (err) {
      // If file doesn't exist, serve index.html for SPA routing
      filePath = path.join(__dirname, 'intelligence-center-frontend/public/index.html');
    }

    fs.readFile(filePath, (err, content) => {
      if (err) {
        if (err.code === 'ENOENT') {
          // Create a basic index.html if it doesn't exist
          const basicHtml = `
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>GlobalData Intelligence Center</title>
    <style>
        body { font-family: Arial, sans-serif; text-align: center; padding: 50px; }
        .container { max-width: 600px; margin: 0 auto; }
        .btn { background: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; margin: 10px; display: inline-block; }
        .error { color: #dc3545; background: #f8d7da; padding: 20px; border-radius: 5px; margin: 20px 0; }
    </style>
</head>
<body>
    <div class="container">
        <h1>🌐 GlobalData Intelligence Center</h1>
        <div class="error">
            <h3>React Development Server</h3>
            <p>The React application is being served through a development proxy.</p>
            <p>APIs are available at:</p>
            <ul style="text-align: left; display: inline-block;">
                <li>News API: <a href="/api/news/health">/api/news/health</a></li>
                <li>Deals API: <a href="/api/deals/health">/api/deals/health</a></li>
            </ul>
        </div>
        <a href="../frontend-app.html" class="btn">Open Full Frontend App</a>
        <a href="../simple-dashboard.html" class="btn">Open Simple Dashboard</a>
    </div>
    <script>
        // Test API connectivity
        fetch('/api/news/health')
            .then(response => response.json())
            .then(data => console.log('News API:', data))
            .catch(err => console.error('News API Error:', err));
            
        fetch('/api/deals/health')
            .then(response => response.json())
            .then(data => console.log('Deals API:', data))
            .catch(err => console.error('Deals API Error:', err));
    </script>
</body>
</html>`;
          res.writeHead(200, { 'Content-Type': 'text/html' });
          res.end(basicHtml);
        } else {
          res.writeHead(500);
          res.end('Server Error');
        }
        return;
      }

      const ext = path.parse(filePath).ext;
      const contentType = mimeTypes[ext] || 'text/plain';

      res.writeHead(200, { 'Content-Type': contentType });
      res.end(content);
    });
  });
});

server.listen(PORT, () => {
  console.log(`🚀 React Development Server running on http://localhost:${PORT}`);
  console.log('📊 API Proxy enabled for:');
  console.log('   - News API: http://localhost:5137 -> /api/news/*');
  console.log('   - Deals API: http://localhost:5215 -> /api/deals/*');
  console.log('');
  console.log('🌐 Available endpoints:');
  console.log('   - Frontend: http://localhost:3000');
  console.log('   - News Health: http://localhost:3000/api/news/health');
  console.log('   - Deals Health: http://localhost:3000/api/deals/health');
});
