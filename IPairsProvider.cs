namespace BitocCore1
{
    public interface IPairsProvider
    {
        /// <summary>
        /// Считает общее количество пар на основании коллекции валют
        /// </summary>
        /// <returns>количество пар</returns>
        uint Count();

        /// <summary>
        /// Формирует коллекцию пар на основании коллекции валют и номера страницы пагинации (page).
        /// Для теста на одной странице выводим по 20 пар.
        /// </summary>
        /// <param name="page">страница пагинации</param>
        /// <returns>коллекция пар</returns>
        IEnumerable<string> GetPairs(int page);
    }
}
