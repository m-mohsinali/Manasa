using Award.Core.ViewModel;
using System.Threading.Tasks;

namespace Award.Core.Interfaces
{

    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
