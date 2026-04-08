#!/bin/bash

#### Server ###
echo "export server..."
./protoc.exe --proto_path=proto --java_out=./../server/src/main/kotlin/ proto/*.proto

#### Client ###
echo "export client..."
./protoc.exe --proto_path=proto --csharp_out=./../client/Assets/Scripts/Msg proto/*.proto