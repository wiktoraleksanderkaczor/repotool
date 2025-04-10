Inference will be done by single property or value because it simplifies the structured  output handling on the inference provider side (which usually have subpar support).

Everything that isn't a (JSON Schema) object will be wrapped in a generic wrapper type.

---

Migrating away from Microsoft.AI.Extensions because it does not have flexible enough generation options.

For example, structured output with native provider JSON schema support is supported but only when using generic methods. 

Even if I can pass a custom schema, it doesn't seem to be used or passed directly in the current provider implementation.

I will be going back to using the OpenAI SDK and OllamaSharp directly and will implement the custom schema generation with JsonSchema.Net.

---

Abandoning the use of streaming for now. It is not necessary for the current use case and adds complexity.

---

Moved from Handlebars.NET to Scriban because of better or indeed just existing whitespace control handling when it comes to auto-indentation for templates.

---

After preview language features are added, need to change to following instead of having private backing fields:
public int Index
{
    get;
    init
    {
        if (value < 0)
        {
            throw new ArgumentException("Index cannot be negative.", nameof(Index));
        }
        field = value; // Auto-implemented backing field.
    }
}

It should remove the need to have private backing fields for properties, allowing for cleaner and more concise code.

---

Most useful indexed files will be those of the main branch, then we can just compute the differences and have the full repo one... need to cache those to avoid useless recomputation, also perhaps a filesystem watcher so they're always ready.

---

Eventually, editing will allow removing previous or removing at index or removing by name and inserting versions with index and name so LLM can recover from mistakenly added items. Might be an interesting idea for a `<think>` reasoning too.

---

Future extensions, should be able to check where we are in the file token by token via asking LLM to echo the tokens it has processed, removing all spaces from LLM output and file before comparing... It might not be entirely linear either, i.e. parameters might be after method signature but it doesn't mean they would be processed in that order, something to consider.

---

Some structured generation engines have issues with resolving $ref to $defs. One `SchemaResolver.cs` wouldn't go amiss.

