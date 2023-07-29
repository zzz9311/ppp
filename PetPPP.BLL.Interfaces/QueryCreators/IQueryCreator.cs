namespace PetPPP.BLL.Interfaces.QueryCreators;

/// <summary>
/// Создает запрос с учетом фильтр
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public interface IQueryCreator<T, TFilter>
{
    public IQueryable<T> Create(TFilter filter);
}