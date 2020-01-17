namespace Microsoft.Extensions.DependencyInjection
{
    public interface IAutoDIable
    {
        
    }

    public interface ISingletonAutoDIable : IAutoDIable
    {

    }
    public interface IScopedAutoDIable : IAutoDIable
    {

    }
    public interface ITransientAutoDIable : IAutoDIable
    {

    }
}