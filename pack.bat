@echo off
call delfile.bat SAE.7z
"C:\Program Files\7-Zip\7z.exe" a -r -t7z -y -xr!?svn\* -xr!TestResults\* SAE.7z *
prompt
