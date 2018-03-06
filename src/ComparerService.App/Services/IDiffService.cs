using ComparerService.App.Models;

namespace ComparerService.App.Services
{
    public interface IDiffService
    {
        DiffResult SimpleDiff(string left, string right);
    }
}