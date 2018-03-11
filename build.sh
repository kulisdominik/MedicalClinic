#!/usr/bin/env bash

#exit if any command fails
set -e

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then  
  rm -R $artifactsFolder
fi

dotnet restore

dotnet test ./test/MedicalClinic -c Release -f netcoreapp1.0

dotnet build ./test/MedicalClinic-c Release -f net451

mono \  
./test/MedicalClinic/bin/Release/net451/*/dotnet-test-xunit.exe \
./test/MedicalClinic/bin/Release/net451/*/TEST_PROJECT_NAME.dll

revision=${TRAVIS_JOB_ID:=1}  
revision=$(printf "%04d" $revision) 

dotnet pack ./src/PROJECT_NAME -c Release -o ./artifacts --version-suffix=$revision  
