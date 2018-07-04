using Tenderfoot.Mvc;

namespace MiniCmsLibrary.Interfaces
{
    public interface ILogin : ITfModel
    {
        string username { get; }
        string password { get; }
    }
}