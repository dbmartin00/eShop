using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.ApplicationInsights;

public class ExampleTargetingContextAccessor : ITargetingContextAccessor
{
    private const string TargetingContextLookup = "ExampleTargetingContextAccessor.TargetingContext";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExampleTargetingContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public ValueTask<TargetingContext> GetContextAsync()
    {
        HttpContext httpContext = _httpContextAccessor.HttpContext;
        TelemetryClient telemetryClient = httpContext.RequestServices.GetRequiredService<TelemetryClient>();        
        if (httpContext.Items.TryGetValue(TargetingContextLookup, out object value))
        {
            return new ValueTask<TargetingContext>((TargetingContext)value);
        }
        List<string> groups = new List<string>();
        if (httpContext.User.Identity.Name != null)
        {
            groups.Add(httpContext.User.Identity.Name.Split("@", StringSplitOptions.None)[1]);
        }
        TargetingContext targetingContext = new TargetingContext
        {
            UserId = telemetryClient.Context.User.AuthenticatedUserId, // DBM httpContext.User.Identity.Name,
            Groups = groups
        };
        httpContext.Items[TargetingContextLookup] = targetingContext;
        return new ValueTask<TargetingContext>(targetingContext);
    }
}
