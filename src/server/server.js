var net = require('net');
var http = require('http');
var fs = require('fs');
var querystring = require('querystring');
var sys = require('sys');

Array.prototype.removeElement = function(element) {
	for(var i = 0; i < this.length; i++) {
		if (this[i] === element) {
			this.splice(i,1);
			return true;
		}
		return false;
	}
}

http.createServer(function (request, response) { 
	var body = '';
	
	request.setEncoding('utf8');
	response.writeHead(200, {'Content-Type': 'text/plain'});
	  	
	request.on('data', function(chunk) {
		body += chunk.toString();
	})
	.on('end', function() {  
		var requestBody = querystring.parse(body);
		var result = '';
		
		response.end();	
		console.log('request');
		
		if (typeof requestBody.payload === 'undefined')
			return;
			
		try {
			result = JSON.parse(requestBody.payload);
		} catch(e) {
			console.log('failed to parse the json');
			return;
		}
		
		sendMessage(JSON.stringify(result));
	});
}).listen(3000);

var tcpClients = [];
var server = net.createServer(function(socket) {
	tcpClients.push(socket);	
	console.log('new client connected');

	socket.on('close', function(had_error) {
		tcpClients.removeElement(socket);
		console.log('socket closed ' + (had_error ? 'with error' : ''));
	});	
}).listen(9898);

function sendMessage(message) {
	for (var i = 0; i < tcpClients.length; i++) {
		var socket = tcpClients[i];
		sendframe(socket, message);
	}
}

function sendframe(stream, message) {
	var length = Buffer.byteLength(message, 'utf8')
	var buffer = new Buffer(length + 4);
		
	buffer.writeInt32(length, 0, 'little');
	buffer.write(message, 4);
	stream.write(buffer);
		
	console.log('length of message: ' + length);	
}

console.log('Server running at http://127.0.0.1:3000/');
