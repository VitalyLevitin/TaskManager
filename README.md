# Task Manager  

CRUD, filtering and sorting system for tasks and users.  
Used C# with .NET6 as the backend and MySQL as the database.  

## Running the Web App Locally  
To run the web app on your local machine, follow these steps:  

1. Clone the project repository to your local machine using Git or download the ZIP file from the project page on GitHub.  
2. Navigate to the project folder using a terminal or command prompt.  
3. Ensure that you have .NET 6 or above installed on your system. If you don't have it installed, download and install it        from the official .NET website.  
4. Run the command **dotnet restore** in the terminal to install all the required packages for the project.  
5. Run the command **dotnet run** in the terminal to start the web app. This will launch a web-based GUI that you can use to    test the different endpoints.  
That's it!  

## Tools used   
SQLTools   
SQLTools MySQL/MariaDB   

### Setup old MySql type of auth:  
If you get the error that the "Client doesn't support auth protocol request by server, create a new MySQL user with the following commands in the command prompt.  
CREATE USER 'sqluser'@'%' IDENTIFIED WITH your_password BY 'password';  
GRANT ALL PRIVILEGES ON *.* TO 'sqluser'@'%';  
FLUSH PRIVILEGES;  
