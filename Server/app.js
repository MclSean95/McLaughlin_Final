var io = require("socket.io")(process.envPort||3000);
var MongoClient = require("mongodb").MongoClient;
var url = "mongodb://localhost:27017/";

var dbObj;

MongoClient.connect(url ,function(err, client){
	if(err) throw err;
	console.log("connected");
	dbObj = client.db("questions");
});

io.on("connection", function(socket){
	console.log("connected");
	socket.on("sendData", function(data){
		console.log(data);
		console.log("ready to send data");
		dbObj.collection("questionData").save(data, function(err, response){
			if(err) throw err;
			console.log("Data Saved");
			
			socket.emit("dataRecieved");
		});
	});
	
});