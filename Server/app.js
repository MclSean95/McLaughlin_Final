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
    
	socket.on("sendData", function(data){

        var isEmpty = dbObj.collection("questionData").findOne().then(function(isEmpty){
            console.log(isEmpty);
        if(isEmpty === null)
        {
            dbObj.collection("questionData").save(data, function(err, response){
			 if(err) throw err;
			 socket.emit("dataRecieved");
		  });
        }
        else
        {
            dbObj.collection("questionData").replaceOne({name : "Round Zero"}, data).then(function(data){
                socket.emit("dataRecieved");
            });
                
                
            
        }
	});
    
    socket.on("getData", function(data){
       dbObj.collection("questionData").find().toArray(function(err, results){
           socket.emit("gotData", results[0]);
       });
    });
	
});
});