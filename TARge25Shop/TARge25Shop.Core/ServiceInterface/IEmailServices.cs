using TARge25Shop.Core.Dto.email;

namespace TARge25Shop.Core.ServiceInterface;

public interface IEmailServices
{
    void SendEmail(EmailDto dto);
}