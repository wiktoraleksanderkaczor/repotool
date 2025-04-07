#!/bin/bash

# Get the list of migrations in JSON format
dotnet ef migrations list --json --context RepoToolDbContext --project ../RepoTool.csproj