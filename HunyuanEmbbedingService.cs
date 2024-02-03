using Microsoft.Extensions.Configuration;
using TencentCloud.Common;
using TencentCloud.Hunyuan.V20230901.Models;
using TencentCloud.Hunyuan.V20230901;

namespace Letu.ChatPdf;

public class HunyuanEmbbedingService : IEmbbedingService
{
    Credential cred;
    public HunyuanEmbbedingService()
    {
        // 从UserSecrets中读取环境变量
        // 初始化Configuration
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        cred = new()
        {
            SecretId = configuration["TencentCloud:SecretId"],
            SecretKey = configuration["TencentCloud:SecretKey"]
        };
    }

    public async Task<float[]> GetVectorAsync(string content)
    {
        HunyuanClient client = new(cred, "ap-guangzhou");
        var rep = await client.GetEmbedding(new GetEmbeddingRequest
        {
            Input = content
        });
        if(rep.Data == null || rep.Data.Length == 0 || rep.Data[0].Embedding == null)
        {
            throw new Exception("获取向量失败");
        }
        return rep.Data[0].Embedding.Select(x => x.HasValue ? (float)x : 0.0f).ToArray();
    }
}