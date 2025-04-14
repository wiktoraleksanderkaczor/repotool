# Scriban Template Formatting Conventions

This document outlines the formatting conventions used for the Scriban templates (`.sbn` files) within this directory (`RepoTool/Resources/Templates/`) and its subdirectories (e.g., `Partials/Parsing/`). These conventions ensure consistent and predictable output generation.

## Goals

1.  **Compact Output:** The primary goal is to generate output with minimal unnecessary whitespace, especially extra blank lines between logical sections or list items.
2.  **Consistent Indentation:** The rendered output should use appropriate indentation (often 2 spaces) for nested content levels where logical structure dictates (e.g., lists, code blocks).
3.  **Template Readability:** The Scriban template source code itself should remain easy to read and maintain through logical indentation of control structures.

## Techniques

*   **Whitespace Control:** Scriban's whitespace control characters are crucial for achieving compact output:
    *   `~` (Tilde): This is the most commonly used character in these templates. It removes whitespace (including newlines) *before and/or after* a Scriban tag (`{{ ... }}`).
        *   It is frequently placed at the *beginning* of a tag (`{{~ ... }}`) to consume preceding whitespace and the newline from the previous line, effectively joining the tag's output with the previous line or preventing an empty line before the tag.
        *   It's also used *within* block tags (e.g., `{{~ for item in list ~}}`, `{{~ end ~}}`) to control whitespace around the block structure itself.
        *   Example from `parsing.sbn`: `{{~ # CURRENT FULL FILE PATH ~}}` prevents a blank line before the "File Path:" output. The `{{~ for ... ~}}` and `{{~ end ~}}` control spacing around the loop.
    *   `-` (Hyphen): Removes whitespace *only* on the side it's placed within a tag (e.g., `{{- ... }}` removes leading whitespace, `{{ ... -}}` removes trailing whitespace). While standard Scriban, the `~` is generally preferred in these templates for more aggressive whitespace removal.

*   **Output Indentation vs. Template Indentation:**
    *   **Output Indentation:** The desired indentation in the final *rendered output* (e.g., 2 spaces for list items) is achieved by carefully placing literal spaces within the template content *outside* of Scriban tags, combined with the strategic use of whitespace control (`~`) to prevent unwanted newlines.
    *   **Template Indentation:** For improved readability of the template *source code*, Scriban logic blocks (e.g., `{{ if condition }}`, `{{ for item in list }}`, `{{ end }}`) are indented. This visual structure makes the template logic easier to follow but *does not* directly translate to indentation in the rendered output; output indentation is controlled separately as described above. For example, the content inside a `{{~ for ... ~}}` loop in the template might be indented for readability, while the actual output indentation is controlled by literal spaces before the content being looped over.

*   **Escaping Content:** When injecting text content (like code snippets, file content, or JSON schemas) that might contain characters problematic for the surrounding format (e.g., backticks in markdown, quotes in JSON), use the `string.escape` filter. This ensures the content is properly escaped.
    *   Example from `parsing.sbn`: `{{ line.Content | string.escape }}` ensures code lines are safely included.
    *   Example from `changelog.sbn`: `{{ change.PatchContent | string.escape }}` ensures diff content is safely included.
    *   Example from `summarization.sbn`: `{{ JsonSchema | string.escape }}` ensures the JSON schema itself is correctly embedded.

These techniques work together to produce clean, compact, and consistently formatted output while keeping the underlying templates maintainable.