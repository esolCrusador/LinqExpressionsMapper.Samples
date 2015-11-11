namespace LinqExpressionsMapper.Samples.CustomMappers
{
    public interface IMapProperties<in TSource, in TDest>
    {
        void Map(TSource source, TDest dest);
    }
}
