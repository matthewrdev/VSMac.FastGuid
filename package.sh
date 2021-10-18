# Capture the commands provided by the user

./clean.sh

BUILD_DATE=$(date +"%y-%m-%d-%H-%M-%S")
BUILD_FOLDER=Builds/mpack/$BUILD_DATE

echo "Cleaning up junk files and old builds..."
mkdir ./Builds
mkdir ./Builds/mpack
mkdir ./Builds/mpack/$BUILD_DATE

echo "Restoring nuget packages for FastGuid..."
nuget restore ./FastGuid/FastGuid.sln

echo "Building FastGuid..."
msbuild /p:MDProfileVersion=8.0 /p:Configuration=Release ./FastGuid/FastGuid.sln


echo "Packaging addin..."
echo "  Output path is: ./Builds/mpack/$BUILD_DATE"

# Package Fast Tool using vstool
/Applications/Visual\ Studio.app/Contents/MacOS/vstool setup pack ./FastGuid/FastGuid/bin/Release/net471/FastGuid.dll -d:./Builds/mpack/$BUILD_DATE

open $BUILD_FOLDER
