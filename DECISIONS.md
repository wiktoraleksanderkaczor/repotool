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