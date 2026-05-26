using Dapper;
using System.Data;

namespace ApiRefactor.Infrastructure.Data;

public sealed class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value.ToString();
    }

    public override Guid Parse(object value) => Guid.Parse((string)value);
}

public sealed class DateTimeTypeHandler : SqlMapper.TypeHandler<DateTime>
{
    public override void SetValue(IDbDataParameter parameter, DateTime value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value.ToString("o");
    }

    public override DateTime Parse(object value) =>
        DateTime.Parse((string)value, null, System.Globalization.DateTimeStyles.RoundtripKind);
}
