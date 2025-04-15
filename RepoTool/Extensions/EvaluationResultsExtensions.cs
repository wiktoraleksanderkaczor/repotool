using Json.Schema;

public static class EvaluationResultsExtensions
{
    public static void DisplayErrors(this List<EvaluationResults> evaluationResults)
    {
        Dictionary<string, List<string>> fieldToErrorMessage = evaluationResults
            .GroupBy(result => result.InstanceLocation.ToString())
            .ToDictionary(
                result => result.Key,
                result => result.Where(x => x.Errors != null).SelectMany(x => x.Errors!).Select(x => x.Value).ToList());
        foreach (KeyValuePair<string, List<string>> item in fieldToErrorMessage)
        {
            Console.WriteLine(item.Key);
            foreach (string error in item.Value)
            {
                Console.WriteLine($"\t- {error}");
            }
        }
    }

    public static List<EvaluationResults> GatherErrors(this EvaluationResults evaluationResults)
    {
        List<EvaluationResults> errors = new();
        foreach (EvaluationResults result in evaluationResults.Details)
        {
            if (result.IsValid)
            {
                continue;
            }

            if (result.HasErrors)
            {
                errors.Add(result);
            }

            errors.AddRange(result.GatherErrors());
        }
        return errors;
    }
}