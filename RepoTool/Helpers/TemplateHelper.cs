using HandlebarsDotNet;
using RepoTool.Enums.Inference;
using RepoTool.Constants;

namespace RepoTool.Helpers
{
    public static class TemplateHelper
    {
        public static string FillMessageTemplateWithContext<TContext>(TemplateData<TContext> context) 
            where TContext : notnull, InferenceContext
        {
            // INFO: https://github.com/Handlebars-Net/Handlebars.Net/issues/167
            string messageTemplate = GetMessageTemplateForReason(context.Request.GetInferenceReason());
            HandlebarsTemplate<object, string> template = Handlebars.Compile(messageTemplate);
            return template(context);
        }

        private static string GetMessageTemplateForReason(EnInferenceReason inferenceReason)
        {
            switch (inferenceReason)
            {
                case EnInferenceReason.Changelog:
                    return ResourceHelper.GetResourceContent(TemplateConstants.ChangelogTemplate)
                        ?? throw new FileNotFoundException("Changelog message template file not found.");
                case EnInferenceReason.Summarization:
                    return ResourceHelper.GetResourceContent(TemplateConstants.SummarizationTemplate) 
                        ?? throw new FileNotFoundException("Summarization message template file not found.");
                case EnInferenceReason.Parsing:
                    return ResourceHelper.GetResourceContent(TemplateConstants.ParsingTemplate)
                        ?? throw new FileNotFoundException("Parsing message template file not found.");
                default:
                    throw new ArgumentException("Invalid inference reason");
            }
        }
    }
}