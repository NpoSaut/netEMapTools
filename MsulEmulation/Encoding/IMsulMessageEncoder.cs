using MsulEmulation.ViewModels;
using ReactiveUI;

namespace MsulEmulation.Encoding
{
    public interface IMsulMessageEncoder
    {
        MsulMessage GetMessage(MsulEmulationParametersViewModel ViewModel);
    }
}
