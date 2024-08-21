namespace EnchCoreApi.Common.CSharp.MSBuild
{
    public interface IProjectLoader<TModule> where TModule : IProjectModule
    {
        public void Load(TModule data);
    }
    public interface IProjectModule
    {

    }
}
