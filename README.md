# Project Brief

## Overview

This project, RepoTool, is a command-line utility designed to analyze and interact with code repositories. It provides functionalities such as parsing code, searching for specific patterns, summarizing code changes, and managing repository metadata. The core of RepoTool's functionality relies on a novel approach to code analysis, leveraging a Large Language Model (LLM) with structured output and tool-calling capabilities. This enables a highly interactive and granular parsing process, operating on code blocks and individual statements within those blocks.

## Goals

- Provide a robust and efficient tool for code analysis.
- Offer various commands for different repository-related tasks.
- Support multiple programming languages.
- Maintain a clear and well-documented codebase.
- Ensure extensibility for future features.

## Key Features

- **Parsing:**
    - **LLM-Driven Analysis:** Utilizes an LLM to analyze code structure and extract relevant information.
    - **Structured Output:** Employs a JSON schema for all LLM responses, ensuring accuracy, consistency, and enabling complex data representation without the limitations of polymorphic function calls.
    - **Interactive Parsing:** The parsing process is interactive, allowing the LLM to request specific actions and information from the system through tool calls.
    - **Code Navigation Tools:**
        - *Scroll Tool:* Allows the LLM to advance the displayed code window, providing context for analysis.
        - *Line-Numbered Windows:* Code is presented in line-numbered windows with a configurable number of lines, providing precise location information.
    - **Block-Based Processing:**
        - *Block Definition:* Code is divided into logical blocks (e.g., functions, classes, methods).
        - *Block Start/End Tools:* Tools are provided for the LLM to explicitly start and end blocks, specifying the block type (from a predefined enum) and name. The starting line number is included.
        - *Nested Blocks:* Supports nested blocks to represent hierarchical code structures.
        - *Block Context:* The current block hierarchy (including parameters, if any) is included in the LLM's action context.
    - **Statement-Level Processing:**
        - *Statement Types:* Individual statements within blocks are processed separately.
        - *Statement Part Decomposition:* Statements may be parsed in multiple parts to avoid polymorphism in the JSON schema.
        - *Block Statements:* Special "block statements" allow for control flow constructs within blocks (branching, looping, error handling).
    - **Language-Agnostic Querying:**
        - *Unified Query Language:* A custom query language is used to interact with the parsed code, treating code blocks as standard statements. This allows for consistent querying across different programming languages.
        - *Language-Specific Tags:* Language-specific features that don't fit into the standard block structure are represented as "tags."
        - *Cross-Language Comparisons:* Tags can be compared across languages based on their embedding (relative position within the file), enabling analysis of similar features even without explicit implementation.
    - **Action History:**
        - *Contextual Awareness:* A list of previous actions taken by the LLM is provided as context.
- **Searching:** Find specific patterns or code elements within the repository, utilizing LLM-powered analysis and tool calls.
- **Summarization:** Generate concise summaries of code changes, driven by LLM analysis and structured data.
- **Indexing:** Create and manage an index for faster repository analysis.
- **Language Support:** Handle multiple programming languages through configurable settings and LLM adaptability.
- **Caching:** Store analysis results for improved performance.
- **Changelog Generation:** Automatically create changelogs based on code commits and LLM-powered summarization.

## Technologies

- C#
- .NET
- LLM (for code parsing, analysis, and tool calling)

## Future Considerations

- Integration with other tools and services.
- Enhanced visualization of code analysis results.
- Support for more advanced code analysis techniques.

## Acceptance Criteria

- The tool successfully parses code from at least three different programming languages (e.g., C#, Python, JavaScript).
- The search command accurately identifies code elements based on user-provided patterns.
- The summarization command generates summaries that accurately reflect code changes.
- The indexing functionality improves the performance of subsequent analysis operations.
- The tool handles common error scenarios gracefully and provides informative error messages.
- The codebase adheres to the defined coding guidelines and style conventions.
- Unit tests cover at least 80% of the codebase.
- The tool successfully installs and runs on the target operating systems (Linux).
- The LLM interaction adheres to the specified structured output and tool-calling mechanisms.
- The parsing process correctly handles blocks, statements, and language-specific tags.
- The query language allows for effective retrieval of information from the parsed code.