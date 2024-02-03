namespace Letu.ChatPdf;

public interface IEmbbedingService
{
    Task<float[]> GetVectorAsync(string content);
}
