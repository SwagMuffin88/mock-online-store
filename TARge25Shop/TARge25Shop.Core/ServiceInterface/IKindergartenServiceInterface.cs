using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core;

public interface IKindergartenServiceInterface
{
    Task<Kindergarten> Create(KindergartenDto dto);
    Task<Kindergarten?> Update(KindergartenDto dto);
    Task<Kindergarten?> DetailAsync(Guid id);
    Task<Kindergarten?> Delete(Guid id);
}