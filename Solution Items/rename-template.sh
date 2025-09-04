#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "Usage: $0 <NewName>" >&2
  exit 1
fi

OLD_NAME="VsaSample"
NEW_NAME="$1"

# Replace occurrences inside files, excluding .git directory and this script
find . -path ./.git -prune -o -name rename-template.sh -prune -o -type f -print | \
  xargs sed -i "s/${OLD_NAME}/${NEW_NAME}/g"

# Rename solution file
if [[ -f "${OLD_NAME}.sln" ]]; then
  mv "${OLD_NAME}.sln" "${NEW_NAME}.sln"
fi

# Rename project directories and files
if [[ -d "src/${OLD_NAME}" ]]; then
  mv "src/${OLD_NAME}" "src/${NEW_NAME}"
  if [[ -f "src/${NEW_NAME}/${OLD_NAME}.csproj" ]]; then
    mv "src/${NEW_NAME}/${OLD_NAME}.csproj" "src/${NEW_NAME}/${NEW_NAME}.csproj"
  fi
fi

if [[ -d "tests/${OLD_NAME}.Tests" ]]; then
  mv "tests/${OLD_NAME}.Tests" "tests/${NEW_NAME}.Tests"
  if [[ -f "tests/${NEW_NAME}.Tests/${OLD_NAME}.Tests.csproj" ]]; then
    mv "tests/${NEW_NAME}.Tests/${OLD_NAME}.Tests.csproj" "tests/${NEW_NAME}.Tests/${NEW_NAME}.Tests.csproj"
  fi
fi

echo "Renamed project to ${NEW_NAME}"
