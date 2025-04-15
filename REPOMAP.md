# Repository Structure Map

This document outlines the structure of the repository, separated by project and root-level files.

## Root Files

Contains configuration, documentation, and solution files at the root of the repository.

- `.editorconfig` - Defines coding styles and formatting rules.
- `.env.template` - Template file for environment variable configuration.
- `.gitignore` - Specifies intentionally untracked files that Git should ignore.
- `.repomixignore` - Specifies files to be ignored by the Repomix tool.
- `.rooignore` - Specifies files and directories that Roo should ignore (marked as 🔒).
- `CODE_OF_CONDUCT.md` - Outlines standards for community participation.
- `DECISIONS.md` - Records architectural and design decisions.
- `docker-compose.yml` - Defines services, networks, and volumes for Docker application setup.
- `ISSUES.md` - Tracks known issues or areas for improvement.
- `LICENCE` - Contains the primary software license for the repository.
- `LICENCES.md` - Contains license information for third-party dependencies.
- `README.md` - Provides a general overview and introduction to the repository.
- `REPOMAP.md` - This file, documenting the repository structure.
- `RepoTool.sln` - Visual Studio Solution file grouping the projects.
- `SBOM.md` - Software Bill of Materials, listing dependencies.
- `TODO.md` - Tracks pending tasks and future work.

## Common Project (`Common/`)

Contains shared utilities or models potentially used by multiple projects within the solution.

- `Common.csproj` - Defines the project structure, dependencies, and build settings for the Common library.
  *(Note: Currently, this project only contains the project file. Add functional groups like 'Helpers', 'Models', etc., as files are added.)*

## RepoTool Project (`RepoTool/`)

The main application project containing the core logic, CLI commands, parsing capabilities, and persistence layer.

### Core

Contains the main entry point, dependency injection setup, and project definition for the RepoTool application.

- `DependencyInjection.cs` - Configures dependency injection services for the application.
- `Program.cs` - Defines the main entry point for the application.
- `RepoTool.csproj` - Defines the project structure, dependencies, and build settings.

### Attributes

Contains custom attributes used throughout the application, particularly for parsing and dependency injection.

- `Attributes/` - Contains custom attribute definitions.
  - `Helpers/` - Contains attributes related to helper functionalities.
    - `SingletonAttribute.cs` - Defines an attribute to mark services for singleton registration.
  - `Parser/` - Contains attributes related to the code parsing functionality.
    - `ItemChoiceAttribute.cs` - Defines an attribute for specifying choices within parsed items.
    - `JsonSpecialAttribute.cs` - Defines an attribute for special handling during JSON schema generation.
    - `ToolChoiceAttribute.cs` - Defines an attribute for specifying tool choices during parsing.

### Constants

Contains constant values used across the application.

- `Constants/` - Contains constant definitions.
  - `PathConstants.cs` - Defines constant values for file system paths.
  - `ResourceConstants.cs` - Defines constant values related to embedded resources.
  - `TemplateConstants.cs` - Defines constant values related to templating.
  - `TypeConstants.cs` - Defines constant values related to type information.

### Enums

Contains enumeration types used for categorization and flags.

- `Enums/` - Contains enumeration definitions.
  - `Changelog/` - Contains enums related to changelog generation.
    - `EnChangeArea.cs` - Defines areas affected by code changes.
    - `EnChangeImportance.cs` - Defines importance levels for changes.
    - `EnChangePerformanceImpact.cs` - Defines performance impact levels.
    - `EnChangeQuality.cs` - Defines quality assessment levels for changes.
    - `EnChangeSize.cs` - Defines size categories for changes.
    - `EnChangeSpecialization.cs` - Defines specialization areas for changes.
    - `EnChangeTechDebt.cs` - Defines technical debt impact levels.
    - `EnChangeType.cs` - Defines types of code changes.
  - `Inference/` - Contains enums related to AI inference operations.
    - `EnInferenceProvider.cs` - Defines the available AI inference providers.
    - `EnInferenceReason.cs` - Defines reasons for performing inference.
    - `EnInferenceRole.cs` - Defines roles within an inference conversation (e.g., User, Assistant).
  - `Json/` - Contains enums related to JSON handling.
    - `EnOutputHandlingType.cs` - Defines how JSON output should be handled.
  - `Parser/` - Contains enums related to code parsing.
    - `EnLanguageParadigm.cs` - Defines programming language paradigms.
    - `Common/` - Contains common enums for parsing across languages.
      - `EnCallableModifierFlag.cs` - Defines flags for callable construct modifiers (e.g., static, async).
      - `EnGenericConstraintTypeFlag.cs` - Defines flags for generic type constraint types.
      - `EnParameterModifierFlag.cs` - Defines flags for parameter modifiers (e.g., ref, out).
      - `EnTypeInfoModifierFlag.cs` - Defines flags for type information modifiers (e.g., nullable).
    - `Items/` - Contains enums related to specific parsed code items.
      - `Blocks/` - Contains enums for block-level items.
        - `EnInheritanceType.cs` - Defines types of inheritance (e.g., inherits, implements).
        - `EnStructModifierFlag.cs` - Defines flags for struct modifiers (e.g., union).
      - `Common/` - Contains common enums for parsed items.
        - `EnAttributeTarget.cs` - Defines possible targets for attributes.
      - `Directives/` - Contains enums for directive items.
        - `EnDefinitionType.cs` - Defines types for definition directives (#define, #undef).
        - `EnDiagnosticLevel.cs` - Defines levels for diagnostic directives (#error, #warning).
        - `EnLineDirectiveModifierFlag.cs` - Defines flags for line directive modifiers.
      - `Expressions/` - Contains enums for expression items.
        - `EnBracketType.cs` - Defines types of brackets used in expressions.
        - `EnCallableUseModifierFlag.cs` - Defines flags for callable use modifiers.
        - `EnFormatStringComponentType.cs` - Defines types of components within format strings.
        - `Operators/` - Contains enums for operator expression items.
          - `EnBinaryOperator.cs` - Defines specific binary operators.
          - `EnTernaryOperator.cs` - Defines specific ternary operators.
          - `EnUnaryOperator.cs` - Defines specific unary operators.
          - `Common/` - Contains common enums for operator items.
            - `EnOperatorAssociativity.cs` - Defines associativity for operators.
      - `Statements/` - Contains enums for statement items.
        - `EnControlFlowStatementType.cs` - Defines types of control flow statements.
        - `EnImportModifierFlag.cs` - Defines flags for import statement modifiers.
        - `EnMarkingStatementType.cs` - Defines types of marking statements (goto, labels).
    - `Tools/` - Contains enums related to parser tooling models.
      - `EnConstructType.cs` - Defines the primary types of code constructs the parser can identify.
      - `Blocks/` - Contains enums for block-level constructs within tools.
        - `EnClassModifierFlag.cs` - Defines flags for class modifiers (e.g., abstract, sealed).
        - `EnTypeModifierFlag.cs` - Defines flags for general type modifiers.
      - `Builders/` - Contains enums for builder tools.
        - `EnIterableBuilderTool.cs` - Defines tool choices for the iterable builder.
        - `EnObjectBuilderTool.cs` - Defines tool choices for the object builder.
        - `Iterable/` - Contains enums specific to the iterable builder.
          - `EnIterableInsertAt.cs` - Defines insertion points for iterable items.
      - `Common/` - Contains common enums for parser tooling.
        - `EnAccessModifierFlag.cs` - Defines flags for access modifiers (e.g., public, private).
        - `EnInheritanceModifierFlag.cs` - Defines flags for inheritance modifiers (e.g., virtual, override).
        - `EnVariableModifierFlag.cs` - Defines flags for variable modifiers (e.g., const, readonly).
      - `Declarations/` - Contains enums for declaration-level constructs within tools.
        - `EnDeclarationModifierFlag.cs` - Defines flags for general declaration modifiers.
        - `EnPropertyModifierFlag.cs` - Defines flags for property modifiers (e.g., init, required).
      - `Selectors/` - Contains enums for selector tools.
        - `EnBlockType.cs` - Defines types of code blocks (class, struct, etc.).
        - `EnDeclarationType.cs` - Defines types of declarations (property, field, etc.).
        - `EnDirectiveType.cs` - Defines types of preprocessor directives.
        - `EnExpressionType.cs` - Defines types of expressions.
        - `EnStatementType.cs` - Defines types of statements.
        - `Expressions/` - Contains enums specific to expression selectors.
          - `EnOperatorType.cs` - Defines categories of operators (unary, binary, ternary).
  - `Schemas/` - Contains enums related to JSON schema generation.
    - `EnOllamaType.cs` - Defines data types allowed in Ollama output schemas.
    - `EnOpenAIType.cs` - Defines data types allowed in OpenAI output schemas.

### Extensions

Contains extension methods for various types to provide additional functionality.

- `Extensions/` - Contains extension method definitions.
  - `JsonDocumentExtensions.cs` - Defines extension methods for `System.Text.Json.JsonDocument`.
  - `JsonSchemaExtensions.cs` - Defines extension methods for `JsonSchema`.
  - `ObjectExtensions.cs` - Defines general extension methods for `object`.
  - `PropertyInfoExtensions.cs` - Defines extension methods for `System.Reflection.PropertyInfo`.
  - `ServiceCollectionExtensions.cs` - Defines extension methods for `Microsoft.Extensions.DependencyInjection.IServiceCollection`.
  - `StringExtensions.cs` - Defines extension methods for `string`.
  - `TypeExtensions.cs` - Defines extension methods for `System.Type`.

### Flags

Contains flag enumerations, often used for bitmask operations.

- `Flags/` - Contains flag enumeration definitions.
  - `Parser/` - Contains flags related to code parsing.
    - `JsonSpecialFlag.cs` - Defines flags for special handling during JSON schema generation based on attributes.

### Helpers

Contains utility classes providing reusable logic for various tasks.

- `Helpers/` - Contains helper class definitions.
  - `DocumentationHelper.cs` - Handles generation of documentation from parsed code structures.
  - `InferenceHelper.cs` - Handles interactions with AI inference providers.
  - `JsonHelper.cs` - Provides utilities for JSON serialization and deserialization.
  - `ParserHelper.cs` - Handles the code parsing process orchestration.
  - `RepositoryHelper.cs` - Provides utilities for interacting with Git repositories.
  - `ResourceHelper.cs` - Provides utilities for accessing embedded resources.
  - `TemplateHelper.cs` - Handles processing of Scriban templates.
  - `ToolingHelper.cs` - Provides miscellaneous helper methods for the tool's operations.

### Models

Contains data transfer objects (DTOs) and models representing application data structures.

- `Models/` - Contains data model definitions.
  - `Documentation/` - Contains models related to code documentation generation.
    - `PropertyDocumentation.cs` - Represents documentation for a property.
    - `TypeDocumentation.cs` - Represents documentation for a type.
    - `Common/` - Contains common base models for documentation.
      - `BaseDocumentation.cs` - Defines the base class for documentation models.
  - `Inference/` - Contains models related to AI inference.
    - `InferenceMessage.cs` - Represents a single message in an inference conversation.
    - `InferenceRequest.cs` - Represents a request sent to an inference provider.
    - `TemplateData.cs` - Represents data used for populating inference templates.
    - `Contexts/` - Contains context models for different inference tasks.
      - `ChangelogContext.cs` - Represents the context for changelog generation inference.
      - `ParserContext.cs` - Represents the context for code parsing inference.
      - `SummarizationContext.cs` - Represents the context for code summarization inference.
      - `Common/` - Contains common base models for inference contexts.
        - `InferenceContext.cs` - Defines the base class for inference context models.
      - `Parser/` - Contains specific context models for the parser inference task.
        - `ActionWindow.cs` - Represents the available actions window for the parser agent.
        - `CodeWindow.cs` - Represents the current code window view for the parser agent.
        - `ItemPath.cs` - Represents the path to the current item being parsed.
  - `Parser/` - Contains models representing parsed code structures.
    - `Interfaces/` - Contains interfaces related to parsing models.
      - `IToolSelector.cs` - Defines an interface for tool selection models used by the parser agent.
    - `Items/` - Contains models for different types of code constructs (blocks, declarations, etc.).
      - `Blocks/` - Contains models for block-level constructs (classes, methods, etc.).
        - `CallableBlock.cs` - Represents a callable block (method, function).
        - `ClassBlock.cs` - Represents a class block.
        - `CustomBlock.cs` - Represents a custom or unrecognized block.
        - `EnumBlock.cs` - Represents an enum block.
        - `InterfaceBlock.cs` - Represents an interface block.
        - `NamespaceBlock.cs` - Represents a namespace block.
        - `RecordBlock.cs` - Represents a record block.
        - `StructBlock.cs` - Represents a struct block.
        - `Common/` - Contains common base models for block constructs.
          - `BlockConstruct.cs` - Defines the base class for block constructs.
      - `Common/` - Contains common models used across different construct types.
        - `AttributeDefinition.cs` - Represents an attribute applied to a construct.
        - `CallableInfoDefinition.cs` - Represents information about a callable construct (return type, parameters).
        - `Construct.cs` - Defines the base class for all parsed code constructs.
        - `ParameterDefinition.cs` - Represents a parameter definition.
        - `TypeInfoDefinition.cs` - Represents type information (name, generics, etc.).
      - `Declarations/` - Contains models for declaration constructs (fields, properties, etc.).
        - `DelegateDeclaration.cs` - Represents a delegate declaration.
        - `EventDeclaration.cs` - Represents an event declaration.
        - `FieldDeclaration.cs` - Represents a field declaration.
        - `PropertyDeclaration.cs` - Represents a property declaration.
        - `Common/` - Contains common base models for declaration constructs.
          - `DeclarationConstruct.cs` - Defines the base class for declaration constructs.
      - `Directives/` - Contains models for preprocessor directives.
        - `BranchingDirective.cs` - Represents branching directives (#if, #else).
        - `CustomDirective.cs` - Represents custom or unrecognized directives.
        - `DefinitionDirective.cs` - Represents definition directives (#define, #undef).
        - `DiagnosticDirective.cs` - Represents diagnostic directives (#error, #warning).
        - `IncludeDirective.cs` - Represents include directives (#load, #r).
        - `LineDirective.cs` - Represents line directives (#line).
        - `PragmaDirective.cs` - Represents pragma directives (#pragma).
        - `RegionDirective.cs` - Represents region directives (#region, #endregion).
        - `Common/` - Contains common base models for directive constructs.
          - `DirectiveConstruct.cs` - Defines the base class for directive constructs.
      - `Expressions/` - Contains models for expression constructs.
        - `BracketExpression.cs` - Represents bracketed expressions.
        - `CallableUseExpression.cs` - Represents the invocation of a callable.
        - `ComprehensionExpression.cs` - Represents list/dict comprehensions (Python-like).
        - `FormatStringExpression.cs` - Represents formatted string literals.
        - `LambdaExpression.cs` - Represents lambda expressions.
        - `LiteralValueExpression.cs` - Represents literal values (numbers, strings, etc.).
        - `NewVariableExpression.cs` - Represents the creation of a new variable/object.
        - `VariableUseExpression.cs` - Represents the usage of a variable.
        - `Common/` - Contains common base models for expression constructs.
          - `ExpressionConstruct.cs` - Defines the base class for expression constructs.
        - `Operators/` - Contains models for operator expressions.
          - `BinaryOperatorExpression.cs` - Represents binary operations.
          - `TernaryOperatorExpression.cs` - Represents ternary operations.
          - `UnaryOperatorExpression.cs` - Represents unary operations.
          - `Common/` - Contains common base models for operator expressions.
            - `OperatorExpression.cs` - Defines the base class for operator expressions.
      - `Statements/` - Contains models for statement constructs.
        - `BranchingStatement.cs` - Represents branching statements (if, switch).
        - `CallableStatement.cs` - Represents statements related to callable definitions (return, yield).
        - `ControlFlowStatement.cs` - Represents control flow statements (break, continue).
        - `CustomStatement.cs` - Represents custom or unrecognized statements.
        - `ExceptionHandlingStatement.cs` - Represents exception handling statements (try, catch, finally, throw).
        - `ExportStatement.cs` - Represents export statements (module exports).
        - `ImportStatement.cs` - Represents import statements (using, import).
        - `LoopingStatement.cs` - Represents looping statements (for, while, foreach).
        - `MarkingStatement.cs` - Represents marking statements (goto, labels).
        - `Common/` - Contains common base models for statement constructs.
          - `StatementConstruct.cs` - Defines the base class for statement constructs.
    - `Tools/` - Contains models used by the parser agent tooling.
      - `ConstructSelector.cs` - Represents the main tool for selecting construct types.
      - `SummarizeAction.cs` - Represents the action to summarize the parsed content.
      - `Builders/` - Contains models for building complex structures during parsing.
        - `IterableBuilder.cs` - Represents a tool for building iterable structures (lists, arrays).
        - `ObjectBuilder.cs` - Represents a tool for building object structures (dictionaries, objects).
        - `Common/` - Contains common models for builder tools.
          - `EndItem.cs` - Represents the action to end the current item being built.
      - `Navigation/` - Contains models for navigating the code window.
        - `PageDown.cs` - Represents the action to page down in the code window.
        - `ScrollDown.cs` - Represents the action to scroll down in the code window.
      - `Selectors/` - Contains models for selecting specific types of constructs.
        - `BlockSelector.cs` - Represents a tool for selecting block constructs.
        - `DeclarationSelector.cs` - Represents a tool for selecting declaration constructs.
        - `DirectiveSelector.cs` - Represents a tool for selecting directive constructs.
        - `ExpressionSelector.cs` - Represents a tool for selecting expression constructs.
        - `StatementSelector.cs` - Represents a tool for selecting statement constructs.
        - `Expressions/` - Contains models for selecting specific types of expressions.
          - `OperatorSelector.cs` - Represents a tool for selecting operator expressions.
  - `Repository/` - Contains models related to repository data.
    - `SourceChange.cs` - Represents a detected change in the source code.
  - `Resources/` - Contains models related to resource management.
    - `LanguageEntry.cs` - Represents an entry for a supported language.

### Options

Contains classes for application configuration options, typically bound from `appsettings.json` or environment variables.

- `Options/` - Contains configuration option model definitions.
  - `InferenceOptions.cs` - Defines configuration options for AI inference providers.
  - `Common/` - Contains common interfaces or base classes for options.
    - `IOptionModel.cs` - Defines an interface for option models.
  - `Provider/` - Contains provider-specific option models (structure may vary).

### Persistence

Contains code related to data persistence, including database context, entities, and configurations.

- `Persistence/` - Contains data persistence components.
  - `RepoToolDbContext.cs` - Defines the Entity Framework Core database context.
  - `RepoToolDbContextFactory.cs` - Defines a factory for creating `RepoToolDbContext` instances, used for design-time operations like migrations.
  - `Configuration/` - Contains EF Core entity type configurations.
    - `ChangelogEntityConfiguration.cs` - Configures the `ChangelogEntity` mapping.
    - `InferenceCacheEntityConfiguration.cs` - Configures the `InferenceCacheEntity` mapping.
    - `LanguageEntityConfiguration.cs` - Configures the `LanguageEntity` mapping.
    - `ParsedFileEntityConfiguration.cs` - Configures the `ParsedFileEntity` mapping.
    - `Common/` - Contains common base configurations.
      - `BaseEntityConfiguration.cs` - Defines a base class for entity configurations.
  - `Entities/` - Contains database entity classes.
    - `ChangelogEntity.cs` - Represents a changelog entry in the database.
    - `InferenceCacheEntity.cs` - Represents a cached inference result in the database.
    - `LanguageEntity.cs` - Represents a supported language configuration in the database.
    - `ParsedFileEntity.cs` - Represents information about a parsed file in the database.
    - `Common/` - Contains common base entities.
      - `BaseEntity.cs` - Defines a base class for database entities.
  - `Migrations/` - Contains EF Core database migrations (individual migration files are excluded from this map).

### Providers

Contains implementations for external services or abstractions, like different AI inference providers.

- `Providers/` - Contains provider implementations.
  - `OllamaProvider.cs` - Implements the inference provider interface for Ollama.
  - `OpenAIProvider.cs` - Implements the inference provider interface for OpenAI.
  - `Common/` - Contains common interfaces for providers.
    - `IInferenceProvider.cs` - Defines the interface for AI inference providers.

### Resources

Contains embedded static resources like templates and configuration files.

- `Resources/` - Contains embedded resource files.
  - `Models.xml` - Contains XML definitions or data, possibly related to models.
  - `parser-languages.json` - Defines configurations for supported languages and their parsers.
  - `Templates/` - Contains template files used for generation tasks.
    - `changelog.sbn` - Scriban template for generating changelog entries.
    - `parsing.sbn` - Scriban template used in the parsing process.
    - `README.md` - Template for generating README files (potentially).
    - `summarization.sbn` - Scriban template for generating code summaries.
    - `Partials/` - Contains partial templates included in main templates.
      - `Parsing/` - Contains partial Scriban templates for parsing documentation.
        - `base_documentation.sbn` - Partial template for base documentation elements.
        - `class_documentation.sbn` - Partial template for class documentation.
        - `generic_documentation.sbn` - Partial template for generic type/method documentation.
        - `member_documentation.sbn` - Partial template for class member documentation.
        - `property_documentation.sbn` - Partial template for property documentation.
        - `type_documentation.sbn` - Partial template for general type documentation.
        - `typed_documentation.sbn` - Partial template for elements with associated types.

### Schemas

Contains classes defining the expected structure of data, particularly for API responses or configuration.

- `Schemas/` - Contains schema definitions.
  - `OllamaOutputSchema.cs` - Defines helper methods for generating Ollama-compatible JSON schemas.
  - `OpenAIOutputSchema.cs` - Defines helper methods for generating OpenAI-compatible JSON schemas.
  - `Generators/` - Contains schema generation logic.
    - `CharSchemaGenerator.cs` - Defines a schema generator for characters to be handled as one-char length strings.
  - `Refiners/` - Contains logic for refining or modifying generated schemas.
    - `AdditionalPropertiesRefiner.cs` - Defines a refiner to handle 'additionalProperties' in JSON schemas.

### Scripts

Contains utility scripts for development tasks, like managing database migrations.

- `Scripts/` - Contains development utility scripts.
  - `CreateMigration.sh` - Script to create a new EF Core database migration.
  - `ListMigrations.sh` - Script to list existing EF Core migrations.
  - `RemoveMigration.sh` - Script to remove the last EF Core migration.
  - `RemoveToMigration.sh` - Script to remove migrations up to a specified one.
  - `RollbackToMigration.sh` - Script to roll back the database to a specified migration.

### Terminal

Contains code related to the command-line interface (CLI) using Spectre.Console.

- `Terminal/` - Contains CLI implementation components.
  - `CommandInterceptor.cs` - Defines an interceptor for modifying command execution behavior.
  - `SpectreRunner.cs` - Defines a custom runner for Spectre.Console commands.
  - `TypeRegistrar.cs` - Defines the type registrar for Spectre.Console's dependency injection.
  - `TypeResolver.cs` - Defines the type resolver for Spectre.Console's dependency injection.
  - `Commands/` - Contains CLI command definitions.
    - `DefaultCommand.cs` - Defines the default command executed when no specific command is provided.
    - `InitCommand.cs` - Defines the command for initializing the repository or tool.
    - `ParseCommand.cs` - Defines the command for parsing source code.
    - `SearchCommand.cs` - Defines the command for searching within the repository.
    - `StatusCommand.cs` - Defines the command for displaying the repository status.
    - `SummarizeCommand.cs` - Defines the command for summarizing code or changes.
    - `Cache/` - Contains commands related to managing the inference cache.
      - `ClearCacheCommand.cs` - Defines the command to clear the inference cache.
      - `ShowCacheCommand.cs` - Defines the command to show the contents of the inference cache.
    - `Common/` - Contains common settings or base classes for commands.
      - `CommonSettings.cs` - Defines common settings applicable to multiple commands.
    - `Index/` - Contains commands related to managing the changelog index.
      - `ClearIndexCommand.cs` - Defines the command to clear the changelog index.
      - `ShowIndexCommand.cs` - Defines the command to show the changelog index.
      - `UpdateIndexCommand.cs` - Defines the command to update the changelog index.
    - `Language/` - Contains commands related to managing language configurations.
      - `AddLanguageCommand.cs` - Defines the command to add a new language configuration.
      - `ListLanguageCommand.cs` - Defines the command to list configured languages.
      - `RemoveLanguageCommand.cs` - Defines the command to remove a language configuration.
      - `Available/` - Contains commands related to available (built-in) language support.
        - `AddAvailableLanguageCommand.cs` - Defines the command to add a language from the available list.
        - `ListAvailableLanguagesCommand.cs` - Defines the command to list available languages.
    - `DefaultCommand.cs` - Defines the default command executed when no specific command is provided.
    - `InitCommand.cs` - Defines the command for initializing the repository or tool.
    - `ParseCommand.cs` - Defines the command for parsing source code.
    - `SearchCommand.cs` - Defines the command for searching within the repository.
    - `StatusCommand.cs` - Defines the command for displaying the repository status.
    - `SummarizeCommand.cs` - Defines the command for summarizing code or changes.
    - `Cache/` - Contains commands related to managing the inference cache.
      - `ClearCacheCommand.cs` - Defines the command to clear the inference cache.
      - `ShowCacheCommand.cs` - Defines the command to show the contents of the inference cache.
    - `Common/` - Contains common settings or base classes for commands.
      - `CommonSettings.cs` - Defines common settings applicable to multiple commands.
    - `Index/` - Contains commands related to managing the changelog index.
      - `ClearIndexCommand.cs` - Defines the command to clear the changelog index.
      - `ShowIndexCommand.cs` - Defines the command to show the changelog index.
      - `UpdateIndexCommand.cs` - Defines the command to update the changelog index.
    - `Language/` - Contains commands related to managing language configurations.
      - `AddLanguageCommand.cs` - Defines the command to add a new language configuration.
      - `ListLanguageCommand.cs` - Defines the command to list configured languages.
      - `RemoveLanguageCommand.cs` - Defines the command to remove a language configuration.
      - `Available/` - Contains commands related to available (built-in) language support.
        - `AddAvailableLanguageCommand.cs` - Defines the command to add a language from the available list.
        - `ListAvailableLanguagesCommand.cs` - Defines the command to list available languages.
