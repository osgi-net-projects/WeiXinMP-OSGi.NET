@echo off

RMDIR /S /Q Deploy
mkdir Deploy 
xcopy /E UIShell.WeChatShell Deploy
pushd Deploy

ERASE /S /Q *.cs
ERASE /S /Q *.snk 
ERASE /S /Q *.resx 
ERASE /S /Q *.user 
ERASE /S /Q *.pdb
ERASE /S /Q *.csproj
ERASE /S /Q *.edmx
ERASE /S /Q *.tt
ERASE /S /Q *.idc
ERASE /S /Q *.Publish.xml
ERASE /S /Q *.xls
ERASE /S /Q persistent.xml
ERASE /S /Q log.txt
 
popd
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S') DO RD "%%G"

