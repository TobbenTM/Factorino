#!/bin/bash

dotnet ef migrations add ScriptedMigration_`date +%Y%m%d%H%M%S` -s ../FNO.ReadModel/
dotnet ef database update -s ../FNO.ReadModel/
