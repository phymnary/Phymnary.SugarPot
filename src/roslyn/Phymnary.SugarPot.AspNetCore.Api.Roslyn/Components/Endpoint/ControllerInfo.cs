using Phymnary.SugarPot.RoslynComponents.Models;

namespace Phymnary.SugarPot.AspNetCore.Api.Roslyn.Components.Endpoint;

internal record ControllerInfo(HierarchyInfo Hierarchy, bool IsStatic);
