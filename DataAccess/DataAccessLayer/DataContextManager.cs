using Core;

namespace DataAccess.DataAccessLayer
{
    public class DataContextManager : IDataContextManager
    {
        private readonly DeployHelperContext _deployHelperContext;

        public DataContextManager()
        {
            _deployHelperContext = new DeployHelperContext();
        }

        public DeployHelperContext GetDeployHelperContext()
        {
            return _deployHelperContext;
        }
    }

    public interface IDataContextManager : IDependency
    {
        DeployHelperContext GetDeployHelperContext();
    }
}
