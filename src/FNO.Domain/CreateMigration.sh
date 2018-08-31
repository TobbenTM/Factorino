#!/bin/bash

dotnet ef migrations add Migration -s ..\FNO.ReadModel\
dotnet ef database update -s ..\FNO.ReadModel\
