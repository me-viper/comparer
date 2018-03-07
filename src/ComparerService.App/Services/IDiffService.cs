using ComparerService.App.Models;

using JetBrains.Annotations;

namespace ComparerService.App.Services
{
    public interface IDiffService
    {
        DiffResult SimpleDiff([CanBeNull] string left, [CanBeNull] string right);
    }
}