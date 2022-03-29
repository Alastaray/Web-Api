# Web api
#### It is a web api with some endpoint. 

##### You can send request such to upload the file by url, if the file is an image then it will be cut, access file and soft removing it by ID.
##### All files will save in universal storage. You just need to write a connection string to storage to be needed.
##### All requests ease are sent by Postman.

#### What request you can send, but firstly you need authorization:
- 	##### api/auth/refresh-token
-	##### api/registration request body: 
	-	{"name":...,
    -   "surname":..,
	-	"login": .., 
	-	"password": ...}
-	##### api/authorization request body: 
	-	{"login": ..., 
	-	"password": ...}
-	##### api/upload-by-url request body: {"url": FILE_URL}
-	##### api/get-url/:id
-	##### api/remove/:id
-	##### api/restore/:id