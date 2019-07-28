# Build binary

`dotnet publish -c Release`
`warp-packer --arch linux-x64 --input_dir unarchive/bin/Release/netcoreapp3.0/linux-x64/publish --exec unarchive --output unarchive`
