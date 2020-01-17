using Microsoft.Extensions.DependencyInjection;

namespace WeShare.Domain
{
    public interface IStudentServices : IScopedAutoDIable
    {
        int GetId();
    }
}