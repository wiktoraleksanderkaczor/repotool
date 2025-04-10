# RepoTool

## Overview

RepoTool is a command-line utility built with .NET/C# designed to analyze and interact with code repositories. It provides functionalities such as LLM-driven code parsing, pattern searching, code summarization, index management for analysis, language configuration, caching, and potentially changelog generation based on repository history.

The core of RepoTool's parsing functionality relies on a novel approach leveraging a Large Language Model (LLM) with structured output (JSON schema) and tool-calling capabilities. This enables a highly interactive and granular parsing process, operating on code blocks and individual statements within those blocks, aiming for detailed, language-agnostic analysis.

## Table of Contents
- [Overview](#overview)
- [Goals](#goals)
- [Key Features](#key-features)
- [Technologies](#technologies)
- [Project Status & Usage](#project-status--usage)
- [Future Considerations](#future-considerations)
- [Licences](#licences)

## Goals

- Provide a robust and efficient tool for code analysis.
- Offer various commands for different repository-related tasks (parsing, searching, summarization, indexing, etc.).
- Support multiple programming languages through configuration.
- Maintain a clear and well-documented codebase.
- Ensure extensibility for future features.

## Key Features

- **Parsing (`ParseCommand`):**
    - **LLM-Driven Analysis:** Utilizes an LLM to analyze code structure and extract relevant information.
    - **Structured Output:** Employs a JSON schema for all LLM responses, ensuring accuracy, consistency, and enabling complex data representation.
    - **Interactive Parsing:** The parsing process is interactive, allowing the LLM to request specific actions and information from the system through tool calls.
    - **Code Navigation Tools:** Includes tools like scrolling (`ScrollDown`, `PageDown`) within line-numbered code windows for contextual analysis.
    - **Block-Based Processing:** Code is divided into logical blocks (e.g., functions, classes). Tools allow the LLM to define block boundaries (`BlockSelector`, `BlockConstruct`) and types, supporting nested structures. Block context is provided to the LLM.
    - **Statement-Level Processing:** Individual statements within blocks are processed (`StatementSelector`, `StatementConstruct`), potentially decomposed into parts to fit the schema, including control flow constructs.
    - **Language-Agnostic Representation:** Aims for a unified representation of code constructs (`Construct` base model) across languages, using language-specific details where necessary.
    - **Action History:** Provides previous LLM actions as context.
- **Searching (`SearchCommand`):** Find specific patterns or code elements within the repository. (Implementation details may vary).
- **Summarization (`SummarizeCommand`):** Generate concise summaries of code or changes, likely leveraging LLM analysis.
- **Indexing (`Index/` commands):** Create and manage an index (`UpdateIndexCommand`, `ShowIndexCommand`, `ClearIndexCommand`) for potentially faster repository analysis or changelog generation.
- **Language Support (`Language/` commands):** Handle multiple programming languages through configurable settings (`AddLanguageCommand`, `ListLanguageCommand`, `RemoveLanguageCommand`, `Available/` commands).
- **Caching (`Cache/` commands):** Store analysis or inference results (`ClearCacheCommand`, `ShowCacheCommand`, `InferenceCacheEntity`) for improved performance.
- **Changelog Generation:** Functionality related to generating changelogs appears present (`ChangelogEntity`, `ChangelogContext`), likely based on code commits and summarization.

## Technologies

- C# / .NET
- Spectre.Console (for CLI)
- Entity Framework Core (for persistence)
- Large Language Models (LLMs) via providers (e.g., OpenAI, Ollama and Outlines (via vLLM)). Latter being preferred for much better schema support.
- Scriban (for templating)

## Project Status & Usage

This project is currently under development.

**Building:**
Use the standard .NET CLI build command from the root directory:
```bash
dotnet build
```

**Running:**
After building, you can run the tool using the `dotnet run` command, specifying the `RepoTool` project and passing the desired command and options:
```bash
dotnet run --project RepoTool/RepoTool.csproj -- [command] [options]
```
For example, to see available commands:
```bash
dotnet run --project RepoTool/RepoTool.csproj -- --help
```

First command to be ran will have to be `init`.

**Configuration:**
- LLM provider details (API keys, endpoints) need to be configured. See documentation for configuration file in `.repotool/settings.json`.
- Persistence is handled via EF Core, likely using SQLite by default. The database file location will be in the initialised project folder within the repository.

## Future Considerations

- Integration with other tools and services.
- Enhanced visualization of code analysis results.
- Support for more advanced code analysis techniques.

## Licences

Licence for this project is contained in [the licence file](LICENCE) and libraries this project uses are in [this licences file](LICENCES.md). I extend my heartfelt gratitude to the developers of these libraries and the open-source community.