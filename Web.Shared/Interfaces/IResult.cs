namespace Web.Shared.Interfaces
{
    public interface IResult
    {
        List<string> Messages { get; set; }
        bool Succeeded { get; set; }
        string DataJson { get; set; }
        Exception Exception { get; set; }
        int Code { get; set; }
    }

    public interface IResult<T> : IResult
    {
        T Data { get; set; }
    }
}
