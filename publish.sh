dotnet publish -c Release
warp-packer --arch linux-x64 --input_dir Unarchive/bin/Release/netcoreapp3.0/linux-x64/publish --exec unarchive --output ~/.bin/unarchive
