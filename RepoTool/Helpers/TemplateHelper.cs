// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Reflection;
using RepoTool.Constants;
using RepoTool.Enums.Inference;
using RepoTool.Extensions;
using RepoTool.Models.Inference;
using RepoTool.Models.Inference.Contexts.Common;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;
using Spectre.Console;

namespace RepoTool.Helpers
{
    internal static class TemplateHelper
    {
        public static string FillMessageTemplateWithContext<TContext>(TemplateData<TContext> data)
            where TContext : notnull, InferenceContext
        {
            string messageTemplate = GetMessageTemplateForReason(data.Request.InferenceReason);
            Template template = Template.Parse(messageTemplate);
            TemplateContext context = new()
            {
                AutoIndent = true,
                TemplateLoader = new TemplateLoader(),
                MemberRenamer = member => member.Name,
            };
            ScriptObject scriptObject = [];
            scriptObject.Import(
                data,
                renamer: member => member.Name,
                // Filter out any method that returns void 
                filter: member =>
                    member is not MethodInfo method
                    || method.ReturnType != typeof(void));

            // DEBUG: Show the imported data object for schema
            // scriptObject.ToJson().DisplayAsJson(Color.Aqua);

            context.PushGlobal(scriptObject);
            try
            {
                // object obj = template.Evaluate(context);
                string output = template.Render(context);
                return output;
            }
            catch ( Exception ex )
            {
                scriptObject.ToJson().DisplayAsJson(Color.Yellow);
                AnsiConsole.WriteException(ex);
                throw;
            }
        }

        private static string GetMessageTemplateForReason(EnInferenceReason inferenceReason)
        {
            return inferenceReason switch
            {
                EnInferenceReason.Changelog => ResourceHelper.GetResourceContent(TemplateConstants.ChangelogTemplate)
                                        ?? throw new FileNotFoundException("Changelog message template file not found."),
                EnInferenceReason.Summarization => ResourceHelper.GetResourceContent(TemplateConstants.SummarizationTemplate)
                                        ?? throw new FileNotFoundException("Summarization message template file not found."),
                EnInferenceReason.Parsing => ResourceHelper.GetResourceContent(TemplateConstants.ParsingTemplate)
                                        ?? throw new FileNotFoundException("Parsing message template file not found."),
                EnInferenceReason.Unknown or _ => throw new ArgumentException("Invalid inference reason"),
            };
        }
    }

    internal sealed class TemplateLoader : ITemplateLoader
    {
        public string GetPath(TemplateContext context, SourceSpan callerSpan, string templateName)
        {
            List<string> resourceNames = ResourceHelper.GetTemplateResourceNames();
            string templateResource = templateName.Replace('/', '.');
            List<string> matches = resourceNames
                .Where(r =>
                    r.EndsWith($"{templateResource}.sbn", StringComparison.InvariantCulture))
                .ToList();
            if ( matches.Count > 1 )
            {
                throw new InvalidOperationException("Multiple templates found. Please use a more specific template name.");
            }
            else if ( matches.Count == 0 )
            {
                throw new FileNotFoundException("Template not found.");
            }

            return matches.First();
        }

        public string Load(TemplateContext context, SourceSpan callerSpan, string templatePath)
        {
            return ResourceHelper.GetResourceContent(templatePath)
                ?? throw new FileNotFoundException("Template content not found.");
        }

        public ValueTask<string> LoadAsync(TemplateContext context, SourceSpan callerSpan, string templatePath) => new(Load(context, callerSpan, templatePath));
    }
}
