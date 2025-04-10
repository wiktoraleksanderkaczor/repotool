# TODO

- [ ] Need Polly for retries in InferenceHelper...
- [ ] Need to exchange various Console.WriteLine for ILogger alternatives.
- [ ] Consider making Expression into ExpressionItem to avoid excessive construct commenting.
- [ ] Consider just posting the entire content of documentation (or raw Models.xml) to make Time-to-First-Token absurd but subsequent prompt evaluations faster... 
- [ ] Validation for only one `<System>` message?
- [ ] Implement architecture tests for interfaces like IToolSelector<T> 
- [ ] Have a read through https://www.promptingguide.ai/techniques
- [ ] Add 'Reason' field in the tool choice thingies... so we can log it in Actions.
- [ ] Fix bad property documentation i.e. `System.String` has `Chars` and `Length` which are useless to the LLM.
- [ ] Move stuff related to object inference from ParserHelper to InferenceHelper because they're general purpose and will be used in other places. More of a StructuredOutputHelper really.
- [ ] Need to fill out selectors with ItemChoice for things like `Expressions` etc.
