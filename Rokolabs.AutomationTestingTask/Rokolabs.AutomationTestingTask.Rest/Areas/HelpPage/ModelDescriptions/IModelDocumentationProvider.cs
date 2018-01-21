using System;
using System.Reflection;

namespace Rokolabs.AutomationTestingTask.Rest.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}