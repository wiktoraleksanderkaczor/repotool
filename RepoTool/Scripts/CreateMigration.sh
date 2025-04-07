#!/bin/bash
read -p "Enter migration name: " MIGRATION_NAME
dotnet ef migrations add $MIGRATION_NAME --context RepoToolDbContext --output-dir Persistence/Migrations --project ../RepoTool.csproj

if [ $? -ne 0 ]; then
    echo "Error: Failed to create migration."
    exit 1
fi