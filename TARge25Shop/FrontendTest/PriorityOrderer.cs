using Xunit.Abstractions;
using Xunit.Sdk;


namespace FrontendTest;

public class OrderAttribute : Attribute
{
    public int OrderValue { get; }
    public OrderAttribute(int orderValue)
    {
        this.OrderValue = orderValue;
    }
}

public class PriorityOrderer : ITestCaseOrderer
{
    // https://learn.microsoft.com/en-us/dotnet/core/testing/order-unit-tests?pivots=xunit
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) 
        where TTestCase : ITestCase
    {
        string assemplyName = typeof(OrderAttribute).AssemblyQualifiedName!;
        var sortedMethods = new SortedDictionary<int, List<TTestCase>>();

        foreach (TTestCase testCase in testCases)
        {
            int priority = testCase.TestMethod.Method
                .GetCustomAttributes(assemplyName)
                .FirstOrDefault()
                ?.GetNamedArgument<int>(nameof(OrderAttribute.OrderValue)) ?? 0;
            
            GetOrCreate(sortedMethods, priority).Add(testCase);
        }

        foreach (TTestCase testCase in sortedMethods.Keys.SelectMany(priority =>
                     sortedMethods[priority].OrderBy(test => test.TestMethod.Method.Name)))
        {
            yield return testCase;
        }
    }
    
    private static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : struct
        where TValue : new() =>
        dictionary.TryGetValue(key, out TValue? result)
            ? result
            : (dictionary[key] = new TValue());
}

