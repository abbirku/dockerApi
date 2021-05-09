# dockerApi
This is a ASP.Net Core WebApi boiler plate

## Environment Variables
Set the following keys in windows 10 environment variables with sutable values

1. ConnectionStrings:DockerApiDb
2. Serilog:WriteTo:1:Args:connectionString
3. Serilog:WriteTo:1:Args:tableName
4. EmailSettings:SMTPEmail
5. EmailSettings:SMTPPassword
6. EmailSettings:SMTPPort
7. EmailSettings:SMTPHostname

**Notes:**
1. After settings the environment variables restart the project to get the latest values.
2. We have used EmailSettings class to bind with the EmailSettings section in appsettings.json.
3. Here, tableName of Serilog should be the table that we have created in database.

## Migration & Data Seeding
1. For each context under a module create a seed class and inherit from DataSeed.
2. Bind it to module as self.
3. In StartUp.cs in Configure method inject that seed class.
4. At the end of Configure method do MigrateAsync.

## Stored Procedure
In this boilerplate, we have used *_Stored Procedure_*. *_id_* of *_User_* Entity is used in *_WebCamImage_* Entity as foreign key. Now to gather *_User_* information along with *_WebCamImage_* data, a *_Stored Procedure_* named **_GetUserWebCamImages_** is created under **_Docker.DbBackup_** project. After running this *_Stored Procedure_* in *_Sql Server_*, you can use **_QueryWithStoredProcedureAsync_** to execute the *_SP_* which will return the queried data. To observe the process please follow **_GetUserWebCamImages_** Endpoint and its dependency.

**Notes:** Some repository method to execute the *_Stored Procedure_*.
1. ExecuteScalarAsync
2. ExecuteStoredProcedure
3. ExecuteStoredProcedureAsync

## AutoMapper
To use **AutoMapper** use the following nuget packages in infrastructure and WebApi
project.
```
//AutoMapper
AutoMapper.Extensions.Microsoft.DependencyInjection
```
Register models mapping in profile like following in **Infrastructure** project
```cs
#region Web Cam Image
CreateMap<WebCamImageInsertDTO, WebCamImage>();
CreateMap<WebCamImageUpdateDTO, WebCamImage>();
CreateMap<WebCamImage, WebCamImageQueryDTO>();
#endregion
```
Register **AutoMapper** for a profile at **ConfigureServices** in **Startup.cs**
```cs
//Automapper
services.AddAutoMapper(typeof(MappingProfile));
```
Inject **_IMapper_** in **_Service_** or **_Repository_** and Map data between two models like below
```cs
var result = _mapper.Map<IList<WebCamImage>, IList<WebCamImageQueryDTO>>(data);
```

## To build docker image run the following command:
docker build -t {imagename} -f {folder}/Dockerfile .

**Notes:**
1. Must run this command from docker context
2. imagename: Provide image name all small letter
3. folder: Provide folder information under which Dockerfile exists.

## To run docker image execute the following command:
docker run -it -p 8000:80 {imagename}

**Notes:**
1. This will run the image on four ground. Press CTRL + C to running.
2. To run the image on back ground execute the following: docker run -d -p 8000:80 <imagename>
3. Here, 8000 is available port number. If the port number is locked then open it by firewall.
4. To browse go to browser and visit the following URL: http://localhost:8000/

## Connect Local SQL Server instance with Docker
1. Open **SQL Server Configuration Manager** in administrator mode. *Go to "C:\Windows\SysWOW64 location"* and run **"SQLServerManager15.msc"** as administrator mode.
2. Open **"Protocols for SQLEXPRESS"** under **"SQL Server Network Configuration"**
3. Check **TCP/IP** is enable.
4. Right click on **TCP/IP** and under Protocol tab make **Enable Yes** and **Listen All Yes**.
5. Then under IP Address under IPALL provide TCP Port (ex. **49172**)
6. Then Apply and OK.
7. Go to **SQL Server Services** and restart **SQL Server (SQLEXPRESS)**. Make **SQL Server Browser** is running. If not then start it.
8. Setup inbound rule on FireWall for **TCP port 49172** which we setup in **SQL Server Configuration Manager**.
9. To check the port is available or not, run the command **"netstat /a /n /o >d:\netstat.txt"** in cmd and check the port number in the text file. If not then configure.
10. Get IP address of your PC by running the command **"ipconfig"** and note down suitable **IPv4** Address.
11. Proper connection string patter: **"Server={IP_Address},49172\\SQLEXPRESS;Initial Catalog=DockerApi;User ID={username};Password={password};"**. Provide **{username}** user DB access of **DockerApi** from
    **sa** user under **security**.
	
## Create docker volume and mapped it with image
1. Create a Create-volume.bat file and in it put the following command 
   "docker volume create --driver local --opt type=none --opt device={Physical location of the file} --opt o=bind {volume name}"
2. run the following command from context to bind the created volume and image
   "docker run -it -p 8000:80 -v {volume name}:/app/Logs {image name}"
3. run the following command from context to bind the multiple virtual path with volume
   "docker run -it -p 8000:80 -v {volume_name}:{virtual_path_1} -v {volume_name}:{virtual_path_2} -v {volume_name}:{virtual_path_3} {image_name}"
	
## Docker compose
1. To run docker compose use the following command: docker-compose up -d
2. To stop run the following command: docker-compose stop

**Notes:**
1. By running docker compose you don't need to create image, run the image and run the volume according to docker-compose.yml file.
2. To work with volume in docker compose first create the volume manually and then use it docker-compose.yml file.
	
**Other useful commands**
1. docker ps -> show the list of containers
2. docker images -> show the list of docker images
3. docker volume ls -> show the list of volumes
4. docker volume rm {volume name} -> To delete a specific volume
	
	
