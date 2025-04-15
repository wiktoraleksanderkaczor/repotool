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

---

Support for Outlines will only be provided via running vLLM, not their FastAPI Docker image. LM Studio might be supported at a later time.

---

Turns out the JSON Schemas require better handling of references and definitions to ensure compatibility across different implementations. For a couple examples; Gemini does not resolve '$ref', OpenAI seems to use 'type' for enums, Ollama probably does it's own thing.

---

Running vLLM with Turing is proving to be problematic, AWQ is not supported despite their 'Supported Hardware' page... BitsAndBytes is my alternative since it has 4-bit quantization and supposedly runs on Turing... official image errors on launch. I will build my own image with the latest version of vLLM and see if that works. If not, back to CPU speed it is... or try out GPTQ even if it's relatively old. Also, TGI by Huggingface is not an option because it only allows guided decoding through tools. GGUF is completely unoptimized so... pass.

---

vLLM seems to have issues with 'outlines'-based guided decoding, it does not report errors properly through the C# OpenAI SDK. It passes back HTTP 400s but no useful error message. You actually need to go as follows:
```csharp
public partial class ChatClient
{
    /// <summary>
    /// [Protocol Method] Creates a model response for the given chat conversation.
    /// </summary>
    /// <param name="content"> The content to send as the body of the request. </param>
    /// <param name="options"> The request options, which can override default behaviors of the client pipeline on a per-call basis. </param>
    /// <exception cref="ArgumentNullException"> <paramref name="content"/> is null. </exception>
    /// <exception cref="ClientResultException"> Service returned a non-success status code. </exception>
    /// <returns> The response returned from the service. </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual async Task<ClientResult> CompleteChatAsync(BinaryContent content, RequestOptions options = null)
    {
        Argument.AssertNotNull(content, nameof(content));

        using PipelineMessage message = CreateCreateChatCompletionRequest(content, options);
        return ClientResult.FromResponse(await _pipeline.ProcessMessageAsync(message, options).ConfigureAwait(false));
    }
}
```

Then step-into the `ProcessMessageAsync` method and check the `Response` property of the `message` object. It will contain the error message from vLLM.

---

Issue with vLLM structured generation was that my JSON schema within the system prompt was not properly escaped. I have updated the README.md for templates to prevent this from happening again. It produced silent failures.

---

Support for Text Generation Interface (TGI) by Huggingface will be added by using the 'tools' parameter even if it might introduce some useless tokens too. Only way to support that with 'chat' instead of the generate endpoint. 

---

I will mark commented out display code with 'DEBUG' tags where it might be useful for debugging later.