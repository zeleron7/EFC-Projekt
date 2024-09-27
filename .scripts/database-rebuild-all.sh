#!/bin/bash
#To make the .sh file executable
#sudo chmod +x ./database-update.sh

#Make sure DbConnection is: SQLServer-musicefc-azkv-docker-sysadmin
#This should be "DbSetActiveIdx": 0  in DbContext/appsettings.json

#If EFC tools needs update use:
#dotnet tool update --global dotnet-ef

#drop any database
dotnet ef database drop -f -c SqlServerDbContext -p ../DbContext -s ../DbContext

#remove any migration
rm -rf ../DbContext/Migrations

#make a full new migration
dotnet ef migrations add miInitial -c SqlServerDbContext -p ../DbContext -s ../DbContext -o ../DbContext/Migrations/SqlServerDbContext

#update the database from the migration
dotnet ef database update -c SqlServerDbContext -p ../DbContext -s ../DbContext

#to iinitialize the database you need to run the sql scripts from Azure Data Studio
#../DbContext/SqlScripts/initDatabase.sql
#or run ./database-init.sh
