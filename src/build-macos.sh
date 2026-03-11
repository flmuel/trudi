#!/bin/sh
# TRuDI macOS Build Script
#
# Usage:
#   ./build-macos.sh [arm64|x64|all]

set -eu

TARGET="${1:-}"
if [ -z "$TARGET" ]; then
    MACHINE_ARCH="$(uname -m)"
    if [ "$MACHINE_ARCH" = "arm64" ]; then
        TARGET="arm64"
    else
        TARGET="x64"
    fi
fi

case "$TARGET" in
    x64)
        RIDS="osx-x64"
        ELECTRON_ARCH_ARGS="--x64"
        ;;
    arm64)
        RIDS="osx-arm64"
        ELECTRON_ARCH_ARGS="--arm64"
        ;;
    all)
        RIDS="osx-x64 osx-arm64"
        ELECTRON_ARCH_ARGS="--x64 --arm64"
        ;;
    *)
        echo "Unsupported target '$TARGET'. Use one of: x64, arm64, all"
        exit 1
        ;;
esac

echo "Building TRuDI for macOS target: $TARGET"

cd TRuDI.Backend
rm -rf bin/dist

dotnet build -c Release

for RID in $RIDS; do
    echo "Publishing backend for $RID"
    dotnet publish -c Release -r "$RID" --self-contained -o "bin/dist/$RID" -p:SelfContainedBuild=true

    # Delete files not needed for TRuDI deployment
    find "bin/dist/$RID" -name '*.pdb' -delete
    rm -rf "bin/dist/$RID/es"
    rm -rf "bin/dist/$RID/fr"
    rm -rf "bin/dist/$RID/it"
    rm -rf "bin/dist/$RID/ja"
    rm -rf "bin/dist/$RID/ko"
    rm -rf "bin/dist/$RID/ru"
    rm -rf "bin/dist/$RID/zh-Hans"
    rm -rf "bin/dist/$RID/zh-Hant"
done

cd ../TRuDI.Frontend
npm install

for RID in $RIDS; do
    if [ "$RID" = "osx-x64" ]; then
        CHECKSUM_FILE="checksums-darwin-x64.json"
    else
        CHECKSUM_FILE="checksums-darwin-arm64.json"
    fi

    echo "Generating checksum file $CHECKSUM_FILE"
    node ../Utils/createDigestList.js "../TRuDI.Backend/bin/dist/$RID" "$CHECKSUM_FILE"
done

npx electron-builder --mac $ELECTRON_ARCH_ARGS

rm -f ../../dist/*.blockmap

echo "macOS build finished."
