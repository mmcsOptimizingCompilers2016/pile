namespace OptimizingCompilers2016.Library.Semilattice
{
    /// <summary>
    /// Интерфейс Полурешётка
    /// Классы, реализующие этот интерфейс должны предоставлять 
    /// реализацию оператора сбора (Collect)
    /// </summary>
    /// <typeparam name="T">
    /// Множество, на котором вводится полурешётка 
    /// (например: мн-во всех подмножеств множетва определений в случае
    /// задачи о достигающих определениях)
    /// </typeparam>
    interface Semilattice<T>
    {
        T Collect(T x, T y);
    }
}
