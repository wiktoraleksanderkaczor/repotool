#!/bin/bash

# JSON output looks like this:
# [
#   {
#     "id": "20250312235404_a",
#     "name": "a",
#     "safeName": "a",
#     "applied": false
#   },
#   {
#     "id": "20250312235409_b",
#     "name": "b",
#     "safeName": "b",
#     "applied": false
#   },
#   {
#     "id": "20250312235415_c",
#     "name": "c",
#     "safeName": "c",
#     "applied": false
#   }
# ]

read -p "Enter migration name to remove up to: " TARGET_MIGRATION

# Get the list of migrations in JSON format
FULL_OUTPUT=$(dotnet ef migrations list --json --context RepoToolDbContext --project ../RepoTool.csproj)

# Extract only the JSON part from the output (everything between the first '[' and the last ']')
MIGRATIONS_JSON=$(echo "$FULL_OUTPUT" | awk '/\[/,/\]/' | tr -d '\r')

# Check if the migrations list is empty
if [ -z "$MIGRATIONS_JSON" ]; then
    echo "No migrations found."
    exit 1
fi

# Check if the output is valid JSON
if ! echo "$MIGRATIONS_JSON" | jq . > /dev/null 2>&1; then
    echo "Error: Invalid JSON output from dotnet ef migrations list command."
    echo "Output was: $FULL_OUTPUT"
    exit 1
fi

# Parse the JSON to find the target migration and migrations to remove
TARGET_MIGRATION_ID=$(echo "$MIGRATIONS_JSON" | jq -r '.[] | select(.name == "'"${TARGET_MIGRATION}"'") | .id')

if [ -z "$TARGET_MIGRATION_ID" ]; then
    echo "Target migration '$TARGET_MIGRATION' not found."
    exit 1
fi

# Check for applied migrations before the target migration
APPLIED_BEFORE_TARGET=$(echo "$MIGRATIONS_JSON" | jq -r ".[] | select(.id <= \"$TARGET_MIGRATION_ID\" and .applied == true) | .name")

if [ ! -z "$APPLIED_BEFORE_TARGET" ]; then
    echo "Error: Cannot remove migrations. There are applied migrations up to the target migration."
    exit 1
fi

MIGRATIONS_TO_REMOVE=$(echo "$MIGRATIONS_JSON" | jq -r ".[] | select(.id > \"$TARGET_MIGRATION_ID\" and .applied == false) | .name" | sort -r)

# Loop through the migrations to remove and execute the remove command
while IFS= read -r MIGRATION_NAME; do
    echo "Removing migration: $MIGRATION_NAME"
    dotnet ef migrations remove --context RepoToolDbContext --project ../RepoTool.csproj
done <<< "$MIGRATIONS_TO_REMOVE"

echo "Migrations removed up to: $TARGET_MIGRATION"