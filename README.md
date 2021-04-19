# dockerApi
This is a boiler plate sample of docker for ASP.Net Core API

## To build docker image run the following command:
docker build -t {imagename} -f {folder}/Dockerfile .

### Note: 
1. Must run this command from docker context
2. imagename: Provide image name all small letter
3. folder: Provide folder information under which Dockerfile exists.

## To run docker image execute the following command:
docker run -it -p 8000:80 {imagename}

### Note:
1. This will run the image on four ground. Press CTRL + C to running.
2. To run the image on back ground execute the following: docker run -d -p 8000:80 <imagename>
3. Here, 8000 is available port number. If the port number is locked then open it by firewall.
4. To browse go to browser and visit the following URL: http://localhost:8000/

## Connect Local SQL Server instance with Docker
1. Open SQL Server Configuration Manager in administrator mode. Go to "C:\Windows\SysWOW64 location" and run "SQLServerManager15.msc" as administrator mode.
2. Open "Protocols for SQLEXPRESS" under "SQL Server Network Configuration"
3. Check TCP/IP is enable.
4. Right click on TCP/IP and under Protocol tab make Enable Yes and Listen All Yes.
5. Then under IP Address under IPALL provide TCP Port (ex. 49172)
6. Then Apply and OK.
7. Go to SQL Server Services and restart SQL Server (SQLEXPRESS). Make SQL Server Browser is running. If not then start it.
8. Setup inbound rule on FireWall for TCP port 49172 which we setup in SQL Server Configuration Manager.
9. To check the port is available or not, run the command "netstat /a /n /o >d:\netstat.txt" in cmd and check the port number in the text file. If not then configure.
10. Get IP address of your PC by running the command "ipconfig" and note down suitable IPv4 Address.
11. Proper connection string patter: "Server=172.27.48.1,49172\\SQLEXPRESS;Initial Catalog=DockerApi;User ID=wadud;Password=abcd1234;". Provide wadud user DB access of DockerApi from
    sa user under security.
	
## Create docker volume and mapped it with image
1. Create a Create-volume.bat file and in it put the following command 
   "docker volume create --driver local --opt type=none --opt device=<Physical location of the file> --opt o=bind <volume name>"
2. run the following command from context to bind the created volume and image
   "docker run -it -p 8000:80 -v {volume name}:/app/Logs {image name}"
	
### Other useful commands
1. docker ps -> show the list of containers
2. docker images -> show the list of docker images
3. docker volume ls -> show the list of volumes
4. docker volume rm {volume name} -> To delete a specific volume
	
	
